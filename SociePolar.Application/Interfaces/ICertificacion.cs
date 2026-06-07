using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface ICertificacion
    {
        Task<List<Certificacion>> GetAllAsync();
        Task<Certificacion?> GetByIdAsync(int id);
        Task<List<Certificacion>> GetBySociedadIdAsync(int sociedadId);
        Task AddAsync(CertificacionDto entity);
        void Update(CertificacionDto entity);
        void Delete(int id);
    }
}
