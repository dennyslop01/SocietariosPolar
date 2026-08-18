namespace SociePolar.Domain.Entities
{
    public class DividendoDefinitivo
    {
        public int Id { get; set; }
        public Sociedad? Sociedad { get; set; }
        public string? Explicacion { get; set; }
        public string? NombreDividendos { get; set; }
        public string? RutaDividendos { get; set; }
        public string? NombreActa { get; set; }
        public string? RutaActa { get; set; }
        public string? NombreDocumento { get; set; }
        public string? RutaDocumento { get; set; }
        public string? Observaciones { get; set; }
        public decimal? MontoPagadoTesoreria { get; set; }
        public decimal? MontoPagadoAccionistas { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
    }

    public class DividendoDefinitivoDetalle
    {
        public int Id { get; set; }
        public int DividendoDefinitivoId { get; set; }
        public Accionista? Accionista { get; set; }
        public Moneda? Moneda { get; set; }

        //public string NombreAccionista { get; set; } = string.Empty;
        //public long CantidadAcciones { get; set; }
        //public string Rif { get; set; } = string.Empty;
        //public string Correos { get; set; } = string.Empty;
        //public string Telefonos { get; set; } = string.Empty;
        //public string Banco { get; set; } = string.Empty;
        //public string TipoCuenta { get; set; } = string.Empty;
        //public string NroCuenta { get; set; } = string.Empty;
        //public string TitularCuenta { get; set; } = string.Empty;
        public string TipoPago { get; set; } = string.Empty;
        //public string MonedaPago { get; set; } = string.Empty;
        public bool Notificado { get; set; }
        public DateTime? FechaNotificacion { get; set; }
        public decimal MontoDecretado { get; set; }
        public DateTime? FechaDecreto { get; set; }
        public decimal MontoRetenido { get; set; }
        public string EjercicioFiscal { get; set; } = string.Empty;
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
        public string Observaciones { get; set; } = string.Empty;
        public bool SoporteEnviadoP1 { get; set; }
        public DateTime? SoporteFechaP1 { get; set; }
        public bool SoporteEnviadoP2 { get; set; }
        public DateTime? SoporteFechaP2 { get; set; }
        public bool SoporteEnviadoP3 { get; set; }
        public DateTime? SoporteFechaP3 { get; set; }
        public bool SoporteEnviadoP4 { get; set; }
        public DateTime? SoporteFechaP4 { get; set; }
        public int CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
