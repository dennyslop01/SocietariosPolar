using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class DividendoPreliminarRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IDividendoPreliminar
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<DividendoPreliminar>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DividendoPreliminar>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .ToListAsync();
        }

        public async Task<DividendoPreliminar?> GetByIdAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DividendoPreliminar>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Int32> AddAsync(DividendoPreliminarDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null)
                throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            DividendoPreliminar newDividendoPreliminar = new()
            {
                Sociedad = sociedad,
                Explicacion = entity.Explicacion,
                NombreDividendos = entity.NombreDividendos,
                RutaDividendos = entity.RutaDividendos,
                NombreActa = entity.NombreActa,
                RutaActa = entity.RutaActa,
                NombreDocumento = entity.NombreDocumento,
                RutaDocumento = entity.RutaDocumento,
                Observaciones = entity.Observaciones,
                MontoPagadoTesoreria = entity.MontoPagadoTesoreria,
                MontoPagadoAccionistas = entity.MontoPagadoAccionistas,
                CreateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
            };

            await context.Set<DividendoPreliminar>().AddAsync(newDividendoPreliminar);
            context.Entry(newDividendoPreliminar.Sociedad).State = EntityState.Unchanged;

            await context.SaveChangesAsync();

            context.Entry(newDividendoPreliminar.Sociedad).State = EntityState.Detached;

            // El ID ya fue poblado por EF Core aquí
            return newDividendoPreliminar.Id;
        }


        public void Delete(Int32 id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<DividendoPreliminar>().Find(id);
            if (entity != null)
            {
                context.Set<DividendoPreliminar>().Remove(entity);
                context.SaveChanges();
            }
        }

        public async Task AddDetalleAsync(List<DividendoDetalleModel> entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var idencabezado = await context.Set<DividendoPreliminar>().FindAsync(entity[0].DividendoPreliminarId);
            if (idencabezado == null)
                throw new Exception($"Dividendo Preliminar con ID {entity[0].DividendoPreliminarId} no existe.");

            await context.Set<DividendoDetalleModel>().AddRangeAsync(entity);
           // context.Entry(newDividendoPreliminar.Sociedad).State = EntityState.Unchanged;

            await context.SaveChangesAsync();

            //context.Entry(newDividendoPreliminar.Sociedad).State = EntityState.Detached;
        }
    }
}
