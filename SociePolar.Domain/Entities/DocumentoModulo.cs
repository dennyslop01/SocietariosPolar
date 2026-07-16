namespace SociePolar.Domain.Entities
{
    public class DocumentoModulo
    {
        public int Id { get; set; }
        public TipoDocumentoSoporte? TipoDocumentoSoporte { get; set; }
        public string? RutaGoogle { get; set; }
        public string? Comentarios { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
    }
}
