using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SociePolar.Domain.Dtos;
using System.Globalization;

namespace SociePolar.WebApp.DocumentsClass
{
    public class ListaAccionistasCorteReportDocument : IDocument
    {
        private readonly SociedadEncabezadoReporteDto _encabezado;
        private readonly DateTime _fechaCorte;
        private readonly List<AccionistaListadoCorteDto> _accionistas;
        private readonly byte[]? _logoBytes;
        private static readonly CultureInfo _cultura = new CultureInfo("es-VE");

        public ListaAccionistasCorteReportDocument(
            SociedadEncabezadoReporteDto encabezado,
            DateTime fechaCorte,
            List<AccionistaListadoCorteDto> accionistas,
            byte[]? logoBytes)
        {
            _encabezado = encabezado;
            _fechaCorte = fechaCorte;
            _accionistas = accionistas ?? new List<AccionistaListadoCorteDto>();
            _logoBytes = logoBytes;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.0f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(8f).FontFamily("Arial"));

                // Encabezado
                page.Header().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        // Bloque superior izquierdo con datos de la sociedad (según formato de la imagen)
                        row.RelativeItem().Column(infoCol =>
                        {
                            infoCol.Item().Text(_encabezado.NombreEmpresa ?? "EMPRESA")
                                .FontSize(11)
                                .Bold()
                                .FontColor("#104E8B");

                            infoCol.Item().PaddingTop(2).Row(r =>
                            {
                                r.ConstantItem(100).Text("CAPITAL SOCIAL:").Bold().FontSize(8).FontColor(Colors.Grey.Darken3);
                                r.ConstantItem(25).Text("Bs.").FontSize(8);
                                r.RelativeItem().Text(_encabezado.CapitalSocial.HasValue ? _encabezado.CapitalSocial.Value.ToString("N2", _cultura) : "0,00")
                                    .FontSize(8).Bold();
                            });

                            infoCol.Item().Row(r =>
                            {
                                r.ConstantItem(100).Text("TOTAL ACCIONES:").Bold().FontSize(8).FontColor(Colors.Grey.Darken3);
                                r.ConstantItem(25).Text("");
                                r.RelativeItem().Text(_encabezado.TotalAcciones.ToString("N0", _cultura))
                                    .FontSize(8).Bold();
                            });

                            infoCol.Item().Row(r =>
                            {
                                r.ConstantItem(100).Text("VALOR NOMINAL:").Bold().FontSize(8).FontColor(Colors.Grey.Darken3);
                                r.ConstantItem(25).Text("Bs.").FontSize(8);
                                r.RelativeItem().Text(_encabezado.ValorNominal.HasValue ? _encabezado.ValorNominal.Value.ToString("0.000", _cultura) : "0,000")
                                    .FontSize(8).Bold();
                            });
                        });

                        // Logo corporativo en esquina superior derecha
                        if (_logoBytes != null && _logoBytes.Length > 0)
                        {
                            row.ConstantItem(75).AlignRight().AlignTop().Image(_logoBytes).FitArea();
                        }
                    });

                    col.Item().PaddingTop(8);

                    // Barra de título con borde que abarca el ancho de la tabla: "LISTADO ACCIONISTAS AL dd.MM.yyyy"
                    col.Item().Border(1).BorderColor(Colors.Black).Background(Colors.White).PaddingVertical(3).AlignCenter()
                        .Text($"LISTADO ACCIONISTAS AL {_fechaCorte:dd.MM.yyyy}")
                        .FontSize(9.5f)
                        .Bold()
                        .FontColor(Colors.Black);

                    col.Item().PaddingBottom(2);
                });

                // Contenido de la Tabla
                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.ConstantColumn(26);       // #
                        cols.ConstantColumn(120);      // Cédulas o R.I.F.
                        cols.RelativeColumn();         // ACCIONISTAS
                        cols.ConstantColumn(80);       // ACCIONES
                        cols.ConstantColumn(70);       // PORCENTAJES
                    });

                    // Encabezados de Columna
                    table.Header(h =>
                    {
                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignCenter()
                            .Text("#").Bold().FontSize(8);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignLeft()
                            .Text("Cédulas o R.I.F.").Bold().FontSize(8);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignLeft()
                            .Text("ACCIONISTAS").Bold().FontSize(8);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignRight()
                            .Text("ACCIONES").Bold().FontSize(8);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignRight()
                            .Text("PORCENTAJES").Bold().FontSize(8);
                    });

                    // Filas de Datos
                    foreach (var acc in _accionistas)
                    {
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(3).AlignCenter()
                            .Text(acc.Numero.ToString()).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(3).AlignLeft()
                            .Text(acc.CedulaRif ?? string.Empty).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(3).AlignLeft()
                            .Text(acc.NombreAccionista ?? string.Empty).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(3).AlignRight()
                            .Text(acc.NroAcciones.ToString("N0", _cultura)).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(3).AlignRight()
                            .Text(acc.PorcentajeParticipacion.ToString("0.000000", _cultura)).FontSize(7.5f);
                    }

                    // Fila Totalizadora
                    long sumaAcciones = _accionistas.Sum(x => x.NroAcciones);
                    string totalPorcentajeStr = "100,000000";

                    table.Cell().ColumnSpan(3).Border(1f).BorderColor(Colors.Black).Padding(3).AlignCenter()
                        .Text("TOTAL ACCIONES").Bold().FontSize(8);

                    table.Cell().Border(1f).BorderColor(Colors.Black).Padding(3).AlignRight()
                        .Text(sumaAcciones.ToString("N0", _cultura)).Bold().FontSize(8);

                    table.Cell().Border(1f).BorderColor(Colors.Black).Padding(3).AlignRight()
                        .Text(totalPorcentajeStr).Bold().FontSize(8);
                });

                // Pie de página
                page.Footer().PaddingTop(4).Row(row =>
                {
                    row.RelativeItem().Text("Empresas Polar - Sistema de Control Societario")
                        .FontSize(7)
                        .FontColor(Colors.Grey.Medium);

                    row.RelativeItem().AlignRight().Text(x =>
                    {
                        x.Span("Página ").FontSize(7).FontColor(Colors.Grey.Medium);
                        x.CurrentPageNumber().FontSize(7).FontColor(Colors.Grey.Medium);
                        x.Span(" de ").FontSize(7).FontColor(Colors.Grey.Medium);
                        x.TotalPages().FontSize(7).FontColor(Colors.Grey.Medium);
                    });
                });
            });
        }
    }
}
