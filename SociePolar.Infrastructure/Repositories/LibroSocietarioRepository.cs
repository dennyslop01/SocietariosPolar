using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class LibroSocietarioRepository(IDbContextFactory<SociedadDbContext> contextFactory) : ILibroSocietario
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<LibroSocietario>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<LibroSocietario>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.Sociedad.EstatusSociedad)
                .Include(b => b.ClaseLibro)
                .Include(b => b.TipoLibro)
                .ToListAsync();
        }

        public async Task<LibroSocietario?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<LibroSocietario>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.Sociedad.EstatusSociedad)
                .Include(b => b.ClaseLibro)
                .Include(b => b.TipoLibro)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<LibroSocietario>> GetBySociedadIdAsync(int sociedadId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<LibroSocietario>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.Sociedad.EstatusSociedad)
                .Include(b => b.ClaseLibro)
                .Include(b => b.TipoLibro)
                .Where(x => x.Sociedad.Id == sociedadId)
                .ToListAsync();
        }

        public async Task AddAsync(LibroSocietarioDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            var claseLibro = await context.Set<ClaseLibro>().FindAsync(entity.ClaseLibroId);
            if (claseLibro == null) throw new Exception($"ClaseLibro con ID {entity.ClaseLibroId} no existe.");

            var tipoLibro = await context.Set<TipoLibro>().FindAsync(entity.TipoLibroId);
            if (tipoLibro == null) throw new Exception($"TipoLibro con ID {entity.TipoLibroId} no existe.");

            LibroSocietario newLibroSocietario = new()
            {
                Sociedad = sociedad,
                ClaseLibro = claseLibro,
                TipoLibro = tipoLibro,
                TomoUso = entity.TomoUso,
                Folios = entity.Folios,
                LibrosSellados = entity.LibrosSellados.ToString(),
                Observaciones = entity.Observaciones,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
                UpdateUserId = entity.UpdateUserId,
                FechaDesde = entity.FechaDesde,
                FechaHasta = entity.FechaHasta,
                FechaSello = entity.FechaSello,
                Vacio = entity.Vacio,
                Ubicacion = entity.Ubicacion
            };

            await context.Set<LibroSocietario>().AddAsync(newLibroSocietario);

            // Cambiar estados a Unchanged para evitar duplicados en tablas maestras
            context.Entry(newLibroSocietario.Sociedad).State = EntityState.Unchanged;
            context.Entry(newLibroSocietario.ClaseLibro).State = EntityState.Unchanged;
            context.Entry(newLibroSocietario.TipoLibro).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(newLibroSocietario.Sociedad).State = EntityState.Detached;
            context.Entry(newLibroSocietario.ClaseLibro).State = EntityState.Detached;
            context.Entry(newLibroSocietario.TipoLibro).State = EntityState.Detached;
        }


        public async void Update(LibroSocietarioDto entity)
        {
            using var context = _contextFactory.CreateDbContext();

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            var claseLibro = await context.Set<ClaseLibro>().FindAsync(entity.ClaseLibroId);
            if (claseLibro == null) throw new Exception($"ClaseLibro con ID {entity.ClaseLibroId} no existe.");

            var tipoLibro = await context.Set<TipoLibro>().FindAsync(entity.TipoLibroId);
            if (tipoLibro == null) throw new Exception($"TipoLibro con ID {entity.TipoLibroId} no existe.");

            LibroSocietario editLibroSocietario = await context.LibrosSocietarios.FindAsync(entity.Id);
            if (editLibroSocietario == null) throw new Exception($"LibroSocietario con ID {entity.Id} no existe.");

            editLibroSocietario.Sociedad = sociedad;
            editLibroSocietario.ClaseLibro = claseLibro;
            editLibroSocietario.TipoLibro = tipoLibro;
            editLibroSocietario.TomoUso = entity.TomoUso;
            editLibroSocietario.Folios = entity.Folios;
            editLibroSocietario.LibrosSellados = entity.LibrosSellados.ToString();
            editLibroSocietario.Ubicacion = entity.Ubicacion;
            editLibroSocietario.UpdateDate = DateTime.UtcNow;
            editLibroSocietario.UpdateUserId = entity.UpdateUserId;
            editLibroSocietario.FechaDesde = entity.FechaDesde;
            editLibroSocietario.FechaHasta = entity.FechaHasta;
            editLibroSocietario.FechaSello = entity.FechaSello;
            editLibroSocietario.Observaciones = entity.Observaciones;
            editLibroSocietario.Vacio = entity.Vacio;

            context.Set<LibroSocietario>().Update(editLibroSocietario);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<LibroSocietario>().Find(id);
            if (entity != null)
            {
                context.Set<LibroSocietario>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}