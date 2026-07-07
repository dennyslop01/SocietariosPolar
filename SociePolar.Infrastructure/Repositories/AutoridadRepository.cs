using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class AutoridadRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IAutoridad
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<Autoridad>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Autoridad>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.Sociedad.EstatusSociedad)
                .Include(b => b.TipoDocumento1)
                .Include(b => b.TipoDocumento2)
                .Include(b => b.Cargo)
                .ToListAsync();
        }

        public async Task<Autoridad?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Autoridad>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.Sociedad.EstatusSociedad)
                .Include(b => b.TipoDocumento1)
                .Include(b => b.TipoDocumento2)
                .Include(b => b.Cargo)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Autoridad>> GetBySociedadIdAsync(int sociedadId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Autoridad>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.Sociedad.EstatusSociedad)
                .Include(b => b.Cargo)
                .Include(b => b.TipoDocumento1)
                .Include(b => b.TipoDocumento2)
                .Where(x => x.Sociedad.Id == sociedadId)
                .ToListAsync();
        }

        public async Task AddAsync(AutoridadDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var cargo = await context.Set<Cargo>().FindAsync(entity.CargoId);
            if (cargo == null) throw new Exception($"Cargo con ID {entity.CargoId} no existe.");

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            TipoDocumento tipodoc1 = null;
            if (entity.TipoDocumento1Id != null)
            {
                if (entity.TipoDocumento1Id != 0)
                {
                    tipodoc1 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento1Id.Value);
                    if (tipodoc1 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento1Id.Value} no existe.");
                }
            }

            TipoDocumento tipodoc2 = null;
            if (entity.TipoDocumento2Id != null)
            {
                if (entity.TipoDocumento2Id != 0)
                {
                    tipodoc2 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento2Id.Value);
                    if (tipodoc2 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento2Id.Value} no existe.");
                }
            }

            Autoridad newAutoridad = new()
            {
                Cargo = cargo,
                Sociedad = sociedad,
                Nombre = entity.Nombre,
                Documento1 = entity.Documento1,
                Documento2 = entity.Documento2,
                TipoDocumento1 = tipodoc1,
                TipoDocumento2 = tipodoc2,
                Duracion = entity.Duracion,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
                UpdateUserId = entity.UpdateUserId
            };

            await context.Set<Autoridad>().AddAsync(newAutoridad);
            context.Entry(newAutoridad.Cargo).State = EntityState.Unchanged;
            context.Entry(newAutoridad.Sociedad).State = EntityState.Unchanged;
            if (tipodoc1 != null) context.Entry(newAutoridad.TipoDocumento1).State = EntityState.Unchanged;
            if (tipodoc2 != null) context.Entry(newAutoridad.TipoDocumento2).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(newAutoridad.Cargo).State = EntityState.Detached;
            context.Entry(newAutoridad.Sociedad).State = EntityState.Detached;
            if (tipodoc1 != null) context.Entry(newAutoridad.TipoDocumento1).State = EntityState.Detached;
            if (tipodoc2 != null) context.Entry(newAutoridad.TipoDocumento2).State = EntityState.Detached;
        }

        public async void Update(AutoridadDto entity)
        {
            using var context = _contextFactory.CreateDbContext();

            var cargo = await context.Set<Cargo>().FindAsync(entity.CargoId);
            if (cargo == null) throw new Exception($"Cargo con ID {entity.CargoId} no existe.");

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            Autoridad editautoridad = await context.Autoridades.FindAsync(entity.Id);
            if (editautoridad == null) throw new Exception($"Autoridad con ID {entity.Id} no existe.");

            TipoDocumento tipodoc1 = null;
            if (entity.TipoDocumento1Id != null)
            {
                if (entity.TipoDocumento1Id != 0)
                {
                    tipodoc1 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento1Id.Value);
                    if (tipodoc1 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento1Id.Value} no existe.");
                }
            }

            TipoDocumento tipodoc2 = null;
            if (entity.TipoDocumento2Id != null)
            {
                if (entity.TipoDocumento2Id != 0)
                {
                    tipodoc2 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento2Id.Value);
                    if (tipodoc2 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento2Id.Value} no existe.");
                }
            }

            editautoridad.Cargo = cargo;
            editautoridad.Sociedad = sociedad;
            editautoridad.Nombre = entity.Nombre;
            editautoridad.Documento1 = entity.Documento1;
            editautoridad.Documento2 = entity.Documento2;
            editautoridad.TipoDocumento1 = tipodoc1;
            editautoridad.TipoDocumento2 = tipodoc2;
            editautoridad.Duracion = entity.Duracion;
            editautoridad.UpdateDate = DateTime.UtcNow;
            editautoridad.UpdateUserId = entity.UpdateUserId;


            context.Set<Autoridad>().Update(editautoridad);
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