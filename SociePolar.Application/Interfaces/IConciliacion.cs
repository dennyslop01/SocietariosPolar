using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IConciliacion
    {
        Task<List<Conciliacion>> GetAllAsync();
        Task<Conciliacion?> GetByIdAsync(Int32 id);
        Task<Int32> AddAsync(ConciliacionDto entity);
        Task AddDetalleAsync(List<ConciliacionDetalle> entity);
        Task<List<ConciliacionDetalle>?> GetDetalleByIdAsync(Int32 id);
        Task<ConciliacionDetalle?> GetDetalleByIdItemAsync(Int32 id);
    }
}
