namespace SociePolar.Domain.Dtos
{
    public class AsambleaDto
    {
        public int Id { get; set; }
        public int? SociedadId { get; set; }
        public int? TipoAsambleaId { get; set; }
        public DateTime FechaCelebracion { get; set; }
        public int NumeroActa { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int? RegistroId { get; set; }
        public int NumeroRegistro { get; set; }
        public int Tomo { get; set; }
        public int AnoPublicacion { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public int NumeroPublicacion { get; set; }
        public int? NombreDiarioId { get; set; }
        public int AplicaReforma { get; set; }
        public int? TipoReformaId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
