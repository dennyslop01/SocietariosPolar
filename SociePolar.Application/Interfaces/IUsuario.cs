using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IUsuario
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task AddAsync(Usuario entity);
        void Update(Usuario entity);
        void Delete(int id);
    }
}
