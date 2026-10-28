using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface IAccionista
    {
        Task<List<Accionista>> GetAllAsync();
        Task<(List<AccionistaMostrarDto> Items, int TotalCount)> GetPagedAsync(
                int pageNumber,
                int pageSize,
                string? searchTerm = null,
                string? sortColumn = null,
                bool sortAscending = true);
        Task<List<Accionista>> GetByNameAsync(string name);
        Task<Accionista?> GetByIdAsync(int id);
        Task<Accionista?> GetByRifAsync(string tipoDoc, string rif);

        Task AddAsync(AccionistaDto entity);
        Task<Accionista> AddReturnEntidadAsync(AccionistaDto entity);
        Task<List<AccionistaResumenDto>> GetResumenListAsync();
        void Update(AccionistaDto entity);
        void Delete(int id);
    }
}
