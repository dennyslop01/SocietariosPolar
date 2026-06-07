using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;

namespace SociePolar.Application.Interfaces
{
    public interface ILibroSocietario
    {
        Task<List<LibroSocietario>> GetAllAsync();
        Task<LibroSocietario?> GetByIdAsync(int id);
        Task<List<LibroSocietario>> GetBySociedadIdAsync(int sociedadId);
        Task AddAsync(LibroSocietarioDto entity);
        void Update(LibroSocietarioDto entity);
        void Delete(int id);
    }
}
