namespace SociePolar.Domain.Entities
{
    public class Asamblea
    {
        public int Id { get; set; }
        public Sociedad? Sociedad { get; set; }
        public TipoAsamblea? TipoAsamblea { get; set; }
        public DateTime? FechaCelebracion { get; set; }
        public string? NumeroActa { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public Registro? Registro { get; set; }
        public string? NumeroRegistro { get; set; }
        public string? Tomo { get; set; }
        public int? AnoPublicacion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public string? NumeroPublicacion { get; set; }
        public NombreDiario? NombreDiario { get; set; }
        public int? IndicadorAsamblea { get; set; }
        public int? AplicaReforma { get; set; }
        public TipoReforma? TipoReforma { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }

        public string? Observacion { get; set; }

    }
}
