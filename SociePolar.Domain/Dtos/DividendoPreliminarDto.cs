using SociePolar.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Dtos
{
    public class DividendoPreliminarDto
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Sociedad válida.")]
        public int? SociedadId { get; set; }

        [Required(ErrorMessage = "La Explicación es requerida.")]
        public string? Explicacion { get; set; }
        public string? NombreDividendos { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un documento.")] 
        public string? RutaDividendos { get; set; }
        public string? NombreActa { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un documento.")] 
        public string? RutaActa { get; set; }
        public string? NombreDocumento { get; set; }
        public string? RutaDocumento { get; set; }
        public string? Observaciones { get; set; }

        [Range(100, double.MaxValue, ErrorMessage = "El Monto Pagado en Tesorería debe ser un valor mayor a cien.")]
        public decimal MontoPagadoTesoreria { get; set; }

        [Range(100, double.MaxValue, ErrorMessage = "El Monto Pagado a Accionistas debe ser un valor mayor a cien.")]
        public decimal MontoPagadoAccionistas { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
    }
}
