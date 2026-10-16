using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IAccionistaSociedad
    {
        Task<List<AccionistaSociedad>> GetAllAsync();
        Task<List<AccionistaSociedad>> GetByNameAsync(int estatusId, string name);
        Task<List<AccionistaSociedad>> GetAllEstatusIdAsync(int estatusId);
        Task<(List<AccionistaSociedad> Items, int TotalCount)> GetPaginatedAsync(int estatusId, int pageNumber, int pageSize, string searchTerm);
        Task<AccionistaSociedad?> GetByIdAsync(int id);
        Task<List<AccionistaSociedad>> GetBySociedadIdAsync(int sociedadId);
        Task<List<AccionistaSociedad>> GetBySociedadIdEstatusIdAsync(int sociedadId, int estatusId);
        Task<AccionistaSociedad?> GetBySociedadIdAccionistaIdAsync(int sociedadId, int accionistaId);
        Task<long> GetTotalAccionesBySociedadIdAsync(int sociedadId, int estatusId);
        Task<List<AccionistaSociedad>> GetCustodiadosAsync(int sociedadId, int custodioId, int estatusId);
        Task<List<int>> GetCustodiosIdBySociedadAsync(int sociedadId, int estatusId);
        Task AddAsync(AccionistaSociedadDto entity);
        Task<AccionistaSociedad> AddReturnEntidadAsync(AccionistaSociedadDto entity);
        void Update(AccionistaSociedadDto entity);
        void Delete(int id);
        void Activar(int id, int estatus);
        Task UpdateNroAccionesAsync(int accionistaid, int sociedadid, Int64 nroacciones, int updateUserId, int opcioaudi);
        Task<List<AuditoriaNroAccion>?> GetAuditoriaBySociedadIdAccionistaIdAsync(int sociedadId, int accionistaId);
        Task AccionistasSociedadInactivar(int procesoid);
        Task<List<AccionistaSociedadHistorico>> GetHistoricoBySociedadIdRangeDateAsync(int sociedadId, DateTime endDate);
        Task<int> GetTotalAccionistasBySociedadIdAsync(int sociedadId, int estatusId);
        Task<int> GetTotalCustodiadosCountBySociedadIdAsync(int sociedadId, int estatusId);
        Task<List<Sociedad>> GetAllSociedadesAsync(int estatusId);
    }
}
