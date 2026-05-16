namespace SociePolar.Domain.Entities
{
    public class Sociedad
    {
        public int Id { get; set; }
        public Region? Region { get; set; }
        public UnidadNegocio? UnidadNegocio { get; set; }
        public Empresa? Empresa { get; set; }
        public string? NumeroSap { get; set; }
        public TipoSociedad? TipoSociedad { get; set; }
        public EstatusSociedad? EstatusSociedad { get; set; }
        public TipoSociedadActiva? TipoSociedadActiva { get; set; }
        public string? Objeto { get; set; }
        public string? Domicilio { get; set; }
        public string? DireccionFiscal { get; set; }
        public string? DatosConstitucion { get; set; }
        public DateTime? FechaConstitucion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int? Duracion { get; set; }
        public int? NumeroAcciones { get; set; }
        public int? AplicaCapital { get; set; }
        public Moneda? Moneda { get; set; }
        public decimal? CapitalSuscrito { get; set; }
        public decimal? CapitalPagado { get; set; }
        public string? ClaseAcciones { get; set; }
        public string? FormaAdministracion { get; set; }
        public string? EjercicioEconomico { get; set; }
        public string? NumeroExpediente { get; set; }
        public string? Observaciones { get; set; }
        public string? Rif { get; set; }
        public string? Nit { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
