namespace SociePolar.Domain.Dtos
{
    public class AutoridadDto
    {
        public int Id { get; set; }
        public int? SociedadId { get; set; }
        public int? CargoId { get; set; }
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
