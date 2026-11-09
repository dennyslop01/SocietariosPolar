using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SociePolar.Domain.Dtos;
using System.Globalization;

namespace SociePolar.WebApp.DocumentsClass
{
    public class ConciliacionReportDocument : IDocument
    {
        private readonly ConciliacionEncabezadoReporteDto _encabezado;
        private readonly List<ConciliacionListadoReporteDto> _detalles;
        private readonly byte[]? _logoBytes;
        private static readonly CultureInfo _cultura = new CultureInfo("es-VE");

        public ConciliacionReportDocument(
            ConciliacionEncabezadoReporteDto encabezado,
            List<ConciliacionListadoReporteDto> detalles,
            byte[]? logoBytes)
        {
            _encabezado = encabezado;
            _detalles = detalles ?? new List<ConciliacionListadoReporteDto>();
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
                        // Bloque superior izquierdo con datos de la conciliación
                        row.RelativeItem().Column(infoCol =>
                        {
                            infoCol.Item().Text(_encabezado.NombreEmpresa ?? "EMPRESA")
                                .FontSize(11)
                                .Bold()
                                .FontColor("#00338D");

                            infoCol.Item().PaddingTop(2).Row(r =>
                            {
                                r.ConstantItem(150).Text("PERÍODO:").Bold().FontSize(8).FontColor(Colors.Grey.Darken3);
                                r.RelativeItem().Text($"{_encabezado.FechaDesde:dd/MM/yyyy} al {_encabezado.FechaHasta:dd/MM/yyyy}")
                                    .FontSize(8).Bold();
                            });

                            infoCol.Item().Row(r =>
                            {
                                r.ConstantItem(150).Text("CANTIDAD TOTAL DE ACCIONES:").Bold().FontSize(8).FontColor(Colors.Grey.Darken3);
                                r.RelativeItem().Text(_encabezado.CantidadTotalAcciones.ToString("N0", _cultura))
                                    .FontSize(8).Bold();
                            });

                            infoCol.Item().Row(r =>
                            {
                                r.ConstantItem(150).Text("ACCIONISTAS CONCILIADOS:").Bold().FontSize(8).FontColor(Colors.Grey.Darken3);
                                r.RelativeItem().Text(_encabezado.TotalAccionistasConciliados.ToString("N0", _cultura))
                                    .FontSize(8).Bold();
                            });

                            infoCol.Item().Row(r =>
                            {
                                r.ConstantItem(150).Text("RESUMEN DE MOVIMIENTOS:").Bold().FontSize(8).FontColor(Colors.Grey.Darken3);
                                r.RelativeItem().Text($"Nuevos: {_encabezado.TotalNuevos:N0}  |  Actualizados: {_encabezado.TotalActualizados:N0}  |  Inhabilitados: {_encabezado.TotalInactivados:N0}")
                                    .FontSize(8);
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
                        .Text($"REPORTE DE CONCILIACIÓN DE ACCIONES ({_encabezado.FechaDesde:dd.MM.yyyy} AL {_encabezado.FechaHasta:dd.MM.yyyy})")
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
                        cols.ConstantColumn(55);       // FECHA
                        cols.ConstantColumn(65);       // ACCIÓN
                        cols.ConstantColumn(45);       // TIPO DOC
                        cols.ConstantColumn(85);       // CÉDULAS O R.I.F.
                        cols.RelativeColumn();         // ACCIONISTAS
                        cols.ConstantColumn(75);       // SALDO ANTERIOR
                        cols.ConstantColumn(75);       // SALDO NUEVO
                        cols.ConstantColumn(75);       // VARIACIÓN
                    });

                    // Encabezados de Columna
                    table.Header(h =>
                    {
                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignCenter()
                            .Text("#").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignCenter()
                            .Text("FECHA").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignCenter()
                            .Text("ACCIÓN").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignCenter()
                            .Text("TIPO DOC.").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignLeft()
                            .Text("CÉDULAS O R.I.F.").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignLeft()
                            .Text("ACCIONISTAS").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignRight()
                            .Text("SALDO ANTERIOR").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignRight()
                            .Text("SALDO NUEVO").Bold().FontSize(7.5f);

                        h.Cell().Border(0.75f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignRight()
                            .Text("VARIACIÓN").Bold().FontSize(7.5f);
                    });

                    // Filas de Datos
                    foreach (var item in _detalles)
                    {
                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignCenter()
                            .Text(item.Numero.ToString()).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignCenter()
                            .Text(item.FechaConciliacion?.ToString("dd/MM/yyyy") ?? "-").FontSize(7.5f);

                        // Celda de Acción estilizada
                        var accionCell = table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignCenter();
                        if (string.Equals(item.Accion, "Creado", StringComparison.OrdinalIgnoreCase))
                        {
                            accionCell.Text(item.Accion ?? "Creado").FontSize(7.5f).Bold().FontColor(Colors.Green.Darken2);
                        }
                        else if (string.Equals(item.Accion, "Inactivado", StringComparison.OrdinalIgnoreCase))
                        {
                            accionCell.Text(item.Accion ?? "Inactivado").FontSize(7.5f).Bold().FontColor(Colors.Red.Darken2);
                        }
                        else
                        {
                            accionCell.Text(item.Accion ?? "Actualizado").FontSize(7.5f).Bold().FontColor("#00338D");
                        }

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignCenter()
                            .Text(item.TipoDocumento ?? string.Empty).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignLeft()
                            .Text(item.CedulaRif ?? string.Empty).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignLeft()
                            .Text(item.NombreAccionista ?? string.Empty).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignRight()
                            .Text(item.SaldoAnterior.ToString("N0", _cultura)).FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignRight()
                            .Text(item.SaldoNuevo.ToString("N0", _cultura)).FontSize(7.5f);

                        // Celda de Variación con signo
                        string variacionStr = item.Variacion > 0
                            ? $"+{item.Variacion.ToString("N0", _cultura)}"
                            : item.Variacion.ToString("N0", _cultura);

                        var variacionCell = table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignRight();
                        if (item.Variacion > 0)
                        {
                            variacionCell.Text(variacionStr).FontSize(7.5f).FontColor(Colors.Green.Darken2);
                        }
                        else if (item.Variacion < 0)
                        {
                            variacionCell.Text(variacionStr).FontSize(7.5f).FontColor(Colors.Red.Darken2);
                        }
                        else
                        {
                            variacionCell.Text(variacionStr).FontSize(7.5f);
                        }
                    }

                    // Fila Totalizadora
                    long totalSaldoAnterior = _detalles.Sum(x => x.SaldoAnterior);
                    long totalSaldoNuevo = _detalles.Sum(x => x.SaldoNuevo);
                    long totalVariacion = _detalles.Sum(x => x.Variacion);

                    table.Cell().ColumnSpan(6).Border(1f).BorderColor(Colors.Black).Padding(3).AlignCenter()
                        .Text("TOTALES").Bold().FontSize(8);

                    table.Cell().Border(1f).BorderColor(Colors.Black).Padding(3).AlignRight()
                        .Text(totalSaldoAnterior.ToString("N0", _cultura)).Bold().FontSize(8);

                    table.Cell().Border(1f).BorderColor(Colors.Black).Padding(3).AlignRight()
                        .Text(totalSaldoNuevo.ToString("N0", _cultura)).Bold().FontSize(8);

                    string totalVarStr = totalVariacion > 0
                        ? $"+{totalVariacion.ToString("N0", _cultura)}"
                        : totalVariacion.ToString("N0", _cultura);

                    table.Cell().Border(1f).BorderColor(Colors.Black).Padding(3).AlignRight()
                        .Text(totalVarStr).Bold().FontSize(8);
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
