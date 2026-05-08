using SociePolar.Domain.Interfaces;

namespace SociePolar.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class, IBaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(int id);
        //Task SaveAsync();
    }
}
