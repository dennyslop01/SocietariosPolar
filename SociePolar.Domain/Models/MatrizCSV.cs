using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Models
{
    public class MatrizCSV()
    {
        [Range(0, 1000, ErrorMessage = "El tipo separador es obligatorio.")]
        public int TipoSeparador { get; set; }
    }
}
