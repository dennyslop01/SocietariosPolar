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
                                r.RelativeItem().Text($"Ingresaron: {_encabezado.TotalIngresaron:N0} ({_encabezado.AccionesIngresadas:N0} acc.) | Compraron: {_encabezado.TotalCompraron:N0} (+{_encabezado.AccionesCompradas:N0} acc.) | Vendieron: {_encabezado.TotalVendieron:N0} (-{_encabezado.AccionesVendidas:N0} acc.) | Egresaron: {_encabezado.TotalEgresaron:N0} (-{_encabezado.AccionesEgresadas:N0} acc.)")
                                    .FontSize(7.5f);
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

                // Contenido de la Tabla clasificado por categorías de movimiento
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

                    // Clasificación de movimientos según lo requerido
                    var grupos = new List<(string Titulo, string ColorFondo, string ColorBorde, List<ConciliacionListadoReporteDto> Items)>
                    {
                        ("1. ACCIONISTAS QUE INGRESARON (NUEVOS)", "#E8F5E9", "#81C784", _detalles.Where(x => x.CategoriaMovimiento == "Ingreso").ToList()),
                        ("2. ACCIONISTAS QUE COMPRARON (INCREMENTO DE ACCIONES)", "#E3F2FD", "#64B5F6", _detalles.Where(x => x.CategoriaMovimiento == "Compra").ToList()),
                        ("3. ACCIONISTAS QUE VENDIERON (DISMINUCIÓN DE ACCIONES)", "#FFF3E0", "#FFB74D", _detalles.Where(x => x.CategoriaMovimiento == "Venta").ToList()),
                        ("4. ACCIONISTAS QUE EGRESARON (INHABILITADOS)", "#FFEBEE", "#E57373", _detalles.Where(x => x.CategoriaMovimiento == "Egreso").ToList()),
                        ("5. ACCIONISTAS SIN VARIACIÓN", "#F5F5F5", "#BDBDBD", _detalles.Where(x => x.CategoriaMovimiento == "Sin Variación").ToList())
                    };

                    int correlativo = 1;

                    foreach (var grupo in grupos)
                    {
                        if (!grupo.Items.Any()) continue;

                        // Encabezado de la Categoría
                        table.Cell().ColumnSpan(9).Border(0.75f).BorderColor(grupo.ColorBorde).Background(grupo.ColorFondo).Padding(3)
                            .Text($"{grupo.Titulo} — {grupo.Items.Count} Registro(s)")
                            .Bold()
                            .FontSize(8f)
                            .FontColor(Colors.Black);

                        foreach (var item in grupo.Items)
                        {
                            table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignCenter()
                                .Text(correlativo++.ToString()).FontSize(7.5f);

                            table.Cell().Border(0.5f).BorderColor(Colors.Grey.Lighten1).PaddingVertical(2).PaddingHorizontal(2).AlignCenter()
                                .Text(item.FechaConciliacion?.ToString("dd/MM/yyyy") ?? string.Empty).FontSize(7.5f);

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

                        // Fila Subtotal de la Categoría
                        long subSaldoAnterior = grupo.Items.Sum(x => x.SaldoAnterior);
                        long subSaldoNuevo = grupo.Items.Sum(x => x.SaldoNuevo);
                        long subVariacion = grupo.Items.Sum(x => x.Variacion);

                        table.Cell().ColumnSpan(6).Border(0.5f).BorderColor(Colors.Grey.Darken1).Background(Colors.Grey.Lighten4).Padding(2).AlignRight()
                            .Text($"SUBTOTAL {grupo.Titulo.Split('(')[0].Trim()}:").Bold().FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Darken1).Background(Colors.Grey.Lighten4).Padding(2).AlignRight()
                            .Text(subSaldoAnterior.ToString("N0", _cultura)).Bold().FontSize(7.5f);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Darken1).Background(Colors.Grey.Lighten4).Padding(2).AlignRight()
                            .Text(subSaldoNuevo.ToString("N0", _cultura)).Bold().FontSize(7.5f);

                        string subVarStr = subVariacion > 0
                            ? $"+{subVariacion.ToString("N0", _cultura)}"
                            : subVariacion.ToString("N0", _cultura);

                        table.Cell().Border(0.5f).BorderColor(Colors.Grey.Darken1).Background(Colors.Grey.Lighten4).Padding(2).AlignRight()
                            .Text(subVarStr).Bold().FontSize(7.5f);
                    }

                    // Fila Totalizadora General
                    long totalSaldoAnterior = _detalles.Sum(x => x.SaldoAnterior);
                    long totalSaldoNuevo = _detalles.Sum(x => x.SaldoNuevo);
                    long totalVariacion = _detalles.Sum(x => x.Variacion);

                    table.Cell().ColumnSpan(6).Border(1f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignCenter()
                        .Text("TOTAL GENERAL").Bold().FontSize(8);

                    table.Cell().Border(1f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignRight()
                        .Text(totalSaldoAnterior.ToString("N0", _cultura)).Bold().FontSize(8);

                    table.Cell().Border(1f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignRight()
                        .Text(totalSaldoNuevo.ToString("N0", _cultura)).Bold().FontSize(8);

                    string totalVarStr = totalVariacion > 0
                        ? $"+{totalVariacion.ToString("N0", _cultura)}"
                        : totalVariacion.ToString("N0", _cultura);

                    table.Cell().Border(1f).BorderColor(Colors.Black).Background(Colors.White).Padding(3).AlignRight()
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
