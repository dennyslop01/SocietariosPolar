namespace SociePolar.Domain.Entities
{
    public class AuditoriaNroAccion
    {
        public int Id { get; set; }
        public int SociedadId { get; set; }
        public int AccionistaId { get; set; }
        public Int64 NroAcciones { get; set; }
        public string? Accion { get; set; }
        public string? Descripcion { get; set; }
        public int CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
