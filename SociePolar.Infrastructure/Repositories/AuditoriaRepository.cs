using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Infrastructure.Repositories
{
    public class AuditoriaRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IAuditoria
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<Auditoria>> GetByModuloIdAsync(int moduloid)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Auditoria>()
                .Where(x => x.ModuloId == moduloid)
                .ToListAsync();
        }
    }
}