using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IMoneda
    {
        Task<List<Moneda>> GetAllAsync();
        Task<Moneda?> GetByIdAsync(int id);
        Task AddAsync(Moneda entity);
        Task UpdateAsync(Moneda entity);
        Task DeleteAsync(int id);
    }
}
