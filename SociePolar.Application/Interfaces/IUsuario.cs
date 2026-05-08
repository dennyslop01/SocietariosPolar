using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IUsuario
    {
        Task<Usuario?> GetByEmailAsync(string email);
    }
}
