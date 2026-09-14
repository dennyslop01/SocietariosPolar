namespace SociePolar.WebApp.Utilities
{
    public static class UtilClass
    {
        public static string FormatearMiles(long? valor)
        {
            if (valor == null || valor == 0)
                return string.Empty;

            return valor.Value.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
        }

        public static string FormatearStringMiles(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor) || valor == "0")
                return string.Empty;

            string soloDigitos = System.Text.RegularExpressions.Regex.Replace(valor, @"[^\d]", "");
            return long.TryParse(soloDigitos, out long resultado) && resultado != 0
                ? resultado.ToString("N0", System.Globalization.CultureInfo.CurrentCulture)
                : (valor == "0" ? string.Empty : valor);
        }

        public static long? LimpiarYConvertirLong(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            string soloDigitos = System.Text.RegularExpressions.Regex.Replace(input, @"[^\d]", "");
            if (long.TryParse(soloDigitos, out long resultado))
            {
                return resultado;
            }
            return null;
        }

        public static string? LimpiarYConvertirStringMiles(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            string soloDigitos = System.Text.RegularExpressions.Regex.Replace(input, @"[^\d]", "");
            if (long.TryParse(soloDigitos, out long resultado))
            {
                return resultado == 0 ? null : resultado.ToString();
            }
            return null;
        }

        public static string FormatearNumero(decimal? valor, int decimales = 2)
        {
            if (valor == null || valor == 0) return "";
            return valor.Value.ToString($"N{decimales}", System.Globalization.CultureInfo.CurrentCulture);
        }
        // Toma el texto ingresado, remueve símbolos y normaliza separadores decimales/miles de forma inteligente
        public static decimal LimpiarYConvertir(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0;

            try
            {
                // 1. Limpieza inicial de caracteres no numéricos habituales
                string limpio = input.Trim()
                    .Replace("$", "")
                    .Replace("Bs.", "")
                    .Replace("Bs", "")
                    .Replace("USD", "")
                    .Replace(" ", "");

                if (string.IsNullOrWhiteSpace(limpio)) return 0;

                // 2. Determinar la función de puntos y comas según su posición
                int ultimoPunto = limpio.LastIndexOf('.');
                int ultimaComa = limpio.LastIndexOf(',');

                if (ultimoPunto >= 0 && ultimaComa >= 0)
                {
                    if (ultimaComa > ultimoPunto)
                    {
                        // Formato: 1.234.567,89 (punto = miles, coma = decimal)
                        limpio = limpio.Replace(".", "").Replace(',', '.');
                    }
                    else
                    {
                        // Formato: 1,234,567.89 (coma = miles, punto = decimal)
                        limpio = limpio.Replace(",", "");
                    }
                }
                else if (ultimaComa >= 0)
                {
                    // Solo contiene comas
                    int countComas = limpio.Count(c => c == ',');
                    if (countComas > 1)
                    {
                        // Múltiples comas: 1,000,000 -> miles
                        limpio = limpio.Replace(",", "");
                    }
                    else
                    {
                        // Una sola coma: 1234,56 -> decimal
                        limpio = limpio.Replace(',', '.');
                    }
                }
                else if (ultimoPunto >= 0)
                {
                    // Solo contiene puntos
                    int countPuntos = limpio.Count(c => c == '.');
                    if (countPuntos > 1)
                    {
                        // Múltiples puntos: 1.000.000 -> miles
                        limpio = limpio.Replace(".", "");
                    }
                    else
                    {
                        // Un solo punto: 1234.56 -> decimal (ya tiene punto)
                    }
                }

                // 3. Conversión final invariante garantizada
                if (decimal.TryParse(limpio, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal resultado))
                {
                    return resultado;
                }
            }
            catch
            {
                // Protege el formulario si se ingresan secuencias inválidas
            }
            return 0;
        }

        public static decimal LimpiarYConvertirA10Decimales(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0;

            try
            {
                // 1. Limpieza inicial de caracteres no numéricos habituales
                string limpio = input.Trim()
                                     .Replace("$", "")
                                     .Replace("Bs.", "")
                                     .Replace("Bs", "")
                                     .Replace("USD", "")
                                     .Replace(" ", "");

                if (string.IsNullOrWhiteSpace(limpio)) return 0;

                // 2. Determinar la función de puntos y comas según su posición
                int ultimoPunto = limpio.LastIndexOf('.');
                int ultimaComa = limpio.LastIndexOf(',');

                if (ultimoPunto >= 0 && ultimaComa >= 0)
                {
                    if (ultimaComa > ultimoPunto)
                    {
                        // Formato: 1.234.567,89 (punto = miles, coma = decimal)
                        limpio = limpio.Replace(".", "").Replace(',', '.');
                    }
                    else
                    {
                        // Formato: 1,234,567.89 (coma = miles, punto = decimal)
                        limpio = limpio.Replace(",", "");
                    }
                }
                else if (ultimaComa >= 0)
                {
                    // Solo contiene comas
                    int countComas = limpio.Count(c => c == ',');
                    if (countComas > 1)
                    {
                        // Múltiples comas: 1,000,000 -> miles
                        limpio = limpio.Replace(",", "");
                    }
                    else
                    {
                        // Una sola coma: 1234,56 -> decimal
                        limpio = limpio.Replace(',', '.');
                    }
                }
                else if (ultimoPunto >= 0)
                {
                    // Solo contiene puntos
                    int countPuntos = limpio.Count(c => c == '.');
                    if (countPuntos > 1)
                    {
                        // Múltiples puntos: 1.000.000 -> miles
                        limpio = limpio.Replace(".", "");
                    }
                }

                // 3. Conversión final invariante garantizada y redondeo a 10 decimales
                if (decimal.TryParse(limpio, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal resultado))
                {
                    // Redondea a 10 decimales exactos usando MidpointRounding.AwayFromZero (redondeo estándar)
                    return Math.Round(resultado, 10, MidpointRounding.AwayFromZero);
                }
            }
            catch
            {
                // Protege el formulario si se ingresan secuencias inválidas
            }

            return 0;
        }
    }
}
