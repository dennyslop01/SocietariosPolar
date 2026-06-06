using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
