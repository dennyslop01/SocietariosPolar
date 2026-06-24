using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface ISociedad
    {
        Task<List<Sociedad>> GetAllAsync();
        Task<Sociedad?> GetByIdAsync(int id);
        Task AddAsync(SociedadDto entity);
        void Update(SociedadDto entity);
        Task AddAsync(SociedadInactivaDto entity);
        void Update(SociedadInactivaDto entity);
        void Delete(int id);
        void Activar(int id, int accion);
    }
}
