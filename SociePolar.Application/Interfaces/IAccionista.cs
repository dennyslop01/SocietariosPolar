using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IAccionista
    {
        Task<List<Accionista>> GetAllAsync();
        Task<Accionista?> GetByIdAsync(int id);
        //Task<List<Accionista>> GetBySociedadIdAsync(int sociedadId);
        Task AddAsync(AccionistaDto entity);
        void Update(AccionistaDto entity);
        void Delete(int id);
        void Activar(int id, int estatus);
    }
}
