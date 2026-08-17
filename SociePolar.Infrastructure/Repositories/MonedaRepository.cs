using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class MonedaRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IMoneda
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<Moneda>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Moneda>()
                .ToListAsync();
        }

        public async Task<Moneda?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Moneda>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Moneda entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.Set<Moneda>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Moneda entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var monedaaux = await context.Set<Moneda>().FindAsync(entity.Id);
            if (monedaaux == null)
                throw new Exception($"Moneda con ID {entity.Id} no existe.");

            monedaaux.Nombre = entity.Nombre;
            monedaaux.Simbolo = entity.Simbolo;
            monedaaux.UpdateDate = entity.UpdateDate;
            monedaaux.UpdateUserId = entity.UpdateUserId;

            context.Set<Moneda>().Update(monedaaux);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var entity = await context.Set<Moneda>().FindAsync(id);
            if (entity == null) throw new Exception($"Moneda con ID {id} no existe.");
            context.Set<Moneda>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
