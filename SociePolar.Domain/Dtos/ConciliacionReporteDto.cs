using System;

namespace SociePolar.Domain.Dtos
{
    public class ConciliacionListadoReporteDto
    {
        public int Numero { get; set; }
        public int ConciliacionId { get; set; }
        public string? NombreArchivo { get; set; }
        public DateTime? FechaConciliacion { get; set; }
        public string? Accion { get; set; }
        public string? TipoDocumento { get; set; }
        public string? CedulaRif { get; set; }
        public string? NombreAccionista { get; set; }
        public long SaldoAnterior { get; set; }
        public long SaldoNuevo { get; set; }
        public long Variacion { get; set; }
    }

    public class ConciliacionEncabezadoReporteDto
    {
        public int SociedadId { get; set; }
        public string? NombreEmpresa { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int TotalProcesosConciliacion { get; set; }
        public long CantidadTotalAcciones { get; set; }
        public int TotalAccionistasConciliados { get; set; }
        public int TotalNuevos { get; set; }
        public int TotalActualizados { get; set; }
        public int TotalInactivados { get; set; }
        public long TotalSaldoAnterior { get; set; }
        public long TotalSaldoNuevo { get; set; }
        public long TotalVariacion { get; set; }
    }
}
