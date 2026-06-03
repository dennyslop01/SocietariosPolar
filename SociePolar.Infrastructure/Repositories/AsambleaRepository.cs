using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class AsambleaRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IAsamblea
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<Asamblea>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Asamblea>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.TipoAsamblea)
                .Include(b => b.Registro)
                .Include(b => b.NombreDiario)
                .Include(b => b.TipoReforma)
                .ToListAsync();
        }

        public async Task<Asamblea?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Asamblea>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.TipoAsamblea)
                .Include(b => b.Registro)
                .Include(b => b.NombreDiario)
                .Include(b => b.TipoReforma)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Asamblea>> GetBySociedadIdAsync(int sociedadId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Asamblea>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.TipoAsamblea)
                .Include(b => b.Registro)
                .Include(b => b.NombreDiario)
                .Include(b => b.TipoReforma)
                .Where(x => x.Sociedad.Id == sociedadId)
                .ToListAsync();
        }

        public async Task AddAsync(AsambleaDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            var tipoAsamblea = await context.Set<TipoAsamblea>().FindAsync(entity.TipoAsambleaId);
            if (tipoAsamblea == null) throw new Exception($"TipoAsamblea con ID {entity.TipoAsambleaId} no existe.");

            var registro = await context.Set<Registro>().FindAsync(entity.RegistroId);
            if (registro == null) throw new Exception($"Registro con ID {entity.RegistroId} no existe.");

            var nombreDiario = await context.Set<NombreDiario>().FindAsync(entity.NombreDiarioId);
            if (nombreDiario == null) throw new Exception($"NombreDiario con ID {entity.NombreDiarioId} no existe.");


            TipoReforma tipoReforma = new();
            if (entity.AplicaReforma == 1)
            {
                tipoReforma = await context.Set<TipoReforma>().FindAsync(entity.TipoReformaId);
                if (tipoReforma == null) throw new Exception($"TipoReforma con ID {entity.TipoReformaId} no existe.");
            }

            Asamblea newAsamblea = new()
            {
                Sociedad = sociedad,
                TipoAsamblea = tipoAsamblea,
                FechaCelebracion = entity.FechaCelebracion.Value,
                NumeroActa = entity.NumeroActa.Value,
                FechaRegistro = entity.FechaRegistro.Value,
                Registro = registro,
                NumeroRegistro = entity.NumeroRegistro.Value,
                Tomo = entity.Tomo,
                AnoPublicacion = entity.AnoPublicacion.Value,
                NumeroPublicacion = entity.NumeroPublicacion.Value,
                FechaPublicacion = entity.FechaPublicacion.Value,
                NombreDiario = nombreDiario,
                IndicadorAsamblea = 1,
                AplicaReforma = entity.AplicaReforma.Value,
                TipoReforma = tipoReforma,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
                UpdateUserId = entity.UpdateUserId
            };

            await context.Set<Asamblea>().AddAsync(newAsamblea);
            context.Entry(newAsamblea.TipoAsamblea).State = EntityState.Unchanged;
            context.Entry(newAsamblea.Registro).State = EntityState.Unchanged;
            context.Entry(newAsamblea.NombreDiario).State = EntityState.Unchanged;
            context.Entry(newAsamblea.TipoReforma).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(newAsamblea.TipoAsamblea).State = EntityState.Detached;
            context.Entry(newAsamblea.Registro).State = EntityState.Detached;
            context.Entry(newAsamblea.NombreDiario).State = EntityState.Detached;
            context.Entry(newAsamblea.TipoReforma).State = EntityState.Detached;
        }

        public async void Update(AsambleaDto entity)
        {
            using var context = _contextFactory.CreateDbContext();

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            var tipoAsamblea = await context.Set<TipoAsamblea>().FindAsync(entity.TipoAsambleaId);
            if (tipoAsamblea == null) throw new Exception($"TipoAsamblea con ID {entity.TipoAsambleaId} no existe.");

            var registro = await context.Set<Registro>().FindAsync(entity.RegistroId);
            if (registro == null) throw new Exception($"Registro con ID {entity.RegistroId} no existe.");

            var nombreDiario = await context.Set<NombreDiario>().FindAsync(entity.NombreDiarioId);
            if (nombreDiario == null) throw new Exception($"NombreDiario con ID {entity.NombreDiarioId} no existe.");

            TipoReforma tipoReforma = new();
            if (entity.AplicaReforma == 1)
            {
                tipoReforma = await context.Set<TipoReforma>().FindAsync(entity.TipoReformaId);
                if (tipoReforma == null) throw new Exception($"TipoReforma con ID {entity.TipoReformaId} no existe.");
            }

            Asamblea editAsamblea = await context.Asambleas.FindAsync(entity.Id);
            if (editAsamblea == null) throw new Exception($"Asamblea con ID {entity.Id} no existe.");

            editAsamblea.Sociedad = sociedad;
            editAsamblea.TipoAsamblea = tipoAsamblea;
            editAsamblea.FechaCelebracion = entity.FechaCelebracion.Value;
            editAsamblea.NumeroActa = entity.NumeroActa.Value;
            editAsamblea.FechaRegistro = entity.FechaRegistro.Value;
            editAsamblea.Registro = registro;
            editAsamblea.NumeroRegistro = entity.NumeroRegistro.Value;
            editAsamblea.Tomo = entity.Tomo;
            editAsamblea.AnoPublicacion = entity.AnoPublicacion.Value;
            editAsamblea.NumeroPublicacion = entity.NumeroPublicacion.Value;
            editAsamblea.FechaPublicacion = entity.FechaPublicacion.Value;
            editAsamblea.NombreDiario = nombreDiario;
            editAsamblea.AplicaReforma =  entity.AplicaReforma.Value;
            editAsamblea.TipoReforma = tipoReforma;
            editAsamblea.UpdateDate = DateTime.UtcNow;
            editAsamblea.UpdateUserId = entity.UpdateUserId;


            context.Set<Asamblea>().Update(editAsamblea);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<Asamblea>().Find(id);
            if (entity != null)
            {
                context.Set<Asamblea>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}