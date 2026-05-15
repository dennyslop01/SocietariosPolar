using SociePolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Application.Interfaces
{
    public interface IUnidadNegocio
    {
        Task<List<UnidadNegocio>> GetAllAsync();
        Task<UnidadNegocio?> GetByIdAsync(int id);
        Task AddAsync(UnidadNegocio entity);
        void Update(UnidadNegocio entity);
        void Delete(int id);
    }
}
