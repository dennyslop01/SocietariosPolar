using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IAccionistaSociedad
    {
        Task<List<AccionistaSociedad>> GetAllAsync();
        Task<AccionistaSociedad?> GetByIdAsync(int id);
        Task<List<AccionistaSociedad>> GetBySociedadIdAsync(int sociedadId);
        Task<AccionistaSociedad?> GetBySociedadIdAccionistaIdAsync(int sociedadId, int accionistaId);
        Task AddAsync(AccionistaSociedadDto entity);
        Task<AccionistaSociedad> AddReturnEntidadAsync(AccionistaSociedadDto entity);
        void Update(AccionistaSociedadDto entity);
        void Delete(int id);
        void Activar(int id, int estatus);
        Task UpdateNroAccionesAsync(int accionistaid, int sociedadid, Int64 nroacciones, int updateUserId, int opcioaudi);
        Task<List<AuditoriaNroAccion>?> GetAuditoriaBySociedadIdAccionistaIdAsync(int sociedadId, int accionistaId);

    }
}
