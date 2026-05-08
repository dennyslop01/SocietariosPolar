namespace SociePolar.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? Cedula { get; set; }
        public string? NombreCorto { get; set; }
        public int? IdRol { get; set; }
        public int? Estatus { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
