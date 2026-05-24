namespace SociePolar.Domain.Entities
{
    public class Autoridad
    {
        public int Id { get; set; }
        public Sociedad Sociedad { get; set; }
        public Cargo Cargo { get; set; }
        public string? Nombre { get; set; }
        public string? Documento { get; set; }
        public string? TipoDocumento { get; set; }
        public int? Duracion { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
