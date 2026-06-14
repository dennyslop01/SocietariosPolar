using SociePolar.Domain.Interfaces;

namespace SociePolar.Domain.Entities
{
    public class TipoCuenta : IBaseEntity
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
