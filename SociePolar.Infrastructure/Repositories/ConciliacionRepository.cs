using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class ConciliacionRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IConciliacion
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<Conciliacion>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Conciliacion>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .ToListAsync();
        }

        public async Task<Conciliacion?> GetByIdAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Conciliacion>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Int32> AddAsync(ConciliacionDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null)
                throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            Conciliacion newConciliacion = new()
            {
                Sociedad = sociedad,
                TipoArchivo = entity.TipoArchivo,
                NombreConciliaciones = entity.NombreConciliaciones,
                RutaConciliaciones = entity.RutaConciliaciones,
                Observaciones = entity.Observaciones,
                CreateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
            };

            await context.Set<Conciliacion>().AddAsync(newConciliacion);
            context.Entry(newConciliacion.Sociedad).State = EntityState.Unchanged;

            await context.SaveChangesAsync();

            context.Entry(newConciliacion.Sociedad).State = EntityState.Detached;

            return newConciliacion.Id;
        }

        public async Task AddDetalleAsync(List<ConciliacionDetalle> entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var idencabezado = await context.Set<Conciliacion>().FindAsync(entity[0].ConciliacionId);
            if (idencabezado == null)
                throw new Exception($"Conciliacion con ID {entity[0].ConciliacionId} no existe.");

            await context.Set<ConciliacionDetalle>().AddRangeAsync(entity);

            await context.SaveChangesAsync();
        }

        public async Task<List<ConciliacionDetalle>?> GetDetalleByIdAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<ConciliacionDetalle>()
                .Where(b => b.ConciliacionId == id)
                .ToListAsync();
        }

        public async Task<ConciliacionDetalle?> GetDetalleByIdItemAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<ConciliacionDetalle>()
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}