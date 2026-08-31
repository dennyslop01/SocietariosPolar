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

        public async Task GenerarYDescargarReporteSociedadAsync(int sociedadId)
        {
            // 1. Extracción de los datos de los repositorios
            var sociedad = await _repository.GetByIdAsync(sociedadId);
            var autoridadesAll = await _repoAutoridad.GetAllAsync();
            var autoridades = autoridadesAll.Where(x => x.Sociedad.Id == sociedadId).OrderBy(x => x.Sociedad.Empresa.Id).ToList();

            var asambleasAll = await _repoAsamblea.GetAllAsync();
            var asambleas = asambleasAll.OrderBy(x => x.Sociedad?.Empresa?.Id)
                                        .Where(x => x.Sociedad?.EstatusSociedad?.Id == 1 && x.IndicadorAsamblea == 1 && x.Sociedad.Id == sociedadId).ToList();

            var reformas = asambleasAll.OrderBy(x => x.Sociedad?.Empresa?.Id)
                                       .Where(x => x.Sociedad?.EstatusSociedad?.Id == 1 && x.Sociedad.Id == sociedadId && (x.IndicadorAsamblea == 0 || x.TipoReforma != null)).ToList();

            var certificacionesAll = await _repoCertificacion.GetAllAsync();
            var certificaciones = certificacionesAll.OrderBy(x => x.Sociedad.Empresa.Id)
                                                    .Where(x => x.Sociedad?.EstatusSociedad?.Id == 1 && x.Sociedad.Id == sociedadId).ToList();

            var librosAll = await _repoLibros.GetAllAsync();
            var libros = librosAll.OrderBy(x => x.Sociedad?.Empresa?.Id).Where(x => x.Sociedad?.EstatusSociedad?.Id == 1 && x.Sociedad.Id == sociedadId).ToList();

            // 2. Cargar los bytes de la imagen desde wwwroot de manera segura
            byte[] logoBytes = Array.Empty<byte>();
            string rutaLogo = Path.Combine(_env.WebRootPath, "images", "LogoPolar.jpg"); // Asegúrate de colocar tu imagen aquí

            if (File.Exists(rutaLogo))
            {
                logoBytes = await File.ReadAllBytesAsync(rutaLogo);
            }

            // 3. Instanciación del documento pasando los bytes del logo
            var document = new SociedadReportDocument(sociedad, autoridades, asambleas, reformas, certificaciones, libros, logoBytes);

            // 4. Renderizado y descarga inmediata del PDF
            var pdfBytes = document.GeneratePdf();
            using var stream = new MemoryStream(pdfBytes);
            var streamRef = new DotNetStreamReference(stream);

            string nombreArchivo = $"Reporte_Sociedad_{sociedad.Empresa?.Nombre ?? sociedadId.ToString()}_{DateTime.Now:yyyyMMdd}.pdf";
            await _js.InvokeVoidAsync("downloadFileFromStream", nombreArchivo, streamRef);
        }
    }
}
