using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SociePolar.Domain.Dtos;
using SociePolar.WebApp.Utilities;
using System.Globalization;

namespace SociePolar.WebApp.DocumentsClass
{
    public class CertificacionCartaReportDocument : IDocument
    {
        private readonly CertificacionReportDto _dto;
        private readonly byte[]? _logoBytes;
        private static readonly CultureInfo _cultura = new CultureInfo("es-VE");

        public CertificacionCartaReportDocument(CertificacionReportDto dto, byte[]? logoBytes)
        {
            _dto = dto;
            _logoBytes = logoBytes;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.MarginTop(2.0f, Unit.Centimetre);
                page.MarginBottom(1.5f, Unit.Centimetre);
                page.MarginLeft(2.8f, Unit.Centimetre);
                page.MarginRight(2.8f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11f).FontFamily("Arial").FontColor(Colors.Black).LineHeight(1.22f));

                // Header
                page.Header().Column(headerCol =>
                {
                    headerCol.Item().Row(row =>
                    {
                        // Logo a la izquierda
                        if (_logoBytes != null && _logoBytes.Length > 0)
                        {
                            row.ConstantItem(150).MaxHeight(55).Image(_logoBytes).FitArea();
                        }
                        else
                        {
                            row.ConstantItem(150);
                        }

                        // Razón social y RIF a la derecha
                        row.RelativeItem().AlignRight().Column(empresaCol =>
                        {
                            empresaCol.Item().AlignRight().Text(_dto.SociedadNombre.ToUpper(_cultura))
                                .FontSize(10f)
                                .Bold();

                            string rifTexto = string.IsNullOrWhiteSpace(_dto.SociedadRif)
                                ? ""
                                : (_dto.SociedadRif.StartsWith("R.I.F.", StringComparison.OrdinalIgnoreCase)
                                    ? _dto.SociedadRif
                                    : $"R.I.F. {_dto.SociedadRif}");

                            if (!string.IsNullOrWhiteSpace(rifTexto))
                            {
                                empresaCol.Item().AlignRight().Text(rifTexto)
                                    .FontSize(10f);
                            }
                        });
                    });
                });

                // Contenido principal de la carta
                page.Content().PaddingTop(25).Column(col =>
                {
                    // TÍTULO: C E R T I F I C A C I Ó N
                    col.Item().AlignCenter().Text("C E R T I F I C A C I Ó N")
                        .FontSize(11.5f)
                        .Bold();

                    col.Item().PaddingTop(12);

                    // PÁRRAFO 1: Declaración de la Secretaría y Custodio
                    col.Item().Text(text =>
                    {
                        text.Justify();
                        text.Span("Yo, ");
                        text.Span($"{_dto.SecretarioNombre}, ");
                        string secCedulaFormateada = UtilClass.FormatearDocumentoPersonaNatural(null, _dto.SecretarioCedula);
                        text.Span($"{_dto.SecretarioNacionalidad}, mayor de edad, domiciliada en la ciudad de Caracas y titular de la cédula de identidad N° {secCedulaFormateada},  procediendo en mi carácter de {_dto.SecretarioCargo} de ");
                        text.Span(_dto.SociedadNombre).Bold();

                        string datosConst = _dto.SociedadDatosConstitucion?.Trim() ?? "";
                        string domicilioSoc = string.IsNullOrWhiteSpace(_dto.SociedadDomicilio) ? "Caracas" : _dto.SociedadDomicilio.Trim();

                        if (datosConst.StartsWith("inscrita ante", StringComparison.OrdinalIgnoreCase))
                        {
                            text.Span($", sociedad mercantil domiciliada en {domicilioSoc} e {datosConst}, así como también de Custodio de sus Libros, ");
                        }
                        else if (datosConst.StartsWith("ante el", StringComparison.OrdinalIgnoreCase))
                        {
                            text.Span($", sociedad mercantil domiciliada en {domicilioSoc} e inscrita {datosConst}, así como también de Custodio de sus Libros, ");
                        }
                        else if (!string.IsNullOrWhiteSpace(datosConst))
                        {
                            text.Span($", sociedad mercantil domiciliada en {domicilioSoc} e inscrita ante el {datosConst}, así como también de Custodio de sus Libros, ");
                        }
                        else
                        {
                            text.Span($", sociedad mercantil domiciliada en {domicilioSoc}, así como también de Custodio de sus Libros, ");
                        }

                        text.Span("certifico").Bold();
                        string nombreSocCorto = !string.IsNullOrWhiteSpace(_dto.SociedadNombreCorto) ? _dto.SociedadNombreCorto : _dto.SociedadNombre;
                        text.Span($": Que en el Libro de Accionistas de {nombreSocCorto}, sociedad antes identificada, figura:");
                    });

                    col.Item().PaddingTop(12);

                    // NOMBRE DEL ACCIONISTA (Centrado y en negrita)
                    string nombreMostrar = _dto.AccionistaNombre?.Trim().ToUpper(_cultura) ?? "";
                    col.Item().AlignCenter().Text($"- {nombreMostrar} -")
                        .FontSize(11f)
                        .Bold();

                    col.Item().PaddingTop(8);

                    // PÁRRAFO DE DETALLES DEL ACCIONISTA SEGÚN TIPO
                    if (_dto.TipoAccionistaId == 1) // Persona Natural
                    {
                        col.Item().Text(text =>
                        {
                            text.Justify();
                            string nac = string.IsNullOrWhiteSpace(_dto.AccionistaNacionalidad) ? "venezolano" : _dto.AccionistaNacionalidad.Trim().ToLower(_cultura);
                            string docFormateado = UtilClass.FormatearDocumentoPersonaNatural(null, _dto.AccionistaDocumento);
                            text.Span($"{nac}, mayor de edad y titular de la cédula de identidad N° {docFormateado}, como poseedor de:");
                        });
                    }
                    else if (_dto.TipoAccionistaId == 2) // Persona Jurídica
                    {
                        col.Item().Text(text =>
                        {
                            text.Justify();
                            string domAcc = string.IsNullOrWhiteSpace(_dto.AccionistaDomicilio) ? "Caracas" : _dto.AccionistaDomicilio.Trim();
                            string regAcc = _dto.AccionistaRegistro?.Trim() ?? "";

                            if (regAcc.StartsWith("inscrita ante", StringComparison.OrdinalIgnoreCase))
                            {
                                text.Span($"sociedad mercantil domiciliada en {domAcc}, e {regAcc}, como poseedora de:");
                            }
                            else if (regAcc.StartsWith("ante el", StringComparison.OrdinalIgnoreCase))
                            {
                                text.Span($"sociedad mercantil domiciliada en {domAcc}, e inscrita {regAcc}, como poseedora de:");
                            }
                            else if (!string.IsNullOrWhiteSpace(regAcc))
                            {
                                text.Span($"sociedad mercantil domiciliada en {domAcc}, e inscrita ante el {regAcc}, como poseedora de:");
                            }
                            else
                            {
                                text.Span($"sociedad mercantil domiciliada en {domAcc}, como poseedora de:");
                            }
                        });
                    }
                    else if (_dto.TipoAccionistaId == 3) // Sucesión
                    {
                        col.Item().AlignCenter().Text(text =>
                        {
                            string exp = !string.IsNullOrWhiteSpace(_dto.ExpedienteSucesion) ? _dto.ExpedienteSucesion.Trim() : "150443";
                            string rif = !string.IsNullOrWhiteSpace(_dto.RifSucesion) ? _dto.RifSucesion.Trim() : _dto.AccionistaDocumento;
                            text.Span($"Certificado de Solvencia de Sucesiones, Expediente N° {exp}, R.I.F.: {rif}, como poseedora de:");
                        });
                    }

                    col.Item().PaddingTop(12);

                    // NÚMERO DE ACCIONES EN LETRAS Y NÚMEROS (Centrado y en negrita)
                    string nroAccFormat = _dto.NroAcciones.ToString("N0", _cultura);
                    col.Item().AlignCenter().Text($"– {_dto.NroAccionesEnLetras} ({nroAccFormat}) –")
                        .FontSize(11f)
                        .Bold();

                    col.Item().PaddingTop(8);

                    // PÁRRAFO DE ACCIONES Y VALOR NOMINAL
                    string valorNomFormat = _dto.ValorNominal.ToString("0.000", _cultura);
                    if (_dto.ValorNominal < 0.001m && _dto.ValorNominal > 0)
                    {
                        valorNomFormat = _dto.ValorNominal.ToString("0.0000", _cultura);
                    }

                    if (_dto.TipoAccionistaId == 3) // Sucesión: incluye porcentaje de capital social y valor patrimonial
                    {
                        col.Item().Text(text =>
                        {
                            text.Justify();
                            text.Span("acciones nominativas de mi representada, totalmente pagadas, de valor nominal ");
                            text.Span($"{_dto.ValorNominalEnLetras} (Bs. {valorNomFormat})").Bold();
                            string pctFormat = _dto.PorcentajeAcciones.ToString("0.0000", _cultura).TrimEnd('0').TrimEnd(',');
                            if (string.IsNullOrEmpty(pctFormat)) pctFormat = "0,0007";
                            text.Span($" cada una, que representan el {pctFormat}% del Capital Social total de la Compañía.");
                        });

                        col.Item().PaddingTop(12);

                        // Párrafo de Valor Patrimonial o en Libros para Sucesión
                        col.Item().Text(text =>
                        {
                            text.Justify();
                            string fechaBal = !string.IsNullOrWhiteSpace(_dto.FechaBalance) ? _dto.FechaBalance : $"30 de septiembre de {_dto.FechaEmision.Year - 1}";
                            string valorPatFormat = _dto.ValorPatrimonial.ToString("N2", _cultura);
                            text.Span("Certifico, igualmente, que el valor patrimonial o valor en libros de cada una de dichas acciones ascendía, según el Estado Consolidado de Situación Financiera de esta sociedad al ");
                            text.Span(fechaBal);
                            text.Span(", en valores constantes, a la cantidad de ");
                            text.Span($"{_dto.ValorPatrimonialEnLetras} (Bs. {valorPatFormat}).").Bold();
                        });
                    }
                    else // Persona Natural y Jurídica
                    {
                        col.Item().Text(text =>
                        {
                            text.Justify();
                            text.Span("acciones nominativas de mi representada, totalmente pagadas, de valor nominal ");
                            text.Span($"{_dto.ValorNominalEnLetras} (Bs. {valorNomFormat})").Bold();
                            text.Span(" cada una.");
                        });
                    }

                    col.Item().PaddingTop(12);

                    // FECHA DE EXPEDICIÓN
                    col.Item().Text(text =>
                    {
                        text.Justify();
                        text.Span($"Certificación que se expide en la ciudad de Caracas, a los {_dto.DiaEnLetras} ({_dto.Dia}) días del mes de {_dto.MesNombre} del año {_dto.AnoEnLetras} ({_dto.Ano}).");
                    });

                    col.Item().PaddingTop(24);

                    // ATENTAMENTE
                    col.Item().AlignCenter().Text("Atentamente,")
                        .FontSize(11f);
                });

                // Footer corporativo
                page.Footer().Column(footerCol =>
                {
                    footerCol.Item().AlignCenter().Text("2da. Avenida de Los Cortijos de Lourdes, Edificio Centro Empresarial Polar")
                        .FontSize(9f)
                        .FontColor(Colors.Black);

                    footerCol.Item().AlignCenter().Text("Caracas, Venezuela – Teléfono: +58 212 202.3111")
                        .FontSize(9f)
                        .FontColor(Colors.Black);
                });
            });
        }
    }
}
