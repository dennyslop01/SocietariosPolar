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
    }
}
