using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SociePolar.Domain.Dtos;
using System.Globalization;

namespace SociePolar.WebApp.DocumentsClass
{
    public class DividendoReportDocument : IDocument
    {
        private readonly DividendoEncabezadoReporteDto _encabezado;
        private readonly List<DividendoListadoReporteDto> _detalles;
        private readonly byte[]? _logoBytes;
        private static readonly CultureInfo _cultura = new CultureInfo("es-VE");

        public DividendoReportDocument(
            DividendoEncabezadoReporteDto encabezado,
            List<DividendoListadoReporteDto> detalles,
            byte[]? logoBytes)
        {
            _encabezado = encabezado;
            _detalles = detalles ?? new List<DividendoListadoReporteDto>();
            _logoBytes = logoBytes;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(1.0f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(8f).FontFamily("Arial"));

                // Encabezado
                page.Header().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        // Bloque superior izquierdo con datos del decreto y sociedad
                        row.RelativeItem().Column(infoCol =>
                        {
                            infoCol.Item().Text(_encabezado.NombreEmpresa ?? "EMPRESA")
                                .FontSize(11)
                                .Bold()
                                .FontColor("#104E8B");

                            infoCol.Item().PaddingTop(2).Row(r =>
                            {
                                r.ConstantItem(120).Text("PERÍODO:").Bold().FontSize(8).FontColor(Colors.Grey.Darken3);
                                r.RelativeItem().Text($"{_encabezado.FechaDesde:dd/MM/yyyy} al {_encabezado.FechaHasta:dd/MM/yyyy}")
                                    .FontSize(8).Bold();
                            });

                            infoCol.Item().Row(r =>
                            {
                                r.ConstantItem(120).Text("MONTO DECRETADO:").Bold().FontSize(8).FontColor(Colors.Grey.Darken3);
                                r.ConstantItem(25).Text(_encabezado.MonedaSimbolo ?? "Bs.").FontSize(8);
                                r.RelativeItem().Text(_encabezado.MontoTotalDecretado.ToString("N2", _cultura))
                                    .FontSize(8).Bold();
                            });

                            infoCol.Item().Row(r =>
                            {
                                r.ConstantItem(120).Text("MONTO RETENIDO:").Bold().FontSize(8).FontColor(Colors.Grey.Darken3);
                                r.ConstantItem(25).Text(_encabezado.MonedaSimbolo ?? "Bs.").FontSize(8);
                                r.RelativeItem().Text(_encabezado.MontoTotalRetenido.ToString("N2", _cultura))
                                    .FontSize(8).Bold();
                            });

                            infoCol.Item().Row(r =>
                            {
                                r.ConstantItem(120).Text("MONTO NETO PAGADO:").Bold().FontSize(8).FontColor(Colors.Grey.Darken3);
                                r.ConstantItem(25).Text(_encabezado.MonedaSimbolo ?? "Bs.").FontSize(8);
                                r.RelativeItem().Text(_encabezado.MontoTotalNetoPagado.ToString("N2", _cultura))
                                    .FontSize(8).Bold();
                            });
                        });

                        // Logo corporativo en esquina superior derecha
                        if (_logoBytes != null && _logoBytes.Length > 0)
                        {
                            row.ConstantItem(75).AlignRight().AlignTop().Image(_logoBytes).FitArea();
                        }
                    });

                    col.Item().PaddingTop(6);

                    // Barra de título con borde que abarca el ancho de la tabla
                    col.Item().Border(1).BorderColor(Colors.Black).Background(Colors.White).PaddingVertical(3).AlignCenter()
                        .Text($"REPORTE DE DIVIDENDOS DEFINITIVOS ({_encabezado.FechaDesde:dd.MM.yyyy} AL {_encabezado.FechaHasta:dd.MM.yyyy})")
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
                        cols.ConstantColumn(24);       // #
                        cols.ConstantColumn(110);      // Cédulas o R.I.F.
                        cols.RelativeColumn();         // ACCIONISTAS
                        cols.ConstantColumn(85);       // MONTO DECRETADO
                        cols.ConstantColumn(80);       // MONTO RETENIDO
                        cols.ConstantColumn(85);       // MONTO NETO
                        cols.ConstantColumn(70);       // TIPO PAGO
                        cols.ConstantColumn(65);       // EJERCICIO
                        cols.ConstantColumn(70);       // FECHA DECRETO
                    });

                    // Encabezados de Columna
                    table.Header(h =>
                    {
                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignCenter()
                            .Text("#").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignLeft()
                            .Text("Cédulas o R.I.F.").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignLeft()
                            .Text("ACCIONISTAS").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignRight()
                            .Text("MTO. DECRETADO").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignRight()
                            .Text("MTO. RETENIDO").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignRight()
                            .Text("MTO. NETO").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignCenter()
                            .Text("TIPO PAGO").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignCenter()
                            .Text("EJERCICIO").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignCenter()
                            .Text("FECHA DECRETO").Bold().FontSize(7.5f);
                    });

                    // Filas de Datos
                    foreach (var item in _detalles)
                    {
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignCenter()
                            .Text(item.Numero.ToString()).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignLeft()
                            .Text(item.CedulaRif ?? string.Empty).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignLeft()
                            .Text(item.NombreAccionista ?? string.Empty).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignRight()
                            .Text(item.MontoDecretado.ToString("N2", _cultura)).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignRight()
                            .Text(item.MontoRetenido.ToString("N2", _cultura)).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignRight()
                            .Text(item.MontoNetoPagado.ToString("N2", _cultura)).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignCenter()
                            .Text(item.TipoPago ?? "-").FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignCenter()
                            .Text(item.EjercicioFiscal ?? "-").FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignCenter()
                            .Text(item.FechaDecreto?.ToString("dd/MM/yyyy") ?? "-").FontSize(7.5f);
                    }

                    // Fila Totalizadora
                    decimal totalDecretado = _detalles.Sum(x => x.MontoDecretado);
                    decimal totalRetenido = _detalles.Sum(x => x.MontoRetenido);
                    decimal totalNeto = _detalles.Sum(x => x.MontoNetoPagado);

                    table.Cell().ColumnSpan(3).Border(1f).BorderColor(Colors.Black).Padding(3).AlignCenter()
                        .Text("TOTALES").Bold().FontSize(8);

                    table.Cell().Border(1f).BorderColor(Colors.Black).Padding(3).AlignRight()
                        .Text(totalDecretado.ToString("N2", _cultura)).Bold().FontSize(8);

                    table.Cell().Border(1f).BorderColor(Colors.Black).Padding(3).AlignRight()
                        .Text(totalRetenido.ToString("N2", _cultura)).Bold().FontSize(8);

                    table.Cell().Border(1f).BorderColor(Colors.Black).Padding(3).AlignRight()
                        .Text(totalNeto.ToString("N2", _cultura)).Bold().FontSize(8);

                    table.Cell().ColumnSpan(3).Border(1f).BorderColor(Colors.Black).Padding(3).AlignCenter()
                        .Text("");
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
