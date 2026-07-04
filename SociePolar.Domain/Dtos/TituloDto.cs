using SociePolar.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Dtos
{
    public class TituloDto
    {
        public int Id { get; set; }

        public int? AccionistaSociedadId { get; set; }

        [Required(ErrorMessage = "El Número es requerido.")] 
        public string? Numero { get; set; }

        [Required(ErrorMessage = "El Número de Acciones es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Número de Acciones es requerido.")]
        public int? Acciones { get; set; }

        public string? Preferente { get; set; }

        [Required(ErrorMessage = "La Ubicación es requerida.")] 
        public string? Ubicacion { get; set; }
        
        public int? Anulado { get; set; }
        public int? Endosado { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida.")] 
        public DateTime Fecha { get; set; }
        
        public string? Observaciones { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
