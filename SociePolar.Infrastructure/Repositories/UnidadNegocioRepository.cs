using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class UnidadNegocioRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IUnidadNegocio
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<UnidadNegocio>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<UnidadNegocio>()
                .Include(b => b.Region)
                .ToListAsync();
        }

        public async Task<UnidadNegocio?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<UnidadNegocio>()
                .Include(b => b.Region)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task AddAsync(UnidadNegocio entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.Set<UnidadNegocio>().AddAsync(entity);
            context.Entry(entity.Region!).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(entity.Region!).State = EntityState.Detached;
        }

        public void Update(UnidadNegocio entity)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Set<UnidadNegocio>().Update(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<UnidadNegocio>().Find(id);
            if (entity != null)
            {
                context.Set<UnidadNegocio>().Remove(entity);
                context.SaveChanges();
            }
        }

        public async Task<List<UnidadNegocio>> GetByIdRegionAllAsync(int idregion)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<UnidadNegocio>()
                .Include(b => b.Region)
                .Where(x => x.Region!.Id == idregion)
                .ToListAsync();

        }
    }
}
