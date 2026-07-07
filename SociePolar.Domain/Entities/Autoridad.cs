namespace SociePolar.Domain.Entities
{
    public class Autoridad
    {
        public int Id { get; set; }
        public Sociedad Sociedad { get; set; }
        public Cargo Cargo { get; set; }
        public string? Nombre { get; set; }
        public TipoDocumento? TipoDocumento1 { get; set; }
        public TipoDocumento? TipoDocumento2 { get; set; }
        public string? Documento1 { get; set; }
        public string? Documento2 { get; set; }
        public string? Duracion { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
