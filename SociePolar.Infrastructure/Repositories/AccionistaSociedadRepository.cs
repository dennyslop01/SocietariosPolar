using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class AccionistaSociedadRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IAccionistaSociedad
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<AccionistaSociedad>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<AccionistaSociedad>()
                .Include(b => b.Sociedad)
                .ThenInclude(s => s!.Empresa)
                .Include(b => b.Sociedad)
                .ThenInclude(s => s!.EstatusSociedad)
                .Include(b => b.Accionista)
                .ThenInclude(a => a!.TipoAccionista)
                .Include(b => b.EstatusAccionista)
                .ToListAsync();
        }

        public async Task<AccionistaSociedad?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<AccionistaSociedad>()
                .Include(b => b.Sociedad)
                .ThenInclude(s => s!.Empresa)
                .Include(b => b.Sociedad)
                .ThenInclude(s => s!.EstatusSociedad)
                .Include(b => b.Accionista)
                .ThenInclude(a => a!.TipoAccionista)
                .Include(b => b.EstatusAccionista)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<AccionistaSociedad>> GetBySociedadIdAsync(int sociedadId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<AccionistaSociedad>()
                .Include(b => b.Sociedad)
                .ThenInclude(s => s!.Empresa)
                .Include(b => b.Sociedad)
                .ThenInclude(s => s!.EstatusSociedad)
                .Include(b => b.Accionista)
                .ThenInclude(a => a!.TipoAccionista)
                .Include(b => b.EstatusAccionista)
                .Where(x => x.Sociedad!.Id == sociedadId)
                .ToListAsync();
        }

        public async Task AddAsync(AccionistaSociedadDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var accionista = await context.Set<Accionista>().FindAsync(entity.AccionistaId);
            if (accionista == null) throw new Exception($"Accionista con ID {entity.AccionistaId} no existe.");

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            var estatus = await context.Set<EstatusAccionista>().FindAsync(entity.EstatusAccionistaId);
            if (estatus == null) throw new Exception($"Estatus Accionista con ID {entity.EstatusAccionistaId} no existe.");

            AccionistaSociedad newEntidad = new()
            {
                Sociedad = sociedad,
                Accionista = accionista,
                EstatusAccionista = estatus,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
                UpdateUserId = entity.UpdateUserId
            };

            await context.Set<AccionistaSociedad>().AddAsync(newEntidad);
            context.Entry(newEntidad.EstatusAccionista).State = EntityState.Unchanged;
            context.Entry(newEntidad.Accionista).State = EntityState.Unchanged;
            context.Entry(newEntidad.Sociedad).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(newEntidad.EstatusAccionista).State = EntityState.Detached;
            context.Entry(newEntidad.Accionista).State = EntityState.Detached;
            context.Entry(newEntidad.Sociedad).State = EntityState.Detached;
        }

        public async void Update(AccionistaSociedadDto entity)
        {
            using var context = _contextFactory.CreateDbContext();

            var accionista = await context.Set<Accionista>().FindAsync(entity.AccionistaId);
            if (accionista == null) throw new Exception($"Accionista con ID {entity.AccionistaId} no existe.");

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            var estatus = await context.Set<EstatusAccionista>().FindAsync(entity.EstatusAccionistaId);
            if (estatus == null) throw new Exception($"Estatus Accionista con ID {entity.EstatusAccionistaId} no existe.");


            AccionistaSociedad? editentidad = await context.AccionistasSociedades.FindAsync(entity.Id);
            if (editentidad == null) throw new Exception($"Accionista - Sodiedad con ID {entity.Id} no existe.");

            editentidad.Sociedad = sociedad;
            editentidad.Accionista = accionista;
            editentidad.EstatusAccionista = estatus;
            editentidad.UpdateDate = DateTime.UtcNow;
            editentidad.UpdateUserId = entity.UpdateUserId;


            context.Set<AccionistaSociedad>().Update(editentidad);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<AccionistaSociedad>().Find(id);
            if (entity != null)
            {
                context.Set<AccionistaSociedad>().Remove(entity);
                context.SaveChanges();
            }
        }
        
        public async void Activar(int Id, int status)
        {
            using var context = _contextFactory.CreateDbContext();

            var estatusAccionista = await context.Set<EstatusAccionista>().FindAsync(status);
            if (estatusAccionista == null) throw new Exception($"Estatus de accionista con ID {status} no existe.");

            AccionistaSociedad? editaccionista = await context.AccionistasSociedades.FindAsync(Id);
            if (editaccionista == null) throw new Exception($"Accionista - Sociedad con ID {Id} no existe.");

            context.Set<AccionistaSociedad>().Update(editaccionista);
            context.SaveChanges();
        }
    }
}