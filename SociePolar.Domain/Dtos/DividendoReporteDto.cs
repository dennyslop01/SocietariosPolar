using System;

namespace SociePolar.Domain.Dtos
{
    public class DividendoListadoReporteDto
    {
        public int Numero { get; set; }
        public int DividendoDefinitivoId { get; set; }
        public string? ProcesoNombre { get; set; }
        public string? SociedadNombre { get; set; }
        public int AccionistaId { get; set; }
        public int? TipoAccionistaId { get; set; }
        public string? CedulaRif { get; set; }
        public string? NombreAccionista { get; set; }
        public long CantidadAcciones { get; set; }
        public string? Correos { get; set; }
        public string? Telefonos { get; set; }
        public string? Banco { get; set; }
        public string? TipoCuenta { get; set; }
        public string? NroCuenta { get; set; }
        public string? TitularCuenta { get; set; }
        public string? TipoPago { get; set; }
        public string? MonedaPago { get; set; }
        public bool Notificado { get; set; }
        public DateTime? FechaNotificacion { get; set; }
        public decimal MontoDecretado { get; set; }
        public DateTime? FechaDecreto { get; set; }
        public decimal MontoRetenido { get; set; }
        public decimal MontoNetoPagado => MontoDecretado - MontoRetenido;
        public string? EjercicioFiscal { get; set; }
        public decimal Porcion1Monto { get; set; }
        public decimal Porcion1Porcentaje { get; set; }
        public DateTime? Porcion1FechaPago { get; set; }
        public decimal Porcion2Monto { get; set; }
        public decimal Porcion2Porcentaje { get; set; }
        public DateTime? Porcion2FechaPago { get; set; }
        public decimal Porcion3Monto { get; set; }
        public decimal Porcion3Porcentaje { get; set; }
        public DateTime? Porcion3FechaPago { get; set; }
        public decimal Porcion4Monto { get; set; }
        public decimal Porcion4Porcentaje { get; set; }
        public DateTime? Porcion4FechaPago { get; set; }
        public string? Observaciones { get; set; }
        public bool SoporteEnviadoP1 { get; set; }
        public DateTime? SoporteFechaP1 { get; set; }
        public bool SoporteEnviadoP2 { get; set; }
        public DateTime? SoporteFechaP2 { get; set; }
        public bool SoporteEnviadoP3 { get; set; }
        public DateTime? SoporteFechaP3 { get; set; }
        public bool SoporteEnviadoP4 { get; set; }
        public DateTime? SoporteFechaP4 { get; set; }
        public string? RutaCarta { get; set; }
        public string? NombreCarta { get; set; }
        public string? RutaSoporte1 { get; set; }
        public string? NombreSoporte1 { get; set; }
        public string? RutaSoporte2 { get; set; }
        public string? NombreSoporte2 { get; set; }
        public DateTime FechaProceso { get; set; }
        public string? MonedaSimbolo { get; set; }
    }

    public class DividendoEncabezadoReporteDto
    {
        public int SociedadId { get; set; }
        public string? NombreEmpresa { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int TotalProcesos { get; set; }
        public int TotalAccionistas { get; set; }
        public long CantidadTotalAcciones { get; set; }
        public decimal MontoTotalDecretado { get; set; }
        public decimal MontoTotalRetenido { get; set; }
        public decimal MontoTotalNetoPagado => MontoTotalDecretado - MontoTotalRetenido;
        public string? MonedaSimbolo { get; set; }
    }
}
