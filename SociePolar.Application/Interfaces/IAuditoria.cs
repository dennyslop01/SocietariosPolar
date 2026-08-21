using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IAuditoria
    {
        Task<List<Auditoria>> GetByModuloIdAsync(int moduloid);
    }
}
