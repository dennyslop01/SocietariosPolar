namespace SociePolar.Domain.Interfaces
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        string? Nombre { get; set; }
        DateTime CreateDate { get; set; }
        DateTime UpdateDate { get; set; }
        int CreateUserId { get; set; }
        int UpdateUserId { get; set; }
    }
}
