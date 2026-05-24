using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IAutoridad
    {
        Task<List<Autoridad>> GetAllAsync();
        Task<Autoridad?> GetByIdAsync(int id);
        Task AddAsync(AutoridadDto entity);
        void Update(AutoridadDto entity);
        void Delete(int id);
    }
}
