namespace SociePolar.Domain.Entities
{
    public class LibroSocietario
    {
        public int Id { get; set; }
        public Sociedad? Sociedad { get; set; }
        public ClaseLibro? ClaseLibro { get; set; }
        public TipoLibro? TipoLibro { get; set; }
        public string? TomoUso  { get; set; }
        public string? Folios { get; set; }
        public string? LibrosSellados { get; set; }
        public string? Observaciones { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }

        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public DateTime? FechaSello { get; set; }
        public int? Vacio { get; set; }
        public string? Ubicacion { get; set; }
    }
}
