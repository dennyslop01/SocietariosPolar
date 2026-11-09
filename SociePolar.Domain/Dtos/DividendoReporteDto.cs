namespace SociePolar.Domain.Dtos
{
    public class DividendoListadoReporteDto
    {
        public int Numero { get; set; }
        public int DividendoDefinitivoId { get; set; }
        public string? ProcesoNombre { get; set; }
        public int AccionistaId { get; set; }
        public string? CedulaRif { get; set; }
        public string? NombreAccionista { get; set; }
        public decimal MontoDecretado { get; set; }
        public decimal MontoRetenido { get; set; }
        public decimal MontoNetoPagado => MontoDecretado - MontoRetenido;
        public decimal Porcion1Monto { get; set; }
        public decimal Porcion2Monto { get; set; }
        public decimal Porcion3Monto { get; set; }
        public decimal Porcion4Monto { get; set; }
        public string? TipoPago { get; set; }
        public string? EjercicioFiscal { get; set; }
        public DateTime? FechaDecreto { get; set; }
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
        public decimal MontoTotalDecretado { get; set; }
        public decimal MontoTotalRetenido { get; set; }
        public decimal MontoTotalNetoPagado => MontoTotalDecretado - MontoTotalRetenido;
        public string? MonedaSimbolo { get; set; }
    }
}
