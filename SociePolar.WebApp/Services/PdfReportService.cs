using Microsoft.JSInterop;
using QuestPDF.Fluent;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Domain.Interfaces;
using SociePolar.WebApp.DocumentsClass;
using SociePolar.WebApp.Utilities;
using Microsoft.AspNetCore.Hosting;

namespace SociePolar.WebApp.Services
{
    public class PdfReportService
    {
        private readonly ISociedad _repository;
        private readonly IAutoridad _repoAutoridad;
        private readonly IAsamblea _repoAsamblea;
        private readonly ICertificacion _repoCertificacion;
        private readonly ILibroSocietario _repoLibros;
        private readonly IAccionista _repoAccionista;
        private readonly IAccionistaSociedad _repoAccionistaSociedad;
        private readonly IRepositoryFactory _repoFactory;
        private readonly IJSRuntime _js;
        private readonly IWebHostEnvironment _env;

        public PdfReportService(
            ISociedad repository, IAutoridad repoAutoridad, IAsamblea repoAsamblea,
            ICertificacion repoCertificacion, ILibroSocietario repoLibros,
            IAccionista repoAccionista, IAccionistaSociedad repoAccionistaSociedad,
            IRepositoryFactory repoFactory,
            IJSRuntime js, IWebHostEnvironment env)
        {
            _repository = repository;
            _repoAutoridad = repoAutoridad;
            _repoAsamblea = repoAsamblea;
            _repoCertificacion = repoCertificacion;
            _repoLibros = repoLibros;
            _repoAccionista = repoAccionista;
            _repoAccionistaSociedad = repoAccionistaSociedad;
            _repoFactory = repoFactory;
            _js = js;
            _env = env;
        }

        public async Task<string> GenerarYDescargarReporteSociedadAsync(int sociedadId)
        {
            // 1. Extracción de los datos de los repositorios
            var sociedad = await _repository.GetByIdAsync(sociedadId);
            if (sociedad == null)
            {
                throw new Exception($"No se encontró la sociedad con Id {sociedadId}");
            }

            var autoridadesAll = await _repoAutoridad.GetBySociedadIdAsync(sociedadId);
            var autoridades = autoridadesAll
                .OrderBy(x => x.Cargo?.Orden ?? 999)
                .ThenBy(x => x.Cargo?.Nombre ?? "")
                .ThenBy(x => x.Nombre ?? "")
                .ToList();
            
            var asambleasAll = await _repoAsamblea.GetBySociedadIdAsync(sociedadId);
            var asambleas = asambleasAll
                .Where(x => x.IndicadorAsamblea == 1)
                .OrderByDescending(x => x.FechaCelebracion)
                .ToList();

            var reformas = asambleasAll
                .Where(x => (x.IndicadorAsamblea == 0 || x.TipoReforma != null))
                .OrderByDescending(x => x.FechaCelebracion)
                .ToList();

            var certificacionesAll = await _repoCertificacion.GetBySociedadIdAsync(sociedadId);
            var certificaciones = certificacionesAll
                .OrderByDescending(x => x.Fecha)
                .ToList();

            var librosAll = await _repoLibros.GetBySociedadIdAsync(sociedadId);
            var libros = librosAll
                .OrderByDescending(x => x.FechaDesde)
                .ToList();

            // 2. Cargar los bytes de la imagen desde wwwroot de manera segura
            byte[] logoBytes = Array.Empty<byte>();
            string rutaLogo = Path.Combine(_env.WebRootPath, "images", "LogoPolar.jpg");

            if (File.Exists(rutaLogo))
            {
                logoBytes = await File.ReadAllBytesAsync(rutaLogo);
            }

            // 3. Instanciación del documento pasando los bytes del logo
            var document = new SociedadReportDocument(sociedad, autoridades, asambleas, reformas, certificaciones, libros, logoBytes);

            // 4. Renderizado a bytes PDF
            var pdfBytes = document.GeneratePdf();

            // 5. Nombre de archivo limpio (sin comas ni símbolos que corrompan la extensión en Windows/Chrome)
            string nombreEmpresa = sociedad.Empresa?.Nombre ?? $"Sociedad_{sociedadId}";
            string nombreLimpio = System.Text.RegularExpressions.Regex.Replace(nombreEmpresa, @"[^\w\s-]", "").Replace(" ", "_").Trim();
            if (string.IsNullOrWhiteSpace(nombreLimpio)) nombreLimpio = $"Sociedad_{sociedadId}";
            string nombreArchivo = $"Reporte_{nombreLimpio}_{DateTime.Now:yyyyMMdd}.pdf";

            // Guardar copia física en wwwroot/reports para acceso y apertura directa
            string reportsFolder = Path.Combine(_env.WebRootPath, "reports");
            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }
            string physicalPath = Path.Combine(reportsFolder, nombreArchivo);
            await File.WriteAllBytesAsync(physicalPath, pdfBytes);

            string relativeUrl = $"reports/{nombreArchivo}";

            // 6. Invocar JavaScript para abrir en visor de pestaña y descargar
            string base64Data = Convert.ToBase64String(pdfBytes);
            await _js.InvokeVoidAsync("descargarYAbrirPdf", base64Data, nombreArchivo, relativeUrl);

            return relativeUrl;
        }

        public async Task<string> GenerarYDescargarReporteAccionistaAsync(int accionistaId)
        {
            var accionista = await _repoAccionista.GetByIdAsync(accionistaId);
            if (accionista == null)
            {
                throw new Exception($"No se encontró el accionista con Id {accionistaId}");
            }

            // Obtener catálogo de tipos de documento para resolver los IDs enteros (Cónyuge, Apoderado, Rep. Legal)
            Dictionary<int, string> tiposDocDict = new();
            try
            {
                var repoTipoDoc = _repoFactory.GetRepository("tipodocumento");
                var method = repoTipoDoc.GetType().GetMethod("GetAllAsync");
                if (method != null)
                {
                    var task = (Task)method.Invoke(repoTipoDoc, null)!;
                    await task;
                    var resultProperty = task.GetType().GetProperty("Result");
                    if (resultProperty?.GetValue(task) is IEnumerable<IBaseEntity> listaTipos)
                    {
                        tiposDocDict = listaTipos.ToDictionary(k => k.Id, v => v.Nombre ?? "");
                    }
                }
            }
            catch
            {
                // Ignorar en caso de fallo al resolver catálogo secundario
            }

            // Obtener participaciones en sociedades
            var relaciones = await _repoAccionistaSociedad.GetByAccionistaIdAsync(accionistaId);
            List<AccionistaSociedadVerDto> participaciones = new();
            foreach (var item in relaciones.OrderBy(x => x.Sociedad?.Empresa?.Nombre ?? ""))
            {
                long nroAcciones = item.NroAcciones ?? 0;
                decimal porcParticipa = 0;
                if (item.Sociedad != null && !string.IsNullOrWhiteSpace(item.Sociedad.NumeroAcciones) &&
                    long.TryParse(item.Sociedad.NumeroAcciones, out long totalAccionesSoc) && totalAccionesSoc > 0 && nroAcciones > 0)
                {
                    porcParticipa = Math.Round((decimal)nroAcciones / totalAccionesSoc * 100, 6);
                }

                participaciones.Add(new AccionistaSociedadVerDto
                {
                    Id = item.Id,
                    SociedadId = item.Sociedad?.Id ?? 0,
                    SociedadNombre = item.Sociedad?.Empresa?.Nombre ?? "-",
                    DocumentoIdentidad = UtilClass.ObtenerDocumentoCompleto(item.Sociedad?.TipoDocumento1?.Nombre, item.Sociedad?.Documento1),
                    EstatusSociedadNombre = item.Sociedad?.EstatusSociedad?.Nombre ?? "-",
                    TipoAccionistaNombre = item.EstatusAccionista?.Nombre ?? "-",
                    NroAcciones = nroAcciones,
                    Participacion = porcParticipa,
                    CreateDate = item.CreateDate,
                    UpdateDate = item.UpdateDate
                });
            }

            byte[] logoBytes = Array.Empty<byte>();
            string rutaLogo = Path.Combine(_env.WebRootPath, "images", "LogoPolar.jpg");
            if (File.Exists(rutaLogo))
            {
                logoBytes = await File.ReadAllBytesAsync(rutaLogo);
            }

            var document = new AccionistaReportDocument(accionista, participaciones, tiposDocDict, logoBytes);
            var pdfBytes = document.GeneratePdf();

            string nombreBase = accionista.TipoAccionista?.Id == 3
                ? (accionista.NombreSusesion ?? accionista.Nombre ?? $"Accionista_{accionistaId}")
                : (accionista.Nombre ?? $"Accionista_{accionistaId}");
            if (nombreBase.Length > 40) nombreBase = nombreBase.Substring(0, 40);
            string nombreLimpio = System.Text.RegularExpressions.Regex.Replace(nombreBase, @"[^\w\s-]", "").Replace(" ", "_").Trim();
            if (string.IsNullOrWhiteSpace(nombreLimpio)) nombreLimpio = $"Accionista_{accionistaId}";
            string nombreArchivo = $"Reporte_Accionista_{nombreLimpio}_{DateTime.Now:yyyyMMdd}.pdf";

            string reportsFolder = Path.Combine(_env.WebRootPath, "reports");
            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }
            string physicalPath = Path.Combine(reportsFolder, nombreArchivo);
            await File.WriteAllBytesAsync(physicalPath, pdfBytes);

            string relativeUrl = $"reports/{nombreArchivo}";

            string base64Data = Convert.ToBase64String(pdfBytes);
            await _js.InvokeVoidAsync("descargarYAbrirPdf", base64Data, nombreArchivo, relativeUrl);

            return relativeUrl;
        }

        public async Task<string> GenerarYDescargarReporteListaAsync(
            string titulo,
            string[] columnas,
            List<string[]> filas,
            string nombreArchivoBase,
            float[]? anchosRelativos = null,
            string? subtitulo = null,
            bool landscape = true)
        {
            byte[] logoBytes = Array.Empty<byte>();
            string rutaLogo = Path.Combine(_env.WebRootPath, "images", "LogoPolar.jpg");
            if (File.Exists(rutaLogo))
            {
                logoBytes = await File.ReadAllBytesAsync(rutaLogo);
            }

            var document = new ListaReportDocument(titulo, subtitulo, columnas, filas, anchosRelativos, logoBytes, landscape);
            var pdfBytes = document.GeneratePdf();

            string nombreLimpio = System.Text.RegularExpressions.Regex.Replace(nombreArchivoBase, @"[^\w\s-]", "").Replace(" ", "_").Trim();
            if (string.IsNullOrWhiteSpace(nombreLimpio)) nombreLimpio = "Reporte_Lista";
            string nombreArchivo = $"{nombreLimpio}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            string reportsFolder = Path.Combine(_env.WebRootPath, "reports");
            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }
            string physicalPath = Path.Combine(reportsFolder, nombreArchivo);
            await File.WriteAllBytesAsync(physicalPath, pdfBytes);

            string relativeUrl = $"reports/{nombreArchivo}";

            string base64Data = Convert.ToBase64String(pdfBytes);
            await _js.InvokeVoidAsync("descargarYAbrirPdf", base64Data, nombreArchivo, relativeUrl);

            return relativeUrl;
        }

        public async Task<string> GenerarYDescargarReporteListadoAccionistasCorteAsync(
            SociedadEncabezadoReporteDto encabezado,
            DateTime fechaCorte,
            List<AccionistaListadoCorteDto> accionistas,
            string nombreArchivoBase)
        {
            byte[] logoBytes = Array.Empty<byte>();
            string rutaLogo = Path.Combine(_env.WebRootPath, "images", "LogoPolar.jpg");
            if (File.Exists(rutaLogo))
            {
                logoBytes = await File.ReadAllBytesAsync(rutaLogo);
            }

            var document = new ListaAccionistasCorteReportDocument(encabezado, fechaCorte, accionistas, logoBytes);
            var pdfBytes = document.GeneratePdf();

            string nombreLimpio = System.Text.RegularExpressions.Regex.Replace(nombreArchivoBase, @"[^\w\s-]", "").Replace(" ", "_").Trim();
            if (string.IsNullOrWhiteSpace(nombreLimpio)) nombreLimpio = "Listado_Accionistas";
            string nombreArchivo = $"{nombreLimpio}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            string reportsFolder = Path.Combine(_env.WebRootPath, "reports");
            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }
            string physicalPath = Path.Combine(reportsFolder, nombreArchivo);
            await File.WriteAllBytesAsync(physicalPath, pdfBytes);

            string relativeUrl = $"reports/{nombreArchivo}";

            string base64Data = Convert.ToBase64String(pdfBytes);
            await _js.InvokeVoidAsync("descargarYAbrirPdf", base64Data, nombreArchivo, relativeUrl);

            return relativeUrl;
        }

        public async Task<string> GenerarYDescargarReporteDividendosAsync(
            DividendoEncabezadoReporteDto encabezado,
            List<DividendoListadoReporteDto> detalles,
            string nombreArchivoBase)
        {
            byte[] logoBytes = Array.Empty<byte>();
            string rutaLogo = Path.Combine(_env.WebRootPath, "images", "LogoPolar.jpg");
            if (File.Exists(rutaLogo))
            {
                logoBytes = await File.ReadAllBytesAsync(rutaLogo);
            }

            var document = new DividendoReportDocument(encabezado, detalles, logoBytes);
            var pdfBytes = document.GeneratePdf();

            string nombreLimpio = System.Text.RegularExpressions.Regex.Replace(nombreArchivoBase, @"[^\w\s-]", "").Replace(" ", "_").Trim();
            if (string.IsNullOrWhiteSpace(nombreLimpio)) nombreLimpio = "Reporte_Dividendos";
            string nombreArchivo = $"{nombreLimpio}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            string reportsFolder = Path.Combine(_env.WebRootPath, "reports");
            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }
            string physicalPath = Path.Combine(reportsFolder, nombreArchivo);
            await File.WriteAllBytesAsync(physicalPath, pdfBytes);

            string relativeUrl = $"reports/{nombreArchivo}";

            string base64Data = Convert.ToBase64String(pdfBytes);
            await _js.InvokeVoidAsync("descargarYAbrirPdf", base64Data, nombreArchivo, relativeUrl);

            return relativeUrl;
        }

        public async Task<string> GenerarYDescargarReporteConciliacionesAsync(
            ConciliacionEncabezadoReporteDto encabezado,
            List<ConciliacionListadoReporteDto> detalles,
            string nombreArchivoBase)
        {
            byte[] logoBytes = Array.Empty<byte>();
            string rutaLogo = Path.Combine(_env.WebRootPath, "images", "LogoPolar.jpg");
            if (File.Exists(rutaLogo))
            {
                logoBytes = await File.ReadAllBytesAsync(rutaLogo);
            }

            var document = new ConciliacionReportDocument(encabezado, detalles, logoBytes);
            var pdfBytes = document.GeneratePdf();

            string nombreLimpio = System.Text.RegularExpressions.Regex.Replace(nombreArchivoBase, @"[^\w\s-]", "").Replace(" ", "_").Trim();
            if (string.IsNullOrWhiteSpace(nombreLimpio)) nombreLimpio = "Reporte_Conciliaciones";
            string nombreArchivo = $"{nombreLimpio}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            string reportsFolder = Path.Combine(_env.WebRootPath, "reports");
            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }
            string physicalPath = Path.Combine(reportsFolder, nombreArchivo);
            await File.WriteAllBytesAsync(physicalPath, pdfBytes);

            string relativeUrl = $"reports/{nombreArchivo}";

            string base64Data = Convert.ToBase64String(pdfBytes);
            await _js.InvokeVoidAsync("descargarYAbrirPdf", base64Data, nombreArchivo, relativeUrl);

            return relativeUrl;
        }

        public async Task<string> GenerarYDescargarCertificacionCartaAsync(
            CertificacionReportDto dto,
            string nombreArchivoBase)
        {
            byte[] logoBytes = Array.Empty<byte>();
            string rutaLogo = Path.Combine(_env.WebRootPath, "images", "LogoCertificacion.jpg");
            if (!File.Exists(rutaLogo))
            {
                rutaLogo = Path.Combine(_env.WebRootPath, "images", "LogoPolar.jpg");
            }
            if (File.Exists(rutaLogo))
            {
                logoBytes = await File.ReadAllBytesAsync(rutaLogo);
            }

            var document = new CertificacionCartaReportDocument(dto, logoBytes);
            var pdfBytes = document.GeneratePdf();

            string nombreLimpio = System.Text.RegularExpressions.Regex.Replace(nombreArchivoBase, @"[^\w\s-]", "").Replace(" ", "_").Trim();
            if (string.IsNullOrWhiteSpace(nombreLimpio)) nombreLimpio = "Certificacion";
            string nombreArchivo = $"{nombreLimpio}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            string reportsFolder = Path.Combine(_env.WebRootPath, "reports");
            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }
            string physicalPath = Path.Combine(reportsFolder, nombreArchivo);
            await File.WriteAllBytesAsync(physicalPath, pdfBytes);

            string relativeUrl = $"reports/{nombreArchivo}";

            string base64Data = Convert.ToBase64String(pdfBytes);
            await _js.InvokeVoidAsync("descargarYAbrirPdf", base64Data, nombreArchivo, relativeUrl);

            return relativeUrl;
        }
    }
}
