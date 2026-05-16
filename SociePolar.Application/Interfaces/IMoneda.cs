using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IMoneda
    {
        Task<List<Moneda>> GetAllAsync();
        Task<Moneda?> GetByIdAsync(int id);
    }
}
