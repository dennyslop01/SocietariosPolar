namespace SociePolar.Domain.Dtos
{
    public class AccionistaListadoCorteDto
    {
        public int Numero { get; set; }
        public int AccionistaId { get; set; }
        public string? CedulaRif { get; set; }
        public string? NombreAccionista { get; set; }
        public long NroAcciones { get; set; }
        public decimal PorcentajeParticipacion { get; set; }
        public DateTime FechaEfectiva { get; set; }
        public string? OrigenDato { get; set; } // "Historico" o "Actual"
    }

    public class SociedadEncabezadoReporteDto
    {
        public int SociedadId { get; set; }
        public string? NombreEmpresa { get; set; }
        public decimal? CapitalSocial { get; set; }
        public long TotalAcciones { get; set; }
        public decimal? ValorNominal { get; set; }
        public string? Moneda { get; set; }
    }
}
