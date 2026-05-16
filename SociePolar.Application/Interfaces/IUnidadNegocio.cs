using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IUnidadNegocio
    {
        Task<List<UnidadNegocio>> GetAllAsync();
        Task<List<UnidadNegocio>> GetByIdRegionAllAsync(int idregion);
        Task<UnidadNegocio?> GetByIdAsync(int id);
        Task AddAsync(UnidadNegocio entity);
        void Update(UnidadNegocio entity);
        void Delete(int id);
    }
}
