namespace SociePolar.Domain.Entities
{
    public class AccionistaSociedad
    {
        public int Id { get; set; }
        public Sociedad? Sociedad { get; set; }
        public Accionista? Accionista { get; set; }
        public EstatusAccionista? EstatusAccionista { get; set; }


        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
