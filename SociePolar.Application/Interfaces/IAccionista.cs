using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IAccionista
    {
        Task<List<Accionista>> GetAllAsync();
        Task<Accionista?> GetByIdAsync(int id);
        Task<Accionista?> GetByRifAsync(string tipoDoc, string rif);

        //Task<List<Accionista>> GetBySociedadIdAsync(int sociedadId);
        Task AddAsync(AccionistaDto entity);
        Task<Accionista> AddReturnEntidadAsync(AccionistaDto entity);
        void Update(AccionistaDto entity);
        void Delete(int id);
    }
}
