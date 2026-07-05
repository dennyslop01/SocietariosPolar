namespace SociePolar.Domain.Entities
{
    public class Titulo
    {
        public int Id { get; set; }
        public AccionistaSociedad? AccionistaSociedad { get; set; }
        public string? Numero { get; set; }
        public int? Acciones { get; set; }
        public string? Preferente { get; set; }
        public string? Ubicacion { get; set; }
        public int? Anulado { get; set; }
        public int? Endosado { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Observaciones { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
