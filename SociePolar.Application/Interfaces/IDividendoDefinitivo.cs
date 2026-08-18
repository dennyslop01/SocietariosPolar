using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Application.Interfaces
{
    public interface IDividendoDefinitivo
    {
        Task<List<DividendoDefinitivo>> GetAllAsync();
        Task<DividendoDefinitivo?> GetByIdAsync(Int32 id);
        void Delete(Int32 id);
        Task<List<DividendoDefinitivoDetalle>?> GetDetalleByIdAsync(Int32 id);
        Task<int> CreateAsync(DividendoDefinitivo entity);
        Task CreateDetalleAsync(List<DividendoDefinitivoDetalle> entities);
    }
}
