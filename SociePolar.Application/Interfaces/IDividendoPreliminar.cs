using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IDividendoPreliminar
    {
        Task<List<DividendoPreliminar>> GetAllAsync();
        Task<DividendoPreliminar?> GetByIdAsync(Int32 id);
        Task<Int32> AddAsync(DividendoPreliminarDto entity);
        void Delete(Int32 id);
        Task AddDetalleAsync(List<DividendoDetalleModel> entity);

    }
}
