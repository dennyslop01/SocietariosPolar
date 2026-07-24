using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class DocumentoModuloRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IDocumentoModulo
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<DocumentoModulo>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DocumentoModulo>()
                .Include(b => b.TipoDocumentoSoporte)
                .ToListAsync();
        }

        public async Task<DocumentoModulo?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DocumentoModulo>()
                .Include(b => b.TipoDocumentoSoporte)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<DocumentoModulo>> GetByModuloIdAsync(int moduloid, int referenciaid)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DocumentoModulo>()
                .Include(b => b.TipoDocumentoSoporte)
                .Where(x => x.TipoDocumentoSoporte!.ModuloId == moduloid && x.ReferenciaId == referenciaid)
                .ToListAsync();
        }

        public async Task AddAsync(DocumentoModuloDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var documento = await context.Set<TipoDocumentoSoporte>().FindAsync(entity.TipoDocumentoSoporteId);
            if (documento == null) throw new Exception($"Tipo Docuemnto Soporte con ID {entity.TipoDocumentoSoporteId} no existe.");

            DocumentoModulo? newDocumentoModulo = new()
            {
                TipoDocumentoSoporte = documento,
                ReferenciaId = entity.ReferenciaId,
                RutaGoogle = entity.RutaGoogle,
                NombreDocumento = entity.NombreDocumento,
                Comentarios = entity.Comentarios,
                CreateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
            };

            await context.Set<DocumentoModulo>().AddAsync(newDocumentoModulo);
            context.Entry(newDocumentoModulo.TipoDocumentoSoporte).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(newDocumentoModulo.TipoDocumentoSoporte).State = EntityState.Detached;
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<DocumentoModulo>().Find(id);
            if (entity != null)
            {
                context.Set<DocumentoModulo>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}