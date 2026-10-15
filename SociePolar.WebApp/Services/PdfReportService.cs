using Microsoft.JSInterop;
using QuestPDF.Fluent;
using SociePolar.Application.Interfaces;
using SociePolar.WebApp.DocumentsClass;
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
        private readonly IJSRuntime _js;
        private readonly IWebHostEnvironment _env;

        public PdfReportService(
            ISociedad repository, IAutoridad repoAutoridad, IAsamblea repoAsamblea,
            ICertificacion repoCertificacion, ILibroSocietario repoLibros, IJSRuntime js, IWebHostEnvironment env)
        {
            _repository = repository;
            _repoAutoridad = repoAutoridad;
            _repoAsamblea = repoAsamblea;
            _repoCertificacion = repoCertificacion;
            _repoLibros = repoLibros;
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

            var autoridadesAll = await _repoAutoridad.GetAllAsync();
            var autoridades = autoridadesAll
                .Where(x => x.Sociedad?.Id == sociedadId)
                .OrderBy(x => x.Sociedad?.Empresa?.Nombre ?? "")
                .ToList();

            var asambleasAll = await _repoAsamblea.GetAllAsync();
            var asambleas = asambleasAll
                .Where(x => x.Sociedad?.Id == sociedadId && x.Sociedad?.EstatusSociedad?.Id == 1 && x.IndicadorAsamblea == 1)
                .OrderBy(x => x.FechaCelebracion)
                .ToList();

            var reformas = asambleasAll
                .Where(x => x.Sociedad?.Id == sociedadId && x.Sociedad?.EstatusSociedad?.Id == 1 && (x.IndicadorAsamblea == 0 || x.TipoReforma != null))
                .OrderBy(x => x.FechaCelebracion)
                .ToList();

            var certificacionesAll = await _repoCertificacion.GetAllAsync();
            var certificaciones = certificacionesAll
                .Where(x => x.Sociedad?.Id == sociedadId && x.Sociedad?.EstatusSociedad?.Id == 1)
                .OrderBy(x => x.Fecha)
                .ToList();

            var librosAll = await _repoLibros.GetAllAsync();
            var libros = librosAll
                .Where(x => x.Sociedad?.Id == sociedadId && x.Sociedad?.EstatusSociedad?.Id == 1)
                .OrderBy(x => x.ClaseLibro?.Nombre ?? "")
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
    }
}
