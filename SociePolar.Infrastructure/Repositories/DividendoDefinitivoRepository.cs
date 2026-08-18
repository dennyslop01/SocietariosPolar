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
            context.Set<DividendoDefinitivoDetalle>()
                  .Where(x => x.DividendoDefinitivoId == id)
                  .ExecuteDelete();
            context.SaveChanges();

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
                .Include(b => b.Accionista)
                .Include(b => b.Moneda)
                .Where(b => b.DividendoDefinitivoId == id)
                .ToListAsync();
        }

        public async Task<int> CreateAsync(DividendoDefinitivo entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            await context.Set<DividendoDefinitivo>().AddAsync(entity);
            context.Entry(entity.Sociedad).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(entity.Sociedad).State = EntityState.Detached;

            // El ID ya fue poblado por EF Core aquí
            return entity.Id;
        }

        public async Task CreateDetalleAsync(List<DividendoDefinitivoDetalle> entities)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.Set<DividendoDefinitivoDetalle>().AddRangeAsync(entities);
            foreach (var entity in entities)
            {
                context.Entry(entity.Accionista).State = EntityState.Unchanged;
                context.Entry(entity.Moneda).State = EntityState.Unchanged;
            }   

            await context.SaveChangesAsync();
            foreach (var entity in entities)
            {
                context.Entry(entity.Accionista).State = EntityState.Detached;
                context.Entry(entity.Moneda).State = EntityState.Detached;
            }
        }
    }
}