namespace SociePolar.Domain.Entities
{
    public class Conciliacion
    {
        public int Id { get; set; }
        public Sociedad? Sociedad { get; set; }
        public string? TipoArchivo { get; set; }
        public string? NombreConciliaciones { get; set; }
        public string? RutaConciliaciones { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaArchivo { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
    }

    public class ConciliacionArchivo
    {
        public int Id { get; set; }
        public int ConciliacionId { get; set; }
        public string NombreAccionista { get; set; } = string.Empty;
        public string Rif { get; set; } = string.Empty;
        public int TotalAcciones { get; set; }
        public string Accion { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
    }

    public class ConciliacionDetalle
    {
        public int Id { get; set; }
        public int ConciliacionId { get; set; }
        public int AccionistaId { get; set; }
        public int TotalAcciones { get; set; }
        public string Accion { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
    }
}
