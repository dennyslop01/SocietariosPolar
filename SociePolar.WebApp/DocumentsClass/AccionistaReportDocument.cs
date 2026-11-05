using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.WebApp.Utilities;

namespace SociePolar.WebApp.DocumentsClass
{
    public class AccionistaReportDocument : IDocument
    {
        private readonly Accionista _accionista;
        private readonly List<AccionistaSociedadVerDto> _participaciones;
        private readonly Dictionary<int, string> _tiposDocumento;
        private readonly byte[] _logoBytes;

        public AccionistaReportDocument(
            Accionista accionista,
            List<AccionistaSociedadVerDto> participaciones,
            Dictionary<int, string> tiposDocumento,
            byte[] logoBytes)
        {
            _accionista = accionista;
            _participaciones = participaciones;
            _tiposDocumento = tiposDocumento;
            _logoBytes = logoBytes;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        private string ObtenerNombreTipoDoc(int? id)
        {
            if (id.HasValue && id.Value > 0 && _tiposDocumento.TryGetValue(id.Value, out var nombre))
            {
                return nombre;
            }
            return "-";
        }

        private static string ValorOGuion(string? valor) =>
            string.IsNullOrWhiteSpace(valor) ? "-" : valor;

        private static string SiNoGuion(int? valor) =>
            valor == 1 ? "Sí" : (valor == 0 ? "No" : "-");

        public void Compose(IDocumentContainer container)
        {
            string nombreMostrar = _accionista.TipoAccionista?.Id == 3
                ? (!string.IsNullOrWhiteSpace(_accionista.NombreSusesion) ? _accionista.NombreSusesion : (_accionista.Nombre ?? "-"))
                : (_accionista.Nombre ?? "-");

            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(9.5f).FontFamily("Arial"));

                // 1. ENCABEZADO
                page.Header().Column(column =>
                {
                    column.Item().Row(row =>
                    {
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
                            row.RelativeItem().Text("");
                        }

                        row.RelativeItem().Text("");

                        row.ConstantItem(120)
                           .AlignRight()
                           .AlignMiddle()
                           .Text($"Fecha: {DateTime.Now:dd/MM/yyyy}")
                           .FontSize(9)
                           .FontColor(Colors.Grey.Medium);
                    });

                    column.Item().PaddingTop(12);

                    column.Item().AlignCenter().Column(titleColumn =>
                    {
                        titleColumn.Item().PaddingTop(2).AlignCenter().Text($"Accionista: {nombreMostrar}")
                            .FontSize(13)
                            .Bold()
                            .FontColor(Colors.Grey.Darken3);

                        titleColumn.Item().PaddingTop(2).AlignCenter().Text($"Tipo de Accionista: {ValorOGuion(_accionista.TipoAccionista?.Nombre)}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);
                    });

                    column.Item().PaddingTop(8).LineHorizontal(1f).LineColor(Colors.Grey.Lighten1);
                });

                // 2. CONTENIDO DEL REPORTE
                page.Content().PaddingVertical(0.6f, Unit.Centimetre).Column(column =>
                {
                    // SECCIÓN 1: DATOS BÁSICOS
                    column.Item().Text("1. Datos Básicos").FontSize(11).Bold().FontColor(Colors.Blue.Darken2).Underline();
                    column.Item().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Accionista: ").Bold(); t.Span(ValorOGuion(_accionista.TipoAccionista?.Nombre)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Nombre / Razón Social: ").Bold(); t.Span(nombreMostrar); });

                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Documento 1: ").Bold(); t.Span(ValorOGuion(_accionista.TipoDocumento1?.Nombre)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Documento Fiscal 1: ").Bold(); t.Span(ValorOGuion(UtilClass.ObtenerNumeroDocumentoLimpio(_accionista.TipoDocumento1?.Nombre, _accionista.Documento1))); });

                        if (_accionista.TipoAccionista?.Id != 3)
                        {
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Fecha Emisión 1: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmision1?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Fecha Vencimiento 1: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimiento1?.ToString("yyyy-MM-dd"))); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Documento 2: ").Bold(); t.Span(ValorOGuion(_accionista.TipoDocumento2?.Nombre)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Documento Fiscal 2: ").Bold(); t.Span(ValorOGuion(UtilClass.ObtenerNumeroDocumentoLimpio(_accionista.TipoDocumento2?.Nombre, _accionista.Documento2))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Fecha Emisión 2: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmision2?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Fecha Vencimiento 2: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimiento2?.ToString("yyyy-MM-dd"))); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Documento 3: ").Bold(); t.Span(ValorOGuion(_accionista.TipoDocumento3?.Nombre)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Documento Fiscal 3: ").Bold(); t.Span(ValorOGuion(UtilClass.ObtenerNumeroDocumentoLimpio(_accionista.TipoDocumento3?.Nombre, _accionista.Documento3))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Fecha Emisión 3: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmision3?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Fecha Vencimiento 3: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimiento3?.ToString("yyyy-MM-dd"))); });
                        }

                        if (_accionista.TipoAccionista?.Id == 1)
                        {
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Documento 4: ").Bold(); t.Span(ValorOGuion(_accionista.TipoDocumento4?.Nombre)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Documento Fiscal 4: ").Bold(); t.Span(ValorOGuion(UtilClass.ObtenerNumeroDocumentoLimpio(_accionista.TipoDocumento4?.Nombre, _accionista.Documento4))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Fecha Emisión 4: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmision4?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Fecha Vencimiento 4: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimiento4?.ToString("yyyy-MM-dd"))); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Estado Civil: ").Bold(); t.Span(ValorOGuion(_accionista.EstadoCivil?.Nombre)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Nacionalidad: ").Bold(); t.Span(ValorOGuion(_accionista.Nacionalidad)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Fecha de Nacimiento: ").Bold(); t.Span(ValorOGuion(_accionista.FechaNacimiento?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Dirigido A: ").Bold(); t.Span(ValorOGuion(_accionista.DirigidoA?.Nombre)); });
                        }
                        else
                        {
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Dirigido A: ").Bold(); t.Span(ValorOGuion(_accionista.DirigidoA?.Nombre)); });
                            table.Cell().PaddingVertical(3).Text("");
                        }

                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Última Actualización: ").Bold(); t.Span(ValorOGuion(_accionista.UltimaActualizacion?.ToString("yyyy-MM-dd"))); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Año Actualización: ").Bold(); t.Span(ValorOGuion(_accionista.AnoActualizacion?.ToString())); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("¿Hace falta actualizar?: ").Bold(); t.Span(SiNoGuion(_accionista.FaltaActualizar)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Fecha de Ingreso: ").Bold(); t.Span(ValorOGuion(_accionista.FechaIngreso?.ToString("yyyy-MM-dd"))); });
                    });

                    // Sub-sección Persona Jurídica
                    if (_accionista.TipoAccionista?.Id == 2)
                    {
                        column.Item().PaddingTop(10).Text("Datos de Persona Jurídica y Representante Legal").FontSize(10.5f).Bold().FontColor(Colors.Grey.Darken2);
                        column.Item().PaddingTop(4).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Representante Legal: ").Bold(); t.Span(ValorOGuion(_accionista.RepresentanteLegal)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Años Duración Compañía: ").Bold(); t.Span(ValorOGuion(_accionista.AnoDuracion)); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Doc. 1 Rep. Legal: ").Bold(); t.Span(ObtenerNombreTipoDoc(_accionista.TipoDocumento1IdLegal)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Doc. 1 Rep. Legal: ").Bold(); t.Span(ValorOGuion(_accionista.Documento1Legal)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Emisión 1 Rep. Legal: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmision1Legal?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Vencimiento 1 Rep. Legal: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimiento1Legal?.ToString("yyyy-MM-dd"))); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Doc. 2 Rep. Legal: ").Bold(); t.Span(ObtenerNombreTipoDoc(_accionista.TipoDocumento2IdLegal)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Doc. 2 Rep. Legal: ").Bold(); t.Span(ValorOGuion(_accionista.Documento2Legal)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Emisión 2 Rep. Legal: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmision2Legal?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Vencimiento 2 Rep. Legal: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimiento2Legal?.ToString("yyyy-MM-dd"))); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Doc. 3 Rep. Legal: ").Bold(); t.Span(ObtenerNombreTipoDoc(_accionista.TipoDocumento3IdLegal)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Doc. 3 Rep. Legal: ").Bold(); t.Span(ValorOGuion(_accionista.Documento3Legal)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Emisión 3 Rep. Legal: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmision3Legal?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Vencimiento 3 Rep. Legal: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimiento3Legal?.ToString("yyyy-MM-dd"))); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Años Vigencia Junta: ").Bold(); t.Span(ValorOGuion(_accionista.VigenciaJunta?.ToString())); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Vencimiento Junta: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimientoJunta?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("¿Doc. Constitutivo?: ").Bold(); t.Span(SiNoGuion(_accionista.DocumentoConstitutivo)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Datos Doc. Constitutivo: ").Bold(); t.Span(ValorOGuion(_accionista.DatosDocumentoConstitutivo)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Inscrito En: ").Bold(); t.Span(ValorOGuion(_accionista.InscritoEn)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Publicación: ").Bold(); t.Span(ValorOGuion(_accionista.Publicacion)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Datos Publicación: ").Bold(); t.Span(ValorOGuion(_accionista.DatosPublicacion)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Año Emisión JD: ").Bold(); t.Span(ValorOGuion(_accionista.AnoEmisionJD)); });
                        });

                        column.Item().PaddingTop(4).Text(t => { t.Span("Junta Directiva: ").Bold(); t.Span(ValorOGuion(_accionista.JuntaDirectiva)); });
                    }

                    // Sub-sección Sucesión
                    if (_accionista.TipoAccionista?.Id == 3)
                    {
                        column.Item().PaddingTop(10).Text("Datos de la Sucesión").FontSize(10.5f).Bold().FontColor(Colors.Grey.Darken2);
                        column.Item().PaddingTop(4).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Documento Sucesión: ").Bold(); t.Span(ValorOGuion(_accionista.CedulaSusesion)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("¿Difunto?: ").Bold(); t.Span(SiNoGuion(_accionista.Difunto)); });

                            if (_accionista.Difunto == 1)
                            {
                                table.Cell().PaddingVertical(3).Text(t => { t.Span("Fecha Defunción: ").Bold(); t.Span(ValorOGuion(_accionista.FechaDefuncion?.ToString("yyyy-MM-dd"))); });
                                table.Cell().PaddingVertical(3).Text(t => { t.Span("Datos Acta Defunción: ").Bold(); t.Span(ValorOGuion(_accionista.DatosActaDefuncion)); });
                                table.Cell().PaddingVertical(3).Text(t => { t.Span("Declaración: ").Bold(); t.Span(ValorOGuion(_accionista.Declaracion)); });
                                table.Cell().PaddingVertical(3).Text(t => { t.Span("Datos Declaración: ").Bold(); t.Span(ValorOGuion(_accionista.DatosDeclaracion)); });
                            }
                        });

                        column.Item().PaddingTop(4).Text(t => { t.Span("Miembros Sucesión: ").Bold(); t.Span(ValorOGuion(_accionista.MiembrosSucesion)); });
                    }

                    // SECCIÓN 2: CONTACTO Y UBICACIÓN
                    column.Item().PaddingTop(14).Text("2. Contacto y Ubicación").FontSize(11).Bold().FontColor(Colors.Blue.Darken2).Underline();
                    column.Item().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Email 1: ").Bold(); t.Span(ValorOGuion(_accionista.Email1)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Email 2: ").Bold(); t.Span(ValorOGuion(_accionista.Email2)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Email 3: ").Bold(); t.Span(ValorOGuion(_accionista.Email3)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Teléfono Móvil: ").Bold(); t.Span(ValorOGuion(_accionista.TelefonoMovil)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Teléfono 1: ").Bold(); t.Span(ValorOGuion(_accionista.Telefono1)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Teléfono 2: ").Bold(); t.Span(ValorOGuion(_accionista.Telefono2)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Teléfono 3: ").Bold(); t.Span(ValorOGuion(_accionista.Telefono3)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Teléfono 4: ").Bold(); t.Span(ValorOGuion(_accionista.Telefono4)); });
                    });

                    column.Item().PaddingTop(4).Text(t => { t.Span("Grupo Familiar: ").Bold(); t.Span(ValorOGuion(_accionista.GrupoFamiliar)); });
                    column.Item().PaddingTop(3).Text(t => { t.Span("Dirección 1: ").Bold(); t.Span(ValorOGuion(_accionista.Direccion1)); });
                    column.Item().PaddingTop(3).Text(t => { t.Span("Dirección 2: ").Bold(); t.Span(ValorOGuion(_accionista.Direccion2)); });
                    column.Item().PaddingTop(3).Text(t => { t.Span("Domiciliado En: ").Bold(); t.Span(ValorOGuion(_accionista.DomiciliadoEn)); });

                    // SECCIÓN 3: CÓNYUGE (Persona Natural)
                    if (_accionista.TipoAccionista?.Id == 1)
                    {
                        column.Item().PaddingTop(14).Text("3. Datos del Cónyuge").FontSize(11).Bold().FontColor(Colors.Blue.Darken2).Underline();
                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Nombre Cónyuge: ").Bold(); t.Span(ValorOGuion(_accionista.NombreConyuge)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("¿Separación de Bienes?: ").Bold(); t.Span(SiNoGuion(_accionista.SeparacionBienes)); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Doc. 1 Cónyuge: ").Bold(); t.Span(ObtenerNombreTipoDoc(_accionista.TipoDocumentoConyugeId1)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Documento 1 Cónyuge: ").Bold(); t.Span(ValorOGuion(_accionista.DocumentoConyuge1)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Emisión 1 Cónyuge: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmisionConyuge1?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Vencimiento 1 Cónyuge: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimientoConyuge1?.ToString("yyyy-MM-dd"))); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Doc. 2 Cónyuge: ").Bold(); t.Span(ObtenerNombreTipoDoc(_accionista.TipoDocumentoConyugeId2)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Documento 2 Cónyuge: ").Bold(); t.Span(ValorOGuion(_accionista.DocumentoConyuge2)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Emisión 2 Cónyuge: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmisionConyuge2?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Vencimiento 2 Cónyuge: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimientoConyuge2?.ToString("yyyy-MM-dd"))); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Doc. 3 Cónyuge: ").Bold(); t.Span(ObtenerNombreTipoDoc(_accionista.TipoDocumentoConyugeId3)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Documento 3 Cónyuge: ").Bold(); t.Span(ValorOGuion(_accionista.DocumentoConyuge3)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Emisión 3 Cónyuge: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmisionConyuge3?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Vencimiento 3 Cónyuge: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimientoConyuge3?.ToString("yyyy-MM-dd"))); });
                        });
                    }

                    // SECCIÓN 4: FINANZAS Y APODERADO
                    column.Item().PaddingTop(14).Text("4. Finanzas y Apoderado").FontSize(11).Bold().FontColor(Colors.Blue.Darken2).Underline();
                    column.Item().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Banco: ").Bold(); t.Span(ValorOGuion(_accionista.Banco?.Nombre)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Cuenta: ").Bold(); t.Span(ValorOGuion(_accionista.TipoCuenta?.Nombre)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Número de Cuenta: ").Bold(); t.Span(ValorOGuion(_accionista.NumeroCuenta)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Nombre Titular: ").Bold(); t.Span(ValorOGuion(_accionista.NombreTitularCuenta)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Modalidad de Pago: ").Bold(); t.Span(ValorOGuion(_accionista.ModalidadPago?.Nombre)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("¿Tiene Apoderado?: ").Bold(); t.Span(SiNoGuion(_accionista.TieneApoderado)); });

                        if (_accionista.TieneApoderado == 1)
                        {
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Nombre Apoderado: ").Bold(); t.Span(ValorOGuion(_accionista.NombreApoderado)); });
                            table.Cell().PaddingVertical(3).Text("");

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Doc. 1 Apoderado: ").Bold(); t.Span(ObtenerNombreTipoDoc(_accionista.TipoDocumentoApoderadoId1)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Documento 1 Apoderado: ").Bold(); t.Span(ValorOGuion(_accionista.DocumentoApoderado1)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Emisión 1 Apoderado: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmisionApoderado1?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Vencimiento 1 Apoderado: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimientoApoderado1?.ToString("yyyy-MM-dd"))); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Doc. 2 Apoderado: ").Bold(); t.Span(ObtenerNombreTipoDoc(_accionista.TipoDocumentoApoderadoId2)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Documento 2 Apoderado: ").Bold(); t.Span(ValorOGuion(_accionista.DocumentoApoderado2)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Emisión 2 Apoderado: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmisionApoderado2?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Vencimiento 2 Apoderado: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimientoApoderado2?.ToString("yyyy-MM-dd"))); });

                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Tipo Doc. 3 Apoderado: ").Bold(); t.Span(ObtenerNombreTipoDoc(_accionista.TipoDocumentoApoderadoId3)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Documento 3 Apoderado: ").Bold(); t.Span(ValorOGuion(_accionista.DocumentoApoderado3)); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Emisión 3 Apoderado: ").Bold(); t.Span(ValorOGuion(_accionista.FechaEmisionApoderado3?.ToString("yyyy-MM-dd"))); });
                            table.Cell().PaddingVertical(3).Text(t => { t.Span("Vencimiento 3 Apoderado: ").Bold(); t.Span(ValorOGuion(_accionista.FechaVencimientoApoderado3?.ToString("yyyy-MM-dd"))); });
                        }
                    });

                    if (_accionista.TieneApoderado == 1)
                    {
                        column.Item().PaddingTop(3).Text(t => { t.Span("Datos Poder: ").Bold(); t.Span(ValorOGuion(_accionista.DatosPoder)); });
                    }

                    // SECCIÓN 5: JUNTA Y OTROS
                    column.Item().PaddingTop(14).Text("5. Contacto Alterno y Otros").FontSize(11).Bold().FontColor(Colors.Blue.Darken2).Underline();
                    column.Item().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Nombre Contacto: ").Bold(); t.Span(ValorOGuion(_accionista.NombreContacto)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Email Contacto: ").Bold(); t.Span(ValorOGuion(_accionista.EmailContacto)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Teléfono Contacto: ").Bold(); t.Span(ValorOGuion(_accionista.TelefonoContacto)); });
                        table.Cell().PaddingVertical(3).Text(t => { t.Span("Condición Especial: ").Bold(); t.Span(ValorOGuion(_accionista.CondicionEspecial?.Nombre)); });
                    });

                    column.Item().PaddingTop(4).Text(t => { t.Span("Observaciones: ").Bold(); t.Span(ValorOGuion(_accionista.Observaciones)); });
                    column.Item().PaddingTop(3).Text(t => { t.Span("Documentos Relacionados: ").Bold(); t.Span(ValorOGuion(_accionista.DocumentosRelacionados)); });

                    // SECCIÓN 6: PARTICIPACIÓN EN SOCIEDADES
                    column.Item().PaddingTop(14).Text("6. Participación en Sociedades").FontSize(11).Bold().FontColor(Colors.Blue.Darken2).Underline();
                    if (_participaciones.Any())
                    {
                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2.5f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.4f);
                                columns.RelativeColumn(1.1f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("Sociedad").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("RIF").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("Estatus Soc.").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("Estatus Acc.").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(4).AlignRight().Text("Nro. Acciones").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(4).AlignRight().Text("% Part.").Bold();
                            });

                            foreach (var part in _participaciones)
                            {
                                table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(ValorOGuion(part.SociedadNombre));
                                table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(ValorOGuion(part.DocumentoIdentidad));
                                table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(ValorOGuion(part.EstatusSociedadNombre));
                                table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(ValorOGuion(part.TipoAccionistaNombre));
                                table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(4).AlignRight().Text(UtilClass.FormatearMiles(part.NroAcciones ?? 0));
                                table.Cell().BorderBottom(0.5f, Unit.Point).BorderColor(Colors.Grey.Lighten2).Padding(4).AlignRight().Text($"{(part.Participacion ?? 0):N6}%");
                            }
                        });
                    }
                    else
                    {
                        column.Item().PaddingTop(4).Text("El accionista no registra participaciones en sociedades.").FontColor(Colors.Grey.Medium).Italic();
                    }
                });

                // 3. PIE DE PÁGINA
                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber().FontSize(9);
                    x.Span(" / ").FontSize(9);
                    x.TotalPages().FontSize(9);
                });
            });
        }
    }
}

