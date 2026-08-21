namespace SociePolar.Domain.Entities
{
    public class DocumentosVencidos
    {
        public int Id { get; set; }
        public string? Modulo { get; set; }
        public string? Documento { get; set; }
        public string? NroDocumento { get; set; }
        public string? Nombre { get; set; }
        public DateTime? FechaVencimiento { get; set; }
    }
}
