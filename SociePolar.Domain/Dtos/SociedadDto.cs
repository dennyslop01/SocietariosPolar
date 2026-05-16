using SociePolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Domain.Dtos
{
    public class SociedadDto
    {
        public int Id { get; set; }
        public int? RegionId { get; set; }
        public int? UnidadNegocioId { get; set; }
        public int? EmpresaId { get; set; }
        public string? NumeroSap { get; set; }
        public int? TipoSociedadId { get; set; }
        public int? EstatusSociedadId { get; set; }
        public int? TipoSociedadActivaId { get; set; }
        public string? Objeto { get; set; }
        public string? Domicilio { get; set; }
        public string? DireccionFiscal { get; set; }
        public string? DatosConstitucion { get; set; }
        public DateTime? FechaConstitucion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int? Duracion { get; set; }
        public int? NumeroAcciones { get; set; }
        public int? AplicaCapital { get; set; }
        public int? MonedaId { get; set; }
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
