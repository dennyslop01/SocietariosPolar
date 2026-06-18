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

        public string? TomoUso { get; set; }
        public string? Folios { get; set; }
        public int? LibrosSellados { get; set; }
        public string? Observaciones { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }

        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public DateTime? FechaSello { get; set; }
        public int? Vacio { get; set; }
        public string? Ubicacion { get; set; }


    }
}
