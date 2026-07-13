using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface ITipoDocumentoSoporte
    {
        Task<List<TipoDocumentoSoporte>> GetAllAsync();
        Task<TipoDocumentoSoporte?> GetByIdAsync(int id);
        Task AddAsync(TipoDocumentoSoporte entity);
        void Update(TipoDocumentoSoporte entity);
        void Delete(int id);
    }
}
