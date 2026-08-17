using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;
using System.Xml.Schema;

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
            context.Set<DividendoPreliminarDetalle>()
                   .Where(x => x.DividendoPreliminarId == id)
                   .ExecuteDelete();
            context.SaveChanges();

            var entity = context.Set<DividendoPreliminar>().Find(id);
            if (entity != null)
            {
                context.Set<DividendoPreliminar>().Remove(entity);
                context.SaveChanges();
            }
        }

        public async Task AddDetalleAsync(List<DividendoPreliminarDetalle> entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var idencabezado = await context.Set<DividendoPreliminar>().FindAsync(entity[0].DividendoPreliminarId);
            if (idencabezado == null)
                throw new Exception($"Dividendo Preliminar con ID {entity[0].DividendoPreliminarId} no existe.");

            await context.Set<DividendoPreliminarDetalle>().AddRangeAsync(entity);
           // context.Entry(newDividendoPreliminar.Sociedad).State = EntityState.Unchanged;

            await context.SaveChangesAsync();

            //context.Entry(newDividendoPreliminar.Sociedad).State = EntityState.Detached;
        }

        public async Task<List<DividendoPreliminarDetalle>?> GetDetalleByIdAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DividendoPreliminarDetalle>()
                .Where(b => b.DividendoPreliminarId == id)
                .ToListAsync();
        }

        public async Task<DividendoPreliminarDetalle?> GetDetalleByIdItemAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DividendoPreliminarDetalle>()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<int> UpdateDetalleAsync(DividendoPreliminarDetalle entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var idencabezado = await context.Set<DividendoPreliminar>().FindAsync(entity.DividendoPreliminarId);
            if (idencabezado == null)
                throw new Exception($"Dividendo Preliminar con ID {entity.DividendoPreliminarId} no existe.");

            var detallaux = await context.Set<DividendoPreliminarDetalle>().FindAsync(entity.Id);
            if (detallaux == null)
                throw new Exception($"Detalle de Dividendo Preliminar con ID {entity.Id} no existe.");

            detallaux.NombreAccionista = entity.NombreAccionista;
            detallaux.Rif = entity.Rif;
            detallaux.TipoPago = entity.TipoPago;
            detallaux.MonedaPago = entity.MonedaPago;
            detallaux.Notificado = entity.Notificado;
            detallaux.FechaNotificacion = entity.FechaNotificacion;
            detallaux.MontoDecretado = entity.MontoDecretado;
            detallaux.FechaDecreto = entity.FechaDecreto;
            detallaux.MontoRetenido = entity.MontoRetenido;
            detallaux.EjercicioFiscal = entity.EjercicioFiscal;
            detallaux.Porcion1Monto = entity.Porcion1Monto;
            detallaux.Porcion2Monto = entity.Porcion2Monto;
            detallaux.Porcion3Monto = entity.Porcion3Monto;
            detallaux.Porcion4Monto = entity.Porcion4Monto;
            detallaux.Porcion1Porcentaje = entity.Porcion1Porcentaje;
            detallaux.Porcion2Porcentaje = entity.Porcion2Porcentaje;
            detallaux.Porcion3Porcentaje = entity.Porcion3Porcentaje;
            detallaux.Porcion4Porcentaje = entity.Porcion4Porcentaje;
            detallaux.Porcion1FechaPago = entity.Porcion1FechaPago;
            detallaux.Porcion2FechaPago = entity.Porcion2FechaPago;
            detallaux.Porcion3FechaPago = entity.Porcion3FechaPago;
            detallaux.Porcion4FechaPago = entity.Porcion4FechaPago;
            detallaux.Observaciones = entity.Observaciones;
            detallaux.SoporteEnviadoP1 = entity.SoporteEnviadoP1;
            detallaux.SoporteEnviadoP2 = entity.SoporteEnviadoP2;
            detallaux.SoporteEnviadoP3 = entity.SoporteEnviadoP3;
            detallaux.SoporteEnviadoP4 = entity.SoporteEnviadoP4;
            detallaux.SoporteFechaP1 = entity.SoporteFechaP1;
            detallaux.SoporteFechaP2 = entity.SoporteFechaP2;
            detallaux.SoporteFechaP3 = entity.SoporteFechaP3;
            detallaux.SoporteFechaP4 = entity.SoporteFechaP4;

            context.Set<DividendoPreliminarDetalle>().Update(detallaux);
            context.SaveChanges();

            await context.SaveChangesAsync();

            return entity.Id;
        }
    }
}