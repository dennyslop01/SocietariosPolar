using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace SociePolar.WebApp.DocumentsClass
{
    public class ListaReportDocument : IDocument
    {
        private readonly string _titulo;
        private readonly string? _subtitulo;
        private readonly string[] _columnas;
        private readonly List<string[]> _filas;
        private readonly float[]? _anchosRelativos;
        private readonly byte[]? _logoBytes;
        private readonly bool _landscape;

        public ListaReportDocument(
            string titulo,
            string? subtitulo,
            string[] columnas,
            List<string[]> filas,
            float[]? anchosRelativos,
            byte[]? logoBytes,
            bool landscape = true)
        {
            _titulo = titulo;
            _subtitulo = subtitulo;
            _columnas = columnas ?? Array.Empty<string>();
            _filas = filas ?? new List<string[]>();
            _anchosRelativos = anchosRelativos;
            _logoBytes = logoBytes;
            _landscape = landscape;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(_landscape ? PageSizes.A4.Landscape() : PageSizes.A4);
                page.Margin(1.0f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(8.5f).FontFamily("Arial"));

                // Encabezado
                page.Header().Column(column =>
                {
                    column.Item().Row(row =>
                    {
                        // Logo corporativo
                        if (_logoBytes != null && _logoBytes.Length > 0)
                        {
                            row.ConstantItem(75)
                               .AlignLeft()
                               .AlignMiddle()
                               .Image(_logoBytes)
                               .FitArea();
                        }
                        else
                        {
                            row.ConstantItem(75).Text("");
                        }

                        // Título y Subtítulo centrados
                        row.RelativeItem().AlignCenter().Column(titleCol =>
                        {
                            titleCol.Item().AlignCenter().Text(_titulo)
                                .FontSize(13)
                                .Bold()
                                .FontColor(Colors.Grey.Darken3);

                            if (!string.IsNullOrWhiteSpace(_subtitulo))
                            {
                                titleCol.Item().AlignCenter().Text(_subtitulo)
                                    .FontSize(9)
                                    .FontColor(Colors.Grey.Darken1);
                            }
                        });

                        // Fecha y Total registros
                        row.ConstantItem(140)
                           .AlignRight()
                           .AlignMiddle()
                           .Column(infoCol =>
                           {
                               infoCol.Item().AlignRight().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                   .FontSize(8)
                                   .FontColor(Colors.Grey.Medium);
                               infoCol.Item().AlignRight().Text($"Total: {_filas.Count:N0} registros")
                                   .FontSize(8)
                                   .Bold()
                                   .FontColor(Colors.Grey.Darken2);
                           });
                    });

                    column.Item().PaddingTop(8).LineHorizontal(1f).LineColor(Colors.Grey.Lighten1);
                    column.Item().PaddingBottom(6);
                });

                // Contenido de la Tabla
                page.Content().Table(table =>
                {
                    // Definición de Columnas
                    table.ColumnsDefinition(columns =>
                    {
                        for (int i = 0; i < _columnas.Length; i++)
                        {
                            if (_anchosRelativos != null && i < _anchosRelativos.Length && _anchosRelativos[i] > 0)
                            {
                                columns.RelativeColumn(_anchosRelativos[i]);
                            }
                            else
                            {
                                columns.RelativeColumn(1);
                            }
                        }
                    });

                    // Encabezados de Columna
                    table.Header(header =>
                    {
                        for (int i = 0; i < _columnas.Length; i++)
                        {
                            header.Cell()
                                .Background(Colors.Grey.Lighten3)
                                .BorderBottom(1f, Unit.Point)
                                .BorderColor(Colors.Grey.Darken1)
                                .PaddingVertical(4)
                                .PaddingHorizontal(3)
                                .Text(_columnas[i])
                                .FontSize(8)
                                .Bold()
                                .FontColor(Colors.Grey.Darken3);
                        }
                    });

                    // Filas de Datos
                    for (int r = 0; r < _filas.Count; r++)
                    {
                        var fila = _filas[r];
                        var bgColor = (r % 2 == 1) ? Colors.Grey.Lighten4 : Colors.White;

                        for (int c = 0; c < _columnas.Length; c++)
                        {
                            string valor = (c < fila.Length && fila[c] != null) ? fila[c] : "";

                            table.Cell()
                                .Background(bgColor)
                                .BorderBottom(0.5f, Unit.Point)
                                .BorderColor(Colors.Grey.Lighten2)
                                .PaddingVertical(3)
                                .PaddingHorizontal(3)
                                .Text(valor)
                                .FontSize(7.5f)
                                .FontColor(Colors.Grey.Darken3);
                        }
                    }
                });

                // Pie de página
                page.Footer().PaddingTop(5).Row(row =>
                {
                    row.RelativeItem().Text("Empresas Polar - Sistema de Control Societario")
                        .FontSize(7.5f)
                        .FontColor(Colors.Grey.Medium);

                    row.RelativeItem().AlignRight().Text(x =>
                    {
                        x.Span("Página ").FontSize(8).FontColor(Colors.Grey.Medium);
                        x.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Medium);
                        x.Span(" de ").FontSize(8).FontColor(Colors.Grey.Medium);
                        x.TotalPages().FontSize(8).FontColor(Colors.Grey.Medium);
                    });
                });
            });
        }
    }
}

