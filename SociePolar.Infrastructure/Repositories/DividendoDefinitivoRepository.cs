using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Infrastructure.Repositories
{
    public class DividendoDefinitivoRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IDividendoDefinitivo
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<DividendoDefinitivo>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DividendoDefinitivo>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .ToListAsync();
        }

        public async Task<DividendoDefinitivo?> GetByIdAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DividendoDefinitivo>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public void Delete(Int32 id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entitydetall = context.Set<DividendoDefinitivoDetalle>().Find(id);
            if (entitydetall != null)
            {
                context.Set<DividendoDefinitivoDetalle>().Remove(entitydetall);
                context.SaveChanges();
            }
            var entity = context.Set<DividendoDefinitivo>().Find(id);
            if (entity != null)
            {
                context.Set<DividendoDefinitivo>().Remove(entity);
                context.SaveChanges();
            }
        }

        public async Task<List<DividendoDefinitivoDetalle>?> GetDetalleByIdAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DividendoDefinitivoDetalle>()
                .Where(b => b.DividendoDefinitivoId == id)
                .ToListAsync();
        }
    }
}