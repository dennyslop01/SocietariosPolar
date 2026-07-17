namespace SociePolar.Domain.Dtos
{
    public class DocumentoModuloDto
    {
        public int Id { get; set; }
        public int? TipoDocumentoSoporteId { get; set; }
        public int ReferenciaId { get; set; }
        public string? RutaGoogle { get; set; }
        public string? Comentarios { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
    }
}
