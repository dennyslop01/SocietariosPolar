using System;
using System.Globalization;
using System.Text;

namespace SociePolar.WebApp.Utilities
{
    public static class NumeroALetrasHelper
    {
        private static readonly string[] UnidadesM = { "", "un", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
        private static readonly string[] UnidadesF = { "", "una", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };

        private static readonly string[] Especiales = {
            "diez", "once", "doce", "trece", "catorce", "quince",
            "dieciséis", "diecisiete", "dieciocho", "diecinueve", "veinte"
        };

        private static readonly string[] VeintenasM = {
            "", "veintiún", "veintidós", "veintitrés", "veinticuatro",
            "veinticinco", "veintiséis", "veintisiete", "veintiocho", "veintinueve"
        };

        private static readonly string[] VeintenasF = {
            "", "veintiuna", "veintidós", "veintitrés", "veinticuatro",
            "veinticinco", "veintiséis", "veintisiete", "veintiocho", "veintinueve"
        };

        private static readonly string[] Decenas = {
            "", "diez", "veinte", "treinta", "cuarenta",
            "cincuenta", "sesenta", "setenta", "ochenta", "noventa"
        };

        private static readonly string[] CentenasM = {
            "", "ciento", "doscientos", "trescientos", "cuatrocientos",
            "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos"
        };

        private static readonly string[] CentenasF = {
            "", "ciento", "doscientas", "trescientas", "cuatrocientas",
            "quinientas", "seiscientas", "setecientas", "ochocientas", "novecientas"
        };

        private static readonly string[] Meses = {
            "", "enero", "febrero", "marzo", "abril", "mayo", "junio",
            "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"
        };

        /// <summary>
        /// Convierte el número de acciones a letras en mayúsculas y concordancia femenina ("DOSCIENTAS...", "UNA", etc.).
        /// Ejemplo: 296253 -> "DOSCIENTAS NOVENTA Y SEIS MIL DOSCIENTAS CINCUENTA Y TRES"
        /// </summary>
        public static string ConvertirAcciones(long numero)
        {
            if (numero == 0) return "CERO";
            return ConvertirEntero(numero, esFemenino: true).Trim().ToUpper(new CultureInfo("es-VE"));
        }

        /// <summary>
        /// Convierte el día del mes a letras en minúsculas (ej: 22 -> "veintidós", 13 -> "trece", 6 -> "seis", 1 -> "un").
        /// </summary>
        public static string ConvertirDia(int dia)
        {
            if (dia == 1) return "un";
            return ConvertirEntero(dia, esFemenino: false).Trim().ToLower(new CultureInfo("es-VE"));
        }

        /// <summary>
        /// Convierte el año a letras en minúsculas (ej: 2026 -> "dos mil veintiséis", 2025 -> "dos mil veinticinco").
        /// </summary>
        public static string ConvertirAno(int ano)
        {
            return ConvertirEntero(ano, esFemenino: false).Trim().ToLower(new CultureInfo("es-VE"));
        }

        /// <summary>
        /// Devuelve el nombre del mes en minúsculas.
        /// </summary>
        public static string ObtenerMesNombre(int mes)
        {
            if (mes >= 1 && mes <= 12) return Meses[mes];
            return "";
        }

        /// <summary>
        /// Convierte un valor nominal en Bolívares a su expresión textual.
        /// Ejemplos:
        /// 0.001 -> "Una Milésima de Bolívar"
        /// 0.004 -> "Cuatro Milésimas de Bolívar"
        /// 0.0001 -> "Una Diezmilésima de Bolívar"
        /// 1.00 -> "Un Bolívar"
        /// </summary>
        public static string ConvertirValorNominal(decimal? valor)
        {
            if (valor == null || valor.Value == 0)
                return "Cero Bolívares";

            decimal v = Math.Abs(valor.Value);

            // Casos especiales de fracciones de bolívar frecuentes en polar
            if (v < 0.01m)
            {
                // Milésimas
                decimal milesimas = Math.Round(v * 1000m, 4);
                if (milesimas == Math.Floor(milesimas))
                {
                    long m = (long)milesimas;
                    if (m == 1) return "Una Milésima de Bolívar";
                    if (m > 1 && m < 1000)
                    {
                        string cant = Capitalizar(ConvertirEntero(m, esFemenino: true));
                        return $"{cant} Milésimas de Bolívar";
                    }
                }

                // Diezmilésimas
                decimal diezMilesimas = Math.Round(v * 10000m, 4);
                if (diezMilesimas == Math.Floor(diezMilesimas))
                {
                    long dm = (long)diezMilesimas;
                    if (dm == 1) return "Una Diezmilésima de Bolívar";
                    if (dm > 1 && dm < 10000)
                    {
                        string cant = Capitalizar(ConvertirEntero(dm, esFemenino: true));
                        return $"{cant} Diezmilésimas de Bolívar";
                    }
                }
            }

            return ConvertirMontoBolivares(v);
        }

        /// <summary>
        /// Convierte un monto en Bolívares y céntimos a letras con formato estándar.
        /// Ejemplo: 6.27 -> "Seis Bolívares con Veintisiete Céntimos"
        /// 1.00 -> "Un Bolívar"
        /// </summary>
        public static string ConvertirMontoBolivares(decimal? valor)
        {
            if (valor == null || valor.Value == 0)
                return "Cero Bolívares";

            decimal v = Math.Abs(valor.Value);
            long entero = (long)Math.Truncate(v);
            long centimos = (long)Math.Round((v - entero) * 100m);

            if (centimos == 100)
            {
                entero += 1;
                centimos = 0;
            }

            string enteroTexto = "";
            if (entero == 1)
            {
                enteroTexto = "Un Bolívar";
            }
            else if (entero > 1)
            {
                enteroTexto = Capitalizar(ConvertirEntero(entero, esFemenino: false)) + " Bolívares";
            }

            if (centimos == 0)
            {
                return string.IsNullOrEmpty(enteroTexto) ? "Cero Bolívares" : enteroTexto;
            }

            string centimosTexto = "";
            if (centimos == 1)
            {
                centimosTexto = "Un Céntimo";
            }
            else
            {
                centimosTexto = Capitalizar(ConvertirEntero(centimos, esFemenino: false)) + " Céntimos";
            }

            if (string.IsNullOrEmpty(enteroTexto))
            {
                return $"{centimosTexto} de Bolívar";
            }

            return $"{enteroTexto} con {centimosTexto}";
        }

        public static string ConvertirEntero(long numero, bool esFemenino = false)
        {
            if (numero == 0) return "cero";
            if (numero == 100) return "cien";

            var sb = new StringBuilder();

            // Trillones y Billones no suelen aplicarse, pero soportamos hasta billones
            if (numero >= 1000000000000)
            {
                long billones = numero / 1000000000000;
                numero %= 1000000000000;
                sb.Append(billones == 1 ? "un billón " : $"{ConvertirEntero(billones, false)} billones ");
            }

            if (numero >= 1000000)
            {
                long millones = numero / 1000000;
                numero %= 1000000;
                sb.Append(millones == 1 ? "un millón " : $"{ConvertirEntero(millones, false)} millones ");
            }

            if (numero >= 1000)
            {
                long miles = numero / 1000;
                numero %= 1000;
                sb.Append(miles == 1 ? "mil " : $"{ConvertirEntero(miles, esFemenino)} mil ");
            }

            if (numero > 0)
            {
                sb.Append(ConvertirCentenas((int)numero, esFemenino));
            }

            return sb.ToString().Trim();
        }

        private static string ConvertirCentenas(int num, bool esFemenino)
        {
            if (num == 0) return "";
            if (num == 100) return "cien";

            var sb = new StringBuilder();
            int c = num / 100;
            int resto = num % 100;

            if (c > 0)
            {
                sb.Append((esFemenino ? CentenasF[c] : CentenasM[c]) + " ");
            }

            if (resto >= 10 && resto <= 20)
            {
                sb.Append(Especiales[resto - 10]);
            }
            else if (resto >= 21 && resto <= 29)
            {
                sb.Append(esFemenino ? VeintenasF[resto - 20] : VeintenasM[resto - 20]);
            }
            else
            {
                int d = resto / 10;
                int u = resto % 10;

                if (d > 0)
                {
                    sb.Append(Decenas[d]);
                    if (u > 0)
                    {
                        sb.Append(" y ");
                        sb.Append(esFemenino && u == 1 ? "una" : (esFemenino ? UnidadesF[u] : UnidadesM[u]));
                    }
                }
                else if (u > 0)
                {
                    sb.Append(esFemenino ? UnidadesF[u] : UnidadesM[u]);
                }
            }

            return sb.ToString().Trim();
        }

        private static string Capitalizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "";
            texto = texto.Trim();
            if (texto.Length == 1) return texto.ToUpper(new CultureInfo("es-VE"));
            return char.ToUpper(texto[0], new CultureInfo("es-VE")) + texto.Substring(1);
        }
    }
}
