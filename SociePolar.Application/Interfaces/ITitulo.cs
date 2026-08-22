using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface ITitulo
    {
        Task<List<Titulo>> GetAllAsync();
        Task<Titulo?> GetByIdAsync(int id);
        Task<List<Titulo>> GetByAccionistaSociedadIdAsync(int sociedadId);
        Task AddAsync(TituloDto entity);
        Task Update(TituloDto entity);
        Task Delete(int id);
    }
}
