using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
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
    }
}
