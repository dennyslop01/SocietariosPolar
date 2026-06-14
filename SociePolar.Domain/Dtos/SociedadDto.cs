using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Dtos
{
    public class SociedadDto
    {
        public int Id { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Región válida.")]
        public int? RegionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Unidad de Negocio.")]
        public int? UnidadNegocioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Nombre de Sociedad.")]
        public int? EmpresaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Moneda.")]
        public int? MonedaId { get; set; }

        [Required(ErrorMessage = "El Número SAP es requerido.")]
        public string? NumeroSap { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Sociedad.")]
        //public int? TipoSociedadId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Estatus.")]
        public int? EstatusSociedadId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Tipo de Sociedad.")]
        public int? TipoSociedadActivaId { get; set; }

        [Required(ErrorMessage = "El objeto de la sociedad es requerido.")]
        public string? Objeto { get; set; }
        
        public string? Domicilio { get; set; }
        public string? DireccionFiscal { get; set; }
        public string? DatosConstitucion { get; set; }
        public DateTime? FechaConstitucion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int? Duracion { get; set; }
        public string? NumeroAcciones { get; set; }
        public int? AplicaCapital { get; set; }
        public decimal? CapitalSuscrito { get; set; }
        public decimal? CapitalPagado { get; set; }
        public string? ClaseAcciones { get; set; }
        public string? FormaAdministracion { get; set; }
        public string? EjercicioEconomico { get; set; }
        public string? NumeroExpediente { get; set; }
        public string? Observaciones { get; set; }
        public string? Rif { get; set; }
        public string? Nit { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? CreateUserId { get; set; }
        public int? UpdateUserId { get; set; }

        public decimal? ValorAccion { get; set; }
        public decimal? ValorPatrimonial { get; set; }
        public int? AnoPublicacion { get; set; }
        public string? NumeroPublicacion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public int? NombreDiarioId { get; set; }

    }

    public class SociedadInactivaDto
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Región válida.")]
        public int? RegionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Unidad de Negocio.")]
        public int? UnidadNegocioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Nombre de Sociedad.")]
        public int? EmpresaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Moneda.")]
        public int? MonedaId { get; set; }

        [Required(ErrorMessage = "El Número SAP es requerido.")]
        public string? NumeroSap { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Sociedad.")]
        //public int? TipoSociedadId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Estatus.")]
        public int? EstatusSociedadId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Tipo de Sociedad.")]
        public int? TipoSociedadActivaId { get; set; }

        [Required(ErrorMessage = "El objeto de la sociedad es requerido.")]
        public string? Objeto { get; set; }

        public string? Domicilio { get; set; }
        public string? DireccionFiscal { get; set; }
        public string? DatosConstitucion { get; set; }
        public DateTime? FechaConstitucion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int? Duracion { get; set; }
        public string? NumeroAcciones { get; set; }
        public int? AplicaCapital { get; set; }
        public decimal? CapitalSuscrito { get; set; }
        public decimal? CapitalPagado { get; set; }
        public string? ClaseAcciones { get; set; }
        public string? FormaAdministracion { get; set; }
        public string? EjercicioEconomico { get; set; }
        public string? NumeroExpediente { get; set; }
        public string? Observaciones { get; set; }
        public string? Rif { get; set; }
        public string? Nit { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? CreateUserId { get; set; }
        public int? UpdateUserId { get; set; }

        public decimal? ValorAccion { get; set; }
        public decimal? ValorPatrimonial { get; set; }
        public int? AnoPublicacion { get; set; }
        public string? NumeroPublicacion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public int? NombreDiarioId { get; set; }

    }
}
