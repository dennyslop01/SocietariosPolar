using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Dtos
{
    public class AutoridadDto
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Sociedad válida.")] 
        public int? SociedadId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Cargo válido.")] 
        public int? CargoId { get; set; }

        [Required(ErrorMessage = "El Nombre es requerido.")] 
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El Documento es requerido.")] 
        public string? Documento { get; set; }

        [Required(ErrorMessage = "El Tipo de Documento es requerido.")] 
        public string? TipoDocumento { get; set; }

        [Required(ErrorMessage = "La Duración es requerida.")]
        public int? Duracion { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
