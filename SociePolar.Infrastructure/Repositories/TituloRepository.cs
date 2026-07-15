using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class TituloRepository(IDbContextFactory<SociedadDbContext> contextFactory) : ITitulo
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<Titulo>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Titulo>()
                .Include(b => b.AccionistaSociedad)
                .Include(b => b.AccionistaSociedad!.Sociedad)
                .Include(b => b.AccionistaSociedad!.Sociedad!.Empresa)
                .Include(b => b.AccionistaSociedad!.Accionista)
                .ToListAsync();
        }

        public async Task<Titulo?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Titulo>()
                .Include(b => b.AccionistaSociedad)
                .Include(b => b.AccionistaSociedad!.Sociedad)
                .Include(b => b.AccionistaSociedad!.Sociedad!.Empresa)
                .Include(b => b.AccionistaSociedad!.Accionista)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Titulo>> GetByAccionistaSociedadIdAsync(int sociedadId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Titulo>()
                .Include(b => b.AccionistaSociedad)
                .Include(b => b.AccionistaSociedad!.Sociedad)
                .Include(b => b.AccionistaSociedad!.Sociedad!.Empresa)
                .Include(b => b.AccionistaSociedad!.Accionista)
                .Where(x => x.AccionistaSociedad!.Id == sociedadId)
                .ToListAsync();
        }

        public async Task AddAsync(TituloDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var accionista = await context.Set<AccionistaSociedad>().FindAsync(entity.AccionistaSociedadId);
            if (accionista == null) throw new Exception($"Accionista - Sociedad con ID {entity.AccionistaSociedadId} no existe.");

            Titulo newTitulo = new()
            {
                AccionistaSociedad = accionista,
                Numero = entity.Numero,
                Acciones = entity.Acciones,
                Preferente = entity.Preferente,
                Ubicacion = entity.Ubicacion,
                Anulado = entity.Anulado,
                Endosado = entity.Endosado,
                Fecha = entity.Fecha,
                Observaciones = entity.Observaciones,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
                UpdateUserId = entity.UpdateUserId
            };

            await context.Set<Titulo>().AddAsync(newTitulo);
            context.Entry(newTitulo.AccionistaSociedad).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(newTitulo.AccionistaSociedad).State = EntityState.Detached;
        }

        public async void Update(TituloDto entity)
        {
            using var context = _contextFactory.CreateDbContext();

            var accionista = await context.Set<AccionistaSociedad>().FindAsync(entity.AccionistaSociedadId);
            if (accionista == null) throw new Exception($"Accionista - Sociedad con ID {entity.AccionistaSociedadId} no existe.");

            var editTitulo = await context.Set<Titulo>().FindAsync(entity.Id);
            if (editTitulo == null) throw new Exception($"Titulo con ID {entity.Id} no existe.");

            editTitulo.AccionistaSociedad = accionista;
            editTitulo.Numero = entity.Numero;
            editTitulo.Acciones = entity.Acciones;
            editTitulo.Preferente = entity.Preferente;
            editTitulo.Ubicacion = entity.Ubicacion;
            editTitulo.Anulado = entity.Anulado;
            editTitulo.Endosado = entity.Endosado;
            editTitulo.Fecha = entity.Fecha;
            editTitulo.Observaciones = entity.Observaciones;
            editTitulo.UpdateDate = DateTime.UtcNow;
            editTitulo.UpdateUserId = entity.UpdateUserId;


            context.Set<Titulo>().Update(editTitulo);
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