using SociePolar.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Application.Interfaces
{
    public interface IAuditoria
    {
        Task<List<Auditoria>> GetByModuloIdAsync(int moduloid);
    }
}
