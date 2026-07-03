using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IAccionistaSociedad
    {
        Task<List<AccionistaSociedad>> GetAllAsync();
        Task<AccionistaSociedad?> GetByIdAsync(int id);
        Task<List<AccionistaSociedad>> GetBySociedadIdAsync(int sociedadId);
        Task AddAsync(AccionistaSociedadDto entity);
        void Update(AccionistaSociedadDto entity);
        void Delete(int id);
        void Activar(int id, int estatus);
    }
}
