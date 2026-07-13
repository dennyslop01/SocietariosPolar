using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class TipoDocumentoSoporteRepository(IDbContextFactory<SociedadDbContext> contextFactory) : ITipoDocumentoSoporte
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<TipoDocumentoSoporte>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<TipoDocumentoSoporte>()
                .ToListAsync();
        }

        public async Task<TipoDocumentoSoporte?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<TipoDocumentoSoporte>()
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(TipoDocumentoSoporte entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            TipoDocumentoSoporte newTipoDocumentoSoporte = new()
            {
                ModuloId = entity.ModuloId,
                Nombre = entity.Nombre,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
                UpdateUserId = entity.UpdateUserId
            };

            await context.Set<TipoDocumentoSoporte>().AddAsync(newTipoDocumentoSoporte);

            await context.SaveChangesAsync();
        }


        public async void Update(TipoDocumentoSoporte entity)
        {
            using var context = _contextFactory.CreateDbContext();
            TipoDocumentoSoporte editTipoDoc = await context.TiposDocumentosSoporte.FindAsync(entity.Id);
            if (editTipoDoc == null) throw new Exception($"TipoDocumentoSoporte con ID {entity.Id} no existe.");

            editTipoDoc.ModuloId = entity.ModuloId;
            editTipoDoc.Nombre = entity.Nombre;
            editTipoDoc.UpdateDate = DateTime.UtcNow;
            editTipoDoc.UpdateUserId = entity.UpdateUserId;

            context.Set<TipoDocumentoSoporte>().Update(editTipoDoc);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<TipoDocumentoSoporte>().Find(id);
            if (entity != null)
            {
                context.Set<TipoDocumentoSoporte>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}