using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class ConciliacionRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IConciliacion
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<Conciliacion>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Conciliacion>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .ToListAsync();
        }

        public async Task<Conciliacion?> GetByIdAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Conciliacion>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Int32> AddAsync(ConciliacionDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null)
                throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            Conciliacion newConciliacion = new()
            {
                Sociedad = sociedad,
                TipoArchivo = entity.TipoArchivo,
                NombreConciliaciones = entity.NombreConciliaciones,
                RutaConciliaciones = entity.RutaConciliaciones,
                Observaciones = entity.Observaciones,
                CreateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
            };

            await context.Set<Conciliacion>().AddAsync(newConciliacion);
            context.Entry(newConciliacion.Sociedad).State = EntityState.Unchanged;

            await context.SaveChangesAsync();

            context.Entry(newConciliacion.Sociedad).State = EntityState.Detached;

            int conciliacionId = newConciliacion.Id;
            context.Database.SetCommandTimeout(300);

            var paramSociedadId = new SqlParameter("@SociedadId", entity.SociedadId);
            var paramUserId = new SqlParameter("@UserId", entity.CreateUserId);
            await context.Database.ExecuteSqlRawAsync("EXEC SP_AccionistasSociedadHistorico @SociedadId, @UserId", paramSociedadId, paramUserId);

            return conciliacionId;
        }

        public async Task AddDetalleAsync(List<ConciliacionDetalle> entity)
        {
            if (entity == null || entity.Count == 0) return;

            using var context = await _contextFactory.CreateDbContextAsync();
            context.Database.SetCommandTimeout(300);

            var idencabezado = await context.Set<Conciliacion>().FindAsync(entity[0].ConciliacionId);
            if (idencabezado == null)
                throw new Exception($"Conciliacion con ID {entity[0].ConciliacionId} no existe.");

            const int batchSize = 1000;
            for (int i = 0; i < entity.Count; i += batchSize)
            {
                var batch = entity.Skip(i).Take(batchSize).ToList();
                await context.Set<ConciliacionDetalle>().AddRangeAsync(batch);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();
            }
        }

        public async Task<List<ConciliacionDetalle>?> GetDetalleByIdAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<ConciliacionDetalle>()
                .Where(b => b.ConciliacionId == id)
                .ToListAsync();
        }

        public async Task<ConciliacionDetalle?> GetDetalleByIdItemAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<ConciliacionDetalle>()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Sociedad>> GetSociedadesConConciliacionesAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var sociedadIds = await context.Set<Conciliacion>()
                .AsNoTracking()
                .Where(c => c.Sociedad != null)
                .Select(c => c.Sociedad!.Id)
                .Distinct()
                .ToListAsync();

            return await context.Set<Sociedad>()
                .AsNoTracking()
                .Include(s => s.Empresa)
                .Where(s => sociedadIds.Contains(s.Id) && s.Empresa != null)
                .OrderBy(s => s.Empresa!.Nombre)
                .ToListAsync();
        }

        public async Task<List<ConciliacionListadoReporteDto>> GetConciliacionesReporteAsync(int sociedadId, DateTime fechaDesde, DateTime fechaHasta)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var fechaInicio = fechaDesde.Date;
            var fechaLimite = fechaHasta.Date.AddDays(1);

            // 1. Obtener las conciliaciones en el rango de fechas para la sociedad indicada
            var conciliaciones = await context.Set<Conciliacion>()
                .AsNoTracking()
                .Include(c => c.Sociedad)
                    .ThenInclude(s => s!.Empresa)
                .Where(c => c.Sociedad!.Id == sociedadId &&
                       (c.FechaArchivo != null
                           ? (c.FechaArchivo >= fechaInicio && c.FechaArchivo < fechaLimite)
                           : (c.CreateDate >= fechaInicio && c.CreateDate < fechaLimite)))
                .OrderBy(c => c.FechaArchivo ?? c.CreateDate)
                .ToListAsync();

            if (!conciliaciones.Any())
            {
                return new List<ConciliacionListadoReporteDto>();
            }

            var dictConciliaciones = conciliaciones.ToDictionary(c => c.Id);
            var conciliacionIds = conciliaciones.Select(c => c.Id).ToList();

            // 2. Obtener los detalles de las conciliaciones
            var detalles = await context.Set<ConciliacionDetalle>()
                .AsNoTracking()
                .Where(cd => conciliacionIds.Contains(cd.ConciliacionId))
                .ToListAsync();

            if (!detalles.Any())
            {
                return new List<ConciliacionListadoReporteDto>();
            }

            var accionistaIds = detalles.Select(d => d.AccionistaId).Distinct().ToList();

            // 3. Obtener datos de los accionistas
            var accionistas = await context.Set<Accionista>()
                .AsNoTracking()
                .Include(a => a.TipoDocumento1)
                .Where(a => accionistaIds.Contains(a.Id))
                .ToListAsync();

            var dictAccionistas = accionistas
                .GroupBy(a => a.Id)
                .ToDictionary(g => g.Key, g => g.First());

            // 4. Obtener histórico de acciones previas (AccionistasSociedadesHistoricoNroAcciones)
            var historicos = await context.Set<AccionistaSociedadHistorico>()
                .AsNoTracking()
                .Where(h => h.Sociedad!.Id == sociedadId && accionistaIds.Contains(h.Accionista!.Id))
                .OrderByDescending(h => h.CreateDate)
                .ToListAsync();

            // 5. Obtener saldos de la tabla actual (AccionistasSociedades)
            var actuales = await context.Set<AccionistaSociedad>()
                .AsNoTracking()
                .Where(a => a.Sociedad!.Id == sociedadId && accionistaIds.Contains(a.Accionista!.Id))
                .ToListAsync();

            var dictActuales = actuales
                .Where(a => a.Accionista != null)
                .GroupBy(a => a.Accionista!.Id)
                .ToDictionary(g => g.Key, g => g.First());

            // 6. Consolidar el movimiento de cada registro de conciliación
            var listaResultado = new List<ConciliacionListadoReporteDto>();

            foreach (var cd in detalles)
            {
                dictConciliaciones.TryGetValue(cd.ConciliacionId, out var conc);
                var fechaMovimiento = conc?.FechaArchivo ?? conc?.CreateDate ?? cd.CreateDate;
                var nombreArchivo = conc?.NombreConciliaciones ?? "Sin Archivo";

                dictAccionistas.TryGetValue(cd.AccionistaId, out var acc);
                string tipoDoc = acc?.TipoDocumento1?.Nombre ?? "";
                string nroDoc = acc?.Documento1 ?? "";
                string nombreAcc = acc?.Nombre ?? "Sin Nombre";
                string accion = string.IsNullOrWhiteSpace(cd.Accion) ? "Actualizado" : cd.Accion;
                long totalAcciones = cd.TotalAcciones;

                long saldoAnterior = 0;
                long saldoNuevo = 0;

                if (accion.Equals("Creado", StringComparison.OrdinalIgnoreCase))
                {
                    saldoAnterior = 0;
                    saldoNuevo = totalAcciones;
                }
                else if (accion.Equals("Inactivado", StringComparison.OrdinalIgnoreCase))
                {
                    saldoNuevo = 0;
                    // Buscar en el histórico snapshot previo a la conciliación
                    var hist = historicos.FirstOrDefault(h => h.Accionista?.Id == cd.AccionistaId && h.CreateDate <= fechaMovimiento);
                    if (hist != null && hist.NroAcciones.HasValue && hist.NroAcciones.Value > 0)
                    {
                        saldoAnterior = hist.NroAcciones.Value;
                    }
                    else if (totalAcciones > 0)
                    {
                        saldoAnterior = totalAcciones;
                    }
                    else if (dictActuales.TryGetValue(cd.AccionistaId, out var act) && act.NroAcciones.HasValue)
                    {
                        saldoAnterior = act.NroAcciones.Value;
                    }
                }
                else // Actualizado
                {
                    saldoNuevo = totalAcciones;
                    var hist = historicos.FirstOrDefault(h => h.Accionista?.Id == cd.AccionistaId && h.CreateDate <= fechaMovimiento);
                    if (hist != null && hist.NroAcciones.HasValue)
                    {
                        saldoAnterior = hist.NroAcciones.Value;
                    }
                    else if (dictActuales.TryGetValue(cd.AccionistaId, out var act) && act.NroAcciones.HasValue)
                    {
                        saldoAnterior = act.NroAcciones.Value;
                    }
                    else
                    {
                        saldoAnterior = 0;
                    }
                }

                long variacion = saldoNuevo - saldoAnterior;

                listaResultado.Add(new ConciliacionListadoReporteDto
                {
                    ConciliacionId = cd.ConciliacionId,
                    NombreArchivo = nombreArchivo,
                    FechaConciliacion = fechaMovimiento,
                    Accion = accion,
                    TipoDocumento = tipoDoc,
                    CedulaRif = nroDoc,
                    NombreAccionista = nombreAcc,
                    SaldoAnterior = saldoAnterior,
                    SaldoNuevo = saldoNuevo,
                    Variacion = variacion
                });
            }

            // Ordenar por Fecha, luego por Nombre de Accionista
            var ordenados = listaResultado
                .OrderBy(x => x.FechaConciliacion)
                .ThenBy(x => x.NombreAccionista)
                .ToList();

            for (int i = 0; i < ordenados.Count; i++)
            {
                ordenados[i].Numero = i + 1;
            }

            return ordenados;
        }
    }
}