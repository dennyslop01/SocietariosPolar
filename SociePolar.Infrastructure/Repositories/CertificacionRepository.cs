using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class CertificacionRepository(IDbContextFactory<SociedadDbContext> contextFactory) : ICertificacion
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<Certificacion>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Certificacion>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad!.Empresa)
                .Include(b => b.Sociedad!.EstatusSociedad)
                .Include(b => b.Cargo)
                .ToListAsync();
        }

        public async Task<Certificacion?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Certificacion>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad!.Empresa)
                .Include(b => b.Sociedad!.EstatusSociedad)
                .Include(b => b.Cargo)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Certificacion>> GetBySociedadIdAsync(int sociedadId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Certificacion>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad!.Empresa)
                .Include(b => b.Sociedad!.EstatusSociedad)
                .Include(b => b.Cargo)
                .Where(x => x.Sociedad!.Id == sociedadId)
                .ToListAsync();
        }

        public async Task AddAsync(CertificacionDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var cargo = await context.Set<Cargo>().FindAsync(entity.CargoId);
            if (cargo == null) throw new Exception($"Cargo con ID {entity.CargoId} no existe.");

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            Certificacion newCertificacion = new()
            {
                Cargo = cargo,
                Sociedad = sociedad,
                Fecha = entity.Fecha,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
                UpdateUserId = entity.UpdateUserId
            };

            await context.Set<Certificacion>().AddAsync(newCertificacion);
            context.Entry(newCertificacion.Cargo).State = EntityState.Unchanged;
            context.Entry(newCertificacion.Sociedad).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(newCertificacion.Cargo).State = EntityState.Detached;
            context.Entry(newCertificacion.Sociedad).State = EntityState.Detached;
        }

        public async void Update(CertificacionDto entity)
        {
            using var context = _contextFactory.CreateDbContext();

            var cargo = await context.Set<Cargo>().FindAsync(entity.CargoId);
            if (cargo == null) throw new Exception($"Cargo con ID {entity.CargoId} no existe.");

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            Certificacion? editCertificacion = await context.Certificaciones.FindAsync(entity.Id);
            if (editCertificacion == null) throw new Exception($"Certificacion con ID {entity.Id} no existe.");

            editCertificacion.Cargo = cargo;
            editCertificacion.Sociedad = sociedad;
            editCertificacion.Fecha = entity.Fecha;
            editCertificacion.UpdateDate = DateTime.UtcNow;
            editCertificacion.UpdateUserId = entity.UpdateUserId;


            context.Set<Certificacion>().Update(editCertificacion);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<Sociedad>().Find(id);
            if (entity != null)
            {
                context.Set<Sociedad>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}