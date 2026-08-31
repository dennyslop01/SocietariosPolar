using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SociePolar.Domain.Entities;

namespace SociePolar.WebApp.DocumentsClass
{
    public class SociedadReportDocument : IDocument
    {
        private readonly Sociedad _sociedad;
        private readonly List<Autoridad> _autoridades;
        private readonly List<Asamblea> _asambleas;
        private readonly List<Asamblea> _reformas;
        private readonly List<Certificacion> _certificaciones;
        private readonly List<LibroSocietario> _libros;
        private readonly byte[] _logoBytes;

        public SociedadReportDocument(
            Sociedad sociedad,
            List<Autoridad> autoridades,
            List<Asamblea> asambleas,
            List<Asamblea> reformas,
            List<Certificacion> certificaciones,
            List<LibroSocietario> libros,
            byte[] logoBytes)
        {
            _sociedad = sociedad;
            _autoridades = autoridades;
            _asambleas = asambleas;
            _reformas = reformas;
            _certificaciones = certificaciones;
            _libros = libros;
            _logoBytes = logoBytes;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                // 1. ENCABEZADO REESTRUCTURADO (LOGO ARRIBA, TÍTULO ABAJO CENTRADO)
                page.Header().Column(column =>
                {
                    // REGLÓN SUPERIOR: Logo a la izquierda y Fecha a la derecha
                    column.Item().Row(row =>
                    {
                        // Logotipo corporativo en el extremo izquierdo
                        if (_logoBytes != null && _logoBytes.Length > 0)
                        {
                            row.ConstantItem(75) // Un poco más grande para mejor visibilidad arriba
                               .AlignLeft()
                               .AlignMiddle()
                               .Image(_logoBytes)
                               .FitArea();
                        }
                        else
                        {
                            // Celda vacía de relleno si no hay logo para mantener la fecha a la derecha
                            row.RelativeItem().Text("");
                        }

                        // Empuja todo el espacio restante para mandar la fecha a la derecha
                        row.RelativeItem().Text("");

                        // Fecha del reporte en el extremo derecho
                        row.ConstantItem(120)
                           .AlignRight()
                           .AlignMiddle()
                           .Text($"Fecha: {DateTime.Now:dd/MM/yyyy}")
                           .FontSize(9)
                           .FontColor(Colors.Grey.Medium);
                    });

                    // Espaciador entre el renglón del logo/fecha y el título
                    column.Item().PaddingTop(15);

                    // REGLÓN INFERIOR: Títulos del Reporte Centrados
                    column.Item().AlignCenter().Column(titleColumn =>
                    {
                        titleColumn.Item().AlignCenter().Text("REPORTE DETALLADO DE SOCIEDAD")
                            .FontSize(16)
                            .Bold()
                            .FontColor(Colors.Blue.Medium);

                        titleColumn.Item().PaddingTop(2).AlignCenter().Text($"Sociedad: {_sociedad.Empresa?.Nombre}")
                            .FontSize(12)
                            .Bold()
                            .FontColor(Colors.Grey.Darken3);
                    });

                    // Línea divisoria decorativa al final del encabezado
                    column.Item().PaddingTop(10).LineHorizontal(1f).LineColor(Colors.Grey.Lighten1); // <-- Corregido a LineColor
                });

                // 2. Contenido del PDF (Conversión de las Pestañas de Blazor)
                page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                {
                    // SECCIÓN 1: DATOS GENERALES
                    column.Item().Text("1. Información General").FontSize(14).Bold().FontColor(Colors.Blue.Medium).Underline();
                    column.Item().PaddingTop(5).Table(table =>
                    {
                        // Definimos 2 columnas de igual ancho (proporcionales)
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        // Cada celda se define con table.Cell() y el contenido fluye de izquierda a derecha automáticamente
                        table.Cell().PaddingVertical(4).Text(t => { t.Span("Región: ").Bold(); t.Span(_sociedad.Region?.Nombre); });
                        table.Cell().PaddingVertical(4).Text(t => { t.Span("Unidad de Negocio: ").Bold(); t.Span(_sociedad.UnidadNegocio?.Nombre); });
                        table.Cell().PaddingVertical(4).Text(t => { t.Span("Moneda: ").Bold(); t.Span(_sociedad.Moneda?.Nombre); });
                        table.Cell().PaddingVertical(4).Text(t => { t.Span("Número SAP: ").Bold(); t.Span(_sociedad.NumeroSap); });
                        table.Cell().PaddingVertical(4).Text(t => { t.Span("Estatus: ").Bold(); t.Span(_sociedad.EstatusSociedad?.Nombre); });

                        if (_sociedad.EstatusSociedad?.Id == 1)
                        {
                            table.Cell().PaddingVertical(4).Text(t => { t.Span("Tipo Activa: ").Bold(); t.Span(_sociedad.TipoSociedadActiva?.Nombre); });
                        }
                        else
                        {
                            table.Cell().Text(""); // Celda vacía para mantener la alineación de la cuadrícula si no es estatus 1
                        }

                        table.Cell().PaddingVertical(4).Text(t => { t.Span("Fecha Constitución: ").Bold(); t.Span(_sociedad.FechaConstitucion?.ToString("yyyy-MM-dd")); });
                        table.Cell().PaddingVertical(4).Text(t => { t.Span("Fecha Vencimiento: ").Bold(); t.Span(_sociedad.FechaVencimiento?.ToString("yyyy-MM-dd")); });
                        table.Cell().PaddingVertical(4).Text(t => { t.Span("Duración: ").Bold(); t.Span(_sociedad.Duracion); });
                        table.Cell().PaddingVertical(4).Text(t => { t.Span("Acciones: ").Bold(); t.Span(FormatearMiles(_sociedad.NumeroAcciones)); });
                    });


                    // Campos de bloque (Varchar Max)
                    column.Item().PaddingTop(10).Text(t => { t.Span("Objeto de la Sociedad: ").Bold(); t.Span(_sociedad.Objeto); });
                    column.Item().PaddingTop(5).Text(t => { t.Span("Domicilio: ").Bold(); t.Span(_sociedad.Domicilio); });
                    column.Item().PaddingTop(5).Text(t => { t.Span("Dirección Fiscal: ").Bold(); t.Span(_sociedad.DireccionFiscal); });

                    // SECCIÓN 2: AUTORIDADES
                    column.Item().PaddingTop(20).Text("2. Autoridades").FontSize(14).Bold().FontColor(Colors.Blue.Medium).Underline();
                    column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Cargo").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Nombre").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Documento").Bold();
                    });

                    foreach (var aut in _autoridades)
                    {
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(aut.Cargo?.Nombre);
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(aut.Nombre);
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(aut.Documento1);
                    }
                });

                    // SECCIÓN 3: ASAMBLEAS
                    column.Item().PaddingTop(20).Text("3. Asambleas").FontSize(14).Bold().FontColor(Colors.Blue.Medium).Underline();
                    column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Tipo").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Celebración").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Registro").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Lugar de Reg.").Bold();
                    });

                    foreach (var asam in _asambleas)
                    {
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(asam.TipoAsamblea?.Nombre);
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(asam.FechaCelebracion?.ToString("dd/MM/yyyy"));
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(asam.FechaRegistro?.ToString("dd/MM/yyyy"));
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(asam.Registro?.Nombre);
                    }
                });

                    // SECCIÓN 4: REFORMAS
                    column.Item().PaddingTop(20).Text("4. Reformas").FontSize(14).Bold().FontColor(Colors.Blue.Medium).Underline();
                    column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Tipo Reforma").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Celebración").Bold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Número").Bold();
                    });

                    foreach (var refm in _reformas)
                    {
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(refm.TipoReforma?.Nombre);
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(refm.FechaCelebracion?.ToString("dd/MM/yyyy"));
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(refm.NumeroRegistro);
                    }
                });

                    // SECCIÓN 5: LIBROS SOCIETARIOS
                    column.Item().PaddingTop(20).Text("5. Libros Societarios").FontSize(14).Bold().FontColor(Colors.Blue.Medium).Underline();
                    column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                });

                    table.Header(header =>
                {
                    header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Clase").Bold();
                    header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Tipo").Bold();
                    header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Tomo").Bold();
                    header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Folios").Bold();
                });


                    foreach (var lib in _libros)
                    {
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(lib.ClaseLibro?.Nombre);
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(lib.TipoLibro?.Nombre);
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(lib.TomoUso);
                        table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(lib.Folios);
                    }
                });
                });

                // 3. Pie de página numérico automático
                page.Footer().AlignCenter().Text(x =>
                {
                    // Aplicamos el tamaño de fuente directamente a los elementos de texto
                    x.CurrentPageNumber().FontSize(9);
                    x.Span(" / ").FontSize(9);
                    x.TotalPages().FontSize(9);
                });
            });
        }

        private string FormatearMiles(string? valor)
        {
            if (valor == null || valor == "0")
                return string.Empty;

            return long.TryParse(valor, out long resultado) ? resultado.ToString("N0", System.Globalization.CultureInfo.CurrentCulture) : valor;
        }
    }
}