using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IDocumentoModulo
    {
        Task<List<DocumentoModulo>> GetAllAsync();
        Task<DocumentoModulo?> GetByIdAsync(int id);
        Task<List<DocumentoModulo>> GetByModuloIdAsync(int moduloid);
        Task AddAsync(DocumentoModuloDto entity);
        void Delete(int id);
    }
}
