using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Domain.Utils
{
    public static class DocumentoNormalizer
    {
        /// <summary>
        /// Normaliza un RIF o Cédula venezolana eliminando caracteres decorativos (guiones, puntos, espacios)
        /// y ceros a la izquierda, extrayendo el tipo de documento (V, E, J, G, P) y la raíz numérica limpia.
        /// </summary>
        public static (string TipoDoc, string NumeroPuro) Normalizar(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ("V", string.Empty);

            string limpio = input.Trim().ToUpper();
            string tipo = "V";

            // 1. Detectar letra inicial (V, E, J, G, P)
            if (limpio.StartsWith("V") || limpio.StartsWith("E") || limpio.StartsWith("J") || limpio.StartsWith("G") || limpio.StartsWith("P"))
            {
                tipo = limpio.Substring(0, 1);
                limpio = limpio.Substring(1);
            }

            // 2. Extraer solo los caracteres numéricos
            var digitosArr = limpio.Where(char.IsDigit).ToArray();
            string digitos = new string(digitosArr);

            // 3. Remover ceros a la izquierda para comparar peras con peras
            string numeroPuro = digitos.TrimStart('0');
            if (string.IsNullOrEmpty(numeroPuro) && digitos.Length > 0)
            {
                numeroPuro = "0";
            }

            return (tipo, numeroPuro);
        }
    }
}
