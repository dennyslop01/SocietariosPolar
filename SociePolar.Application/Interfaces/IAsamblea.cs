using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IAsamblea
    {
        Task<List<Asamblea>> GetAllAsync();
        Task<Asamblea?> GetByIdAsync(int id);
        Task<List<Asamblea>> GetBySociedadIdAsync(int sociedadId);
        Task AddAsync(AsambleaDto entity);
        void Update(AsambleaDto entity);
        void Delete(int id);
    }
}
