namespace SociePolar.Domain.Entities
{
    public class DividendoPreliminar
    {
        public int Id { get; set; }
        public Sociedad? Sociedad { get; set; }
        public string? Explicacion { get; set; }
        public string? NombreDividendos {  get; set; }
        public string? RutaDividendos { get; set; }
        public string? NombreActa {  get; set; }
        public string? RutaActa { get; set; }
        public string? NombreDocumento { get; set; }
        public string? RutaDocumento { get; set; }
        public string? Observaciones { get; set; }
        public decimal? MontoPagadoTesoreria { get; set; }
        public decimal? MontoPagadoAccionistas { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
    }

    public class DividendoDetalleModel
    {
        public int Id { get; set; }
        public int DividendoPreliminarId { get; set; }
        
        public int CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
