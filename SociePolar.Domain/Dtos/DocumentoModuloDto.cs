using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Dtos
{
    public class DocumentoModuloDto
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Tipo Documento válida.")]
        public int? TipoDocumentoSoporteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Referencia válida.")]
        public int ReferenciaId { get; set; }

        [Required(ErrorMessage = "El nombre del documento es obligatorio.")]
        public string? NombreDocumento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un documento.")]
        public string? RutaGoogle { get; set; }
        public string? Comentarios { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
    }
}
