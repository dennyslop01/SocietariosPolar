using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class DividendoDefinitivoRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IDividendoDefinitivo
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<DividendoDefinitivo>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DividendoDefinitivo>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .ToListAsync();
        }

        public async Task<DividendoDefinitivo?> GetByIdAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DividendoDefinitivo>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public void Delete(Int32 id)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Set<DividendoDefinitivoDetalle>()
                  .Where(x => x.DividendoDefinitivoId == id)
                  .ExecuteDelete();
            context.SaveChanges();

            var entity = context.Set<DividendoDefinitivo>().Find(id);
            if (entity != null)
            {
                context.Set<DividendoDefinitivo>().Remove(entity);
                context.SaveChanges();
            }
        }

        public async Task<List<DividendoDefinitivoDetalle>?> GetDetalleByIdAsync(Int32 id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<DividendoDefinitivoDetalle>()
                .Include(b => b.Accionista)
                .Include(b => b.Moneda)
                .Where(b => b.DividendoDefinitivoId == id)
                .ToListAsync();
        }

        public async Task<int> CreateAsync(DividendoDefinitivo entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            await context.Set<DividendoDefinitivo>().AddAsync(entity);
            context.Entry(entity.Sociedad).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(entity.Sociedad).State = EntityState.Detached;

            // El ID ya fue poblado por EF Core aquí
            return entity.Id;
        }

        public async Task CreateDetalleAsync(List<DividendoDefinitivoDetalle> entities)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            await context.Set<DividendoDefinitivoDetalle>().AddRangeAsync(entities);
            foreach (var entity in entities)
            {
                context.Entry(entity.Accionista).State = EntityState.Unchanged;
                context.Entry(entity.Moneda).State = EntityState.Unchanged;
            }   

            await context.SaveChangesAsync();
            foreach (var entity in entities)
            {
                context.Entry(entity.Accionista).State = EntityState.Detached;
                context.Entry(entity.Moneda).State = EntityState.Detached;
            }
        }

        public async Task<List<Sociedad>> GetSociedadesConDividendosDefinitivosAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var sociedades = await context.Set<DividendoDefinitivo>()
                .AsNoTracking()
                .Include(d => d.Sociedad)
                    .ThenInclude(s => s!.Empresa)
                .Where(d => d.Sociedad != null && d.Sociedad.Empresa != null)
                .Select(d => d.Sociedad!)
                .ToListAsync();

            return sociedades
                .GroupBy(s => s.Id)
                .Select(g => g.First())
                .OrderBy(s => s.Empresa?.Nombre ?? "")
                .ToList();
        }

        public async Task<List<DividendoListadoReporteDto>> GetDividendosReporteAsync(int sociedadId, DateTime fechaDesde, DateTime fechaHasta)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            DateTime fechaInicio = fechaDesde.Date;
            DateTime fechaFinLimite = fechaHasta.Date.AddDays(1);

            // Buscar procesos de dividendos definitivos por fecha de creación o por fecha de decreto en el rango
            var procesosDirectos = await context.Set<DividendoDefinitivo>()
                .AsNoTracking()
                .Include(d => d.Sociedad)
                    .ThenInclude(s => s!.Empresa)
                .Include(d => d.Moneda)
                .Where(d => d.Sociedad != null && d.Sociedad.Id == sociedadId &&
                            d.CreateDate >= fechaInicio && d.CreateDate < fechaFinLimite)
                .ToListAsync();

            var procesosPorDetalleIds = await context.Set<DividendoDefinitivoDetalle>()
                .AsNoTracking()
                .Where(det => det.FechaDecreto.HasValue &&
                              det.FechaDecreto.Value >= fechaInicio && det.FechaDecreto.Value < fechaFinLimite)
                .Select(det => det.DividendoDefinitivoId)
                .Distinct()
                .ToListAsync();

            List<DividendoDefinitivo> procesosPorDetalle = new();
            if (procesosPorDetalleIds.Any())
            {
                procesosPorDetalle = await context.Set<DividendoDefinitivo>()
                    .AsNoTracking()
                    .Include(d => d.Sociedad)
                        .ThenInclude(s => s!.Empresa)
                    .Include(d => d.Moneda)
                    .Where(d => d.Sociedad != null && d.Sociedad.Id == sociedadId && procesosPorDetalleIds.Contains(d.Id))
                    .ToListAsync();
            }

            var todosProcesos = procesosDirectos
                .Concat(procesosPorDetalle)
                .GroupBy(p => p.Id)
                .Select(g => g.First())
                .ToList();

            if (!todosProcesos.Any()) return new List<DividendoListadoReporteDto>();

            var idsProcesos = todosProcesos.Select(p => p.Id).ToList();
            var procesosDict = todosProcesos.ToDictionary(p => p.Id);

            // Cargar detalles de dividendos para esos procesos
            var detalles = await context.Set<DividendoDefinitivoDetalle>()
                .AsNoTracking()
                .Include(det => det.Accionista)
                    .ThenInclude(a => a!.TipoDocumento1)
                .Include(det => det.Accionista)
                    .ThenInclude(a => a!.TipoAccionista)
                .Include(det => det.Accionista)
                    .ThenInclude(a => a!.Banco)
                .Include(det => det.Accionista)
                    .ThenInclude(a => a!.TipoCuenta)
                .Include(det => det.Moneda)
                .Where(det => idsProcesos.Contains(det.DividendoDefinitivoId))
                .ToListAsync();

            // Cargar cantidad de acciones de los accionistas para esa sociedad
            var accionistaIds = detalles.Where(d => d.Accionista != null).Select(d => d.Accionista!.Id).Distinct().ToList();
            var dictAcciones = await context.Set<AccionistaSociedad>()
                .AsNoTracking()
                .Where(x => x.Sociedad!.Id == sociedadId && accionistaIds.Contains(x.Accionista!.Id) && x.CustodioId == null)
                .GroupBy(x => x.Accionista!.Id)
                .ToDictionaryAsync(g => g.Key, g => g.Sum(s => s.NroAcciones ?? 0));

            var resultado = new List<DividendoListadoReporteDto>();

            foreach (var det in detalles)
            {
                procesosDict.TryGetValue(det.DividendoDefinitivoId, out var proc);
                var acc = det.Accionista;

                string cedulaRif = acc != null ? FormatearCedulaRifReporte(acc.TipoDocumento1?.Nombre, acc.Documento1, acc.TipoAccionista?.Id) : string.Empty;

                string nombreAccionista = string.Empty;
                if (acc != null)
                {
                    nombreAccionista = (acc.TipoAccionista?.Id == 3 && !string.IsNullOrWhiteSpace(acc.NombreSusesion))
                        ? acc.NombreSusesion.Trim()
                        : (acc.Nombre?.Trim() ?? string.Empty);

                    if (acc.Difunto == 1)
                    {
                        nombreAccionista += acc.FechaDefuncion.HasValue
                            ? $" (DIFUNTO {acc.FechaDefuncion.Value:MM/yyyy})"
                            : " (DIFUNTO)";
                    }
                }

                string moneda = det.Moneda?.Simbolo ?? proc?.Moneda?.Simbolo ?? "Bs.";
                string monedaPago = det.Moneda?.Nombre ?? det.Moneda?.Simbolo ?? proc?.Moneda?.Nombre ?? proc?.Moneda?.Simbolo ?? "Bolívares";

                dictAcciones.TryGetValue(acc?.Id ?? 0, out long cantAcciones);

                resultado.Add(new DividendoListadoReporteDto
                {
                    DividendoDefinitivoId = det.DividendoDefinitivoId,
                    ProcesoNombre = proc?.NombreDividendos ?? proc?.Explicacion ?? $"Proceso {det.DividendoDefinitivoId}",
                    SociedadNombre = proc?.Sociedad?.Empresa?.Nombre ?? "",
                    AccionistaId = acc?.Id ?? 0,
                    TipoAccionistaId = acc?.TipoAccionista?.Id,
                    CedulaRif = cedulaRif,
                    NombreAccionista = nombreAccionista,
                    CantidadAcciones = cantAcciones,
                    Correos = acc?.EmailContacto ?? "",
                    Telefonos = acc?.TelefonoContacto ?? "",
                    Banco = acc?.Banco?.Nombre ?? "",
                    TipoCuenta = acc?.TipoCuenta?.Nombre ?? "",
                    NroCuenta = acc?.NumeroCuenta ?? "",
                    TitularCuenta = acc?.NombreTitularCuenta ?? "",
                    TipoPago = det.TipoPago ?? "",
                    MonedaPago = monedaPago,
                    Notificado = det.Notificado,
                    FechaNotificacion = det.FechaNotificacion,
                    MontoDecretado = det.MontoDecretado,
                    FechaDecreto = det.FechaDecreto,
                    MontoRetenido = det.MontoRetenido,
                    EjercicioFiscal = det.EjercicioFiscal ?? "",
                    Porcion1Monto = det.Porcion1Monto,
                    Porcion1Porcentaje = det.Porcion1Porcentaje,
                    Porcion1FechaPago = det.Porcion1FechaPago,
                    Porcion2Monto = det.Porcion2Monto,
                    Porcion2Porcentaje = det.Porcion2Porcentaje,
                    Porcion2FechaPago = det.Porcion2FechaPago,
                    Porcion3Monto = det.Porcion3Monto,
                    Porcion3Porcentaje = det.Porcion3Porcentaje,
                    Porcion3FechaPago = det.Porcion3FechaPago,
                    Porcion4Monto = det.Porcion4Monto,
                    Porcion4Porcentaje = det.Porcion4Porcentaje,
                    Porcion4FechaPago = det.Porcion4FechaPago,
                    Observaciones = det.Observaciones ?? "",
                    SoporteEnviadoP1 = det.SoporteEnviadoP1,
                    SoporteFechaP1 = det.SoporteFechaP1,
                    SoporteEnviadoP2 = det.SoporteEnviadoP2,
                    SoporteFechaP2 = det.SoporteFechaP2,
                    SoporteEnviadoP3 = det.SoporteEnviadoP3,
                    SoporteFechaP3 = det.SoporteFechaP3,
                    SoporteEnviadoP4 = det.SoporteEnviadoP4,
                    SoporteFechaP4 = det.SoporteFechaP4,
                    RutaCarta = proc?.RutaDividendos ?? "",
                    NombreCarta = proc?.NombreDividendos ?? "",
                    RutaSoporte1 = proc?.RutaActa ?? "",
                    NombreSoporte1 = proc?.NombreActa ?? "",
                    RutaSoporte2 = proc?.RutaDocumento ?? "",
                    NombreSoporte2 = proc?.NombreDocumento ?? "",
                    FechaProceso = proc?.CreateDate ?? DateTime.MinValue,
                    MonedaSimbolo = moneda
                });
            }

            // Ordenar alfabéticamente por accionista
            resultado = resultado
                .OrderBy(x => x.NombreAccionista, StringComparer.OrdinalIgnoreCase)
                .ThenBy(x => x.FechaDecreto)
                .ToList();

            for (int i = 0; i < resultado.Count; i++)
            {
                resultado[i].Numero = i + 1;
            }

            return resultado;
        }

        private static string FormatearCedulaRifReporte(string? tipoDoc, string? documento, int? tipoAccionistaId)
        {
            if (string.IsNullOrWhiteSpace(documento)) return string.Empty;
            string limpio = documento.Trim();

            // Si es Persona Natural (Id == 1) o tiene tipo V / E, formatear con separadores de miles y millones (.)
            if (tipoAccionistaId == 1 || string.Equals(tipoDoc, "V", StringComparison.OrdinalIgnoreCase) || string.Equals(tipoDoc, "E", StringComparison.OrdinalIgnoreCase))
            {
                string prefijo = !string.IsNullOrWhiteSpace(tipoDoc) ? tipoDoc.Trim().ToUpper() : "V";
                var match = System.Text.RegularExpressions.Regex.Match(limpio, @"^([VEPvep])[\-:\s]*(.+)$");
                if (match.Success)
                {
                    prefijo = match.Groups[1].Value.ToUpper();
                    limpio = match.Groups[2].Value.Trim();
                }
                else if (limpio.StartsWith(prefijo, StringComparison.OrdinalIgnoreCase))
                {
                    limpio = limpio.Substring(prefijo.Length).TrimStart('-', ':', ' ', '.');
                }

                if (limpio.Contains('.'))
                {
                    return $"{prefijo}-{limpio}";
                }

                string soloDigitos = System.Text.RegularExpressions.Regex.Replace(limpio, @"[^\d]", "");
                if (long.TryParse(soloDigitos, out long numero))
                {
                    var nfi = new System.Globalization.NumberFormatInfo
                    {
                        NumberGroupSeparator = ".",
                        NumberDecimalDigits = 0
                    };
                    return $"{prefijo}-{numero.ToString("N0", nfi)}";
                }

                return $"{prefijo}-{limpio}";
            }

            // Jurídico o Sucesión
            string pref = !string.IsNullOrWhiteSpace(tipoDoc) ? tipoDoc.Trim().ToUpper() : "";
            if (!string.IsNullOrEmpty(pref) && !limpio.StartsWith(pref, StringComparison.OrdinalIgnoreCase))
            {
                return $"{pref}-{limpio}";
            }
            return limpio;
        }
    }
}