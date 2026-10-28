namespace SociePolar.Domain.Dtos
{
    public class AccionistaResumenDto
    {
        public int Id { get; set; }
        public string? Documento1 { get; set; }
        public string? Nombre { get; set; }
    }

    public class AccionistaSociedadResumenDto
    {
        public int Id { get; set; }
        public int AccionistaId { get; set; }
        public int? CustodioId { get; set; }
        public long? NroAcciones { get; set; }
        public int? EstatusAccionistaId { get; set; }
    }
}
