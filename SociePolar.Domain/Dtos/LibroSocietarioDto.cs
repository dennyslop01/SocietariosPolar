using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Dtos
{
    public class LibroSocietarioDto
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Sociedad válida.")] 
        public int? SociedadId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Clase de Libro válida.")] 
        public int? ClaseLibroId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Tipo de Libro válido.")] 
        public int? TipoLibroId { get; set; }

        [Required(ErrorMessage = "El campo Tomo de Uso es requerido.")] 
        public string? TomoUso { get; set; }
        [Required(ErrorMessage = "El campo Folio es requerido.")]
        public string? Folios { get; set; }
        [Required(ErrorMessage = "El campo Último Asiento es requerido.")]
        public string? UltimoAsiento { get; set; }
        [Required(ErrorMessage = "El campo Libros Sellados es requerido.")]
        public string? LibrosSellados { get; set; }
        public string? Observaciones { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
