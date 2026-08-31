using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Dtos
{
    public class ConciliacionDto
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Sociedad válida.")]
        public int? SociedadId { get; set; }

        [Required(ErrorMessage = "Debe indicar el tipo de archivo.")]
        public string? TipoArchivo { get; set; }
        public string? NombreConciliaciones { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un documento.")]
        public string? RutaConciliaciones { get; set; }
        public string? Observaciones { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una fecha.")]
        public DateTime? FechaArchivo { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
    }

    public class ConciliacionDetalleDto
    {
        public int Id { get; set; }
        public string? Accion { get; set; }
        public string? Documento { get; set; }
        public string? NroDocumento { get; set; }
        public string? Nombre { get; set; }
        public Int64? TotalAcciones { get; set; }
    }
}
