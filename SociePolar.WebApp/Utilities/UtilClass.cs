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
            if (valor == null || valor == "0")
                return string.Empty;

            return long.TryParse(valor, out long resultado) ? resultado.ToString("N0", System.Globalization.CultureInfo.CurrentCulture) : valor;
        }

        public static string FormatearNumero(decimal? valor)
        {
            if (valor == null) return "";
            if (valor == 0) return "";
            return valor?.ToString("N2", System.Globalization.CultureInfo.CurrentCulture);
        }

        // Toma el texto del teclado, remueve separadores visuales y guarda el decimal puro en tu clase
        public static decimal LimpiarYConvertir(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0;

            try
            {
                string separadorMiles = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;

                // Eliminamos el separador de miles para que no rompa la conversión decimal
                string limpio = input.Replace(separadorMiles, "");

                if (decimal.TryParse(limpio, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out decimal resultado))
                {
                    return resultado;
                }
            }
            catch
            {
                // Protege el formulario de errores si escriben caracteres extraños
            }
            return 0;
        }
    }
}