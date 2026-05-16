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
        void Delete(int id);
    }
}
