using Microsoft.Data.SqlClient;
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

        public async Task<List<AccionistaSociedad>> GetAllEstatusIdAsync(int estatusId)
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
                .Where(x => x.EstatusAccionista!.Id == estatusId)
                .ToListAsync();
        }

        public async Task<List<AccionistaSociedad>> GetByNameAsync(int estatusId, string name)
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
                .Where(x => x.EstatusAccionista!.Id == estatusId && (x.Accionista.Nombre.Contains(name) || x.Accionista.NombreSusesion.Contains(name) || x.Sociedad.Empresa.Nombre.Contains(name)))
                .ToListAsync();
        }

        public async Task<(List<AccionistaSociedad> Items, int TotalCount)> GetPaginatedAsync(int estatusId, int pageNumber, int pageSize, string searchTerm)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            // 1. Crear la consulta base filtrada (Mantenla lo más ligera posible, sin Includes)
            var queryBase = context.Set<AccionistaSociedad>()
                .AsNoTracking() // Desactiva el tracking para mejorar rendimiento de lectura
                .Where(x => x.EstatusAccionista.Id == estatusId && x.CustodioId == null); // Usa la FK directa si existe en lugar de x.EstatusAccionista!.Id

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                queryBase = queryBase.Where(x => x.Accionista.Nombre.Contains(searchTerm)
                                              || x.Accionista.NombreSusesion.Contains(searchTerm)
                                              || x.Sociedad.Empresa.Nombre.Contains(searchTerm));
            }

            // 2. Obtener el conteo total de forma rápida
            int totalCount = await queryBase.CountAsync();

            // 3. Obtener solo los IDs de la página actual (Operación muy rápida)
            // Nota: El OrderBy se hace aquí para asegurar que la paginación sea consistente
            var pagedIds = await queryBase
                .OrderBy(x => x.Sociedad.Empresa.Nombre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => x.Id) // Suponiendo que tu llave primaria se llama Id
                .ToListAsync();

            // 4. Cargar los datos completos e Includes SOLO para los registros de la página
            var items = await context.Set<AccionistaSociedad>()
                .AsNoTracking()
                .Where(x => pagedIds.Contains(x.Id))
                .OrderBy(x => x.Sociedad.Empresa.Nombre) // Reordenar el set pequeño
                .Include(b => b.Sociedad)
                    .ThenInclude(s => s!.Empresa)
                .Include(b => b.Sociedad)
                    .ThenInclude(s => s!.EstatusSociedad)
                .Include(b => b.Accionista)
                    .ThenInclude(a => a!.TipoAccionista)
                .Include(b => b.EstatusAccionista)
                .ToListAsync();

            return (items, totalCount);
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
                .Include(b => b.Accionista)
                    .ThenInclude(a => a!.TipoDocumento1)
                .Include(b => b.EstatusAccionista)
                .Where(x => x.Sociedad!.Id == sociedadId)
                .ToListAsync();
        }

        public async Task<List<AccionistaSociedad>> GetBySociedadIdEstatusIdAsync(int sociedadId, int estatusId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<AccionistaSociedad>()
                .Include(b => b.Sociedad)
                //.ThenInclude(s => s!.Empresa)
                //.Include(b => b.Sociedad)
                //.ThenInclude(s => s!.EstatusSociedad)
                .Include(b => b.Accionista)
                .ThenInclude(a => a!.TipoAccionista)
                .Include(b => b.EstatusAccionista)
                .Where(x => x.Sociedad!.Id == sociedadId && x.EstatusAccionista!.Id == estatusId)
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
                CustodioId = entity.CustodioId,
                NroAcciones = entity.NroAcciones,
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

            editaccionista.EstatusAccionista = estatusAccionista;

            context.Set<AccionistaSociedad>().Update(editaccionista);
            context.SaveChanges();
        }

        public async Task<AccionistaSociedad?> GetBySociedadIdAccionistaIdAsync(int sociedadId, int accionistaId)
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
                .Where(x => x.Sociedad.Id == sociedadId && x.Accionista.Id == accionistaId)
                .FirstOrDefaultAsync();
        }

        public async Task<AccionistaSociedad?> GetBySociedadIdAccionistaIdCustodioIdAsync(int sociedadId, int accionistaId, int? custodioId)
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
                .Where(x => x.Sociedad.Id == sociedadId && x.Accionista.Id == accionistaId && x.CustodioId == custodioId)
                .FirstOrDefaultAsync();
        }

        public async Task<long> GetTotalAccionesBySociedadIdAsync(int sociedadId, int estatusId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var sum = await context.Set<AccionistaSociedad>()
                .Where(x => x.Sociedad.Id == sociedadId && x.EstatusAccionista.Id == estatusId && x.CustodioId == null)
                .SumAsync(x => x.NroAcciones);
            return sum ?? 0;
        }

        public async Task<List<int>> GetCustodiosIdBySociedadAsync(int sociedadId, int estatusId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<AccionistaSociedad>()
                .Where(x => x.Sociedad.Id == sociedadId && x.CustodioId != null && x.EstatusAccionista.Id == estatusId)
                .Select(x => x.CustodioId.Value)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<AccionistaSociedad>> GetCustodiadosAsync(int sociedadId, int custodioId, int estatusId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<AccionistaSociedad>()
                .Include(b => b.Accionista).ThenInclude(a => a!.TipoAccionista)
                .Include(b => b.Accionista).ThenInclude(a => a!.TipoDocumento1)
                .Include(b => b.EstatusAccionista)
                .Where(x => x.Sociedad.Id == sociedadId && x.CustodioId == custodioId && x.EstatusAccionista.Id == estatusId)
                .OrderBy(x => x.Accionista.Nombre)
                .ToListAsync();
        }

        public async Task<AccionistaSociedad> AddReturnEntidadAsync(AccionistaSociedadDto entity)
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
                CustodioId = entity.CustodioId,
                NroAcciones = entity.NroAcciones,
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

            AuditoriaNroAccion auditoria = new()
            {
                SociedadId = entity.SociedadId ?? 0,
                AccionistaId = entity.AccionistaId ?? 0,
                NroAcciones = entity.NroAcciones ?? 0,
                Accion = "Insert",
                Descripcion = $"Se creó la relación entre el accionista con ID {entity.AccionistaId} y la sociedad con ID {entity.SociedadId}.",
                CreateUserId = entity.CreateUserId,
                CreateDate = DateTime.UtcNow
            };
            await context.Set<AuditoriaNroAccion>().AddAsync(auditoria);
            await context.SaveChangesAsync();

            return newEntidad;
        }

        public async Task UpdateNroAccionesAsync(int accionistaid, int sociedadid, Int64 nroacciones, int updateUserId, int opcioaudi)
        {
            await UpdateNroAccionesCustodioAsync(accionistaid, sociedadid, null, nroacciones, updateUserId, opcioaudi);
        }

        public async Task UpdateNroAccionesCustodioAsync(int accionistaid, int sociedadid, int? custodioid, Int64 nroacciones, int updateUserId, int opcioaudi)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var accionista = await context.Set<Accionista>().FindAsync(accionistaid);
            if (accionista == null) throw new Exception($"Accionista con ID {accionistaid} no existe.");

            var sociedad = await context.Set<Sociedad>().FindAsync(sociedadid);
            if (sociedad == null) throw new Exception($"Sociedad con ID {sociedadid} no existe.");

            AccionistaSociedad? editentidad = await context.AccionistasSociedades
                .Include(x => x.EstatusAccionista)
                .FirstOrDefaultAsync(x => x.Accionista.Id == accionistaid 
                                       && x.Sociedad.Id == sociedadid 
                                       && x.CustodioId == custodioid);

            if (editentidad == null && custodioid == null)
            {
                editentidad = await context.AccionistasSociedades
                    .Include(x => x.EstatusAccionista)
                    .FirstOrDefaultAsync(x => x.Accionista.Id == accionistaid && x.Sociedad.Id == sociedadid && x.CustodioId == null);
            }

            if (editentidad == null) return;

            var estatusActivo = await context.Set<EstatusAccionista>().FindAsync(1);
            long saldoAnterior = editentidad.NroAcciones ?? 0;

            if (saldoAnterior != nroacciones || editentidad.EstatusAccionista?.Id != 1)
            {
                editentidad.NroAcciones = nroacciones;
                editentidad.EstatusAccionista = estatusActivo;
                editentidad.UpdateDate = DateTime.UtcNow;
                editentidad.UpdateUserId = updateUserId;

                context.Set<AccionistaSociedad>().Update(editentidad);
                await context.SaveChangesAsync();

                if (opcioaudi == 1 && saldoAnterior != nroacciones)
                {
                    long diferencia = nroacciones - saldoAnterior;
                    string accionAudit = diferencia > 0 ? "Compra" : "Venta";
                    string desc = diferencia > 0
                        ? $"Compra de acciones en conciliación: +{diferencia:N0} acciones (Saldo anterior: {saldoAnterior:N0}, Nuevo: {nroacciones:N0})"
                        : $"Venta de acciones en conciliación: {diferencia:N0} acciones (Saldo anterior: {saldoAnterior:N0}, Nuevo: {nroacciones:N0})";

                    AuditoriaNroAccion auditoria = new()
                    {
                        SociedadId = sociedadid,
                        AccionistaId = accionistaid,
                        NroAcciones = nroacciones,
                        Accion = accionAudit,
                        Descripcion = desc,
                        CreateUserId = updateUserId,
                        CreateDate = DateTime.UtcNow
                    };
                    await context.Set<AuditoriaNroAccion>().AddAsync(auditoria);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<AuditoriaNroAccion>> GetAuditoriaBySociedadIdAccionistaIdAsync(int sociedadId, int accionistaId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<AuditoriaNroAccion>()
                .Where(a => a.SociedadId == sociedadId && a.AccionistaId == accionistaId)
                .ToListAsync();
        }

        public async Task AccionistasSociedadInactivar(int procesoid, int? custodioId = null)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Database.SetCommandTimeout(300);

            var paramProcesoId = new SqlParameter("@ProcesoId", procesoid);
            var paramCustodioId = new SqlParameter("@CustodioId", (object?)custodioId ?? DBNull.Value);

            await context.Database.ExecuteSqlRawAsync("EXEC SP_AccionistasSociedadInactivar @ProcesoId, @CustodioId", paramProcesoId, paramCustodioId);
        }

        public async Task<List<AccionistaSociedadHistorico>> GetHistoricoBySociedadIdRangeDateAsync(int sociedadId, DateTime endDate)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            // 1. Filtramos y agrupamos directamente en la tabla historico para obtener solo los IDs ganadores
            var registrosGanadoresIds = await context.Set<AccionistaSociedadHistorico>()
                .Where(h => h.Sociedad.Id == sociedadId && h.CreateDate == endDate)
                .GroupBy(h => h.Accionista.Id)
                .Select(grupo => grupo.OrderByDescending(g => g.CreateDate).Select(g => g.Id).FirstOrDefault())
                .ToListAsync();

            // 2. Ahora cargamos los registros completos con todos sus Includes correspondientes
            return await context.Set<AccionistaSociedadHistorico>()
                .Include(b => b.Sociedad)
                    .ThenInclude(s => s!.Empresa)
                .Include(b => b.Accionista)
                    .ThenInclude(a => a!.TipoAccionista)
                .Where(h => registrosGanadoresIds.Contains(h.Id))
                .ToListAsync();
        }

        public async Task<int> GetTotalAccionistasBySociedadIdAsync(int sociedadId, int estatusId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var count = await context.Set<AccionistaSociedad>()
                .Where(x => x.Sociedad.Id == sociedadId && x.EstatusAccionista.Id == estatusId && x.CustodioId == null)
                .CountAsync();
            return count;
        }

        public async Task<int> GetTotalCustodiadosCountBySociedadIdAsync(int sociedadId, int estatusId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<AccionistaSociedad>()
                .Where(x => x.Sociedad.Id == sociedadId && x.EstatusAccionista.Id == estatusId && x.CustodioId != null)
                .CountAsync();
        }

        public async Task<List<Sociedad>> GetAllSociedadesAsync(int estatusId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            return await context.Set<Sociedad>()
                .Include(s => s.Empresa)
                .Include(s => s.EstatusSociedad)
                // Filtramos directamente en la base de datos si existe alguna relación en AccionistaSociedad con ese estatus
                .Where(s => context.Set<AccionistaSociedad>()
                    .Any(a => a.Sociedad.Id == s.Id && a.EstatusAccionista!.Id == estatusId))
                .ToListAsync();
        }

        public async Task<List<AccionistaSociedadResumenDto>> GetResumenBySociedadIdAsync(int sociedadId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.AccionistasSociedades
                .AsNoTracking()
                .Where(x => x.Sociedad!.Id == sociedadId)
                .Select(x => new AccionistaSociedadResumenDto
                {
                    Id = x.Id,
                    AccionistaId = x.Accionista!.Id,
                    CustodioId = x.CustodioId,
                    NroAcciones = x.NroAcciones,
                    EstatusAccionistaId = x.EstatusAccionista != null ? x.EstatusAccionista.Id : null
                })
                .ToListAsync();
        }

        public async Task ActualizarSaldosYAuditoriaConciliacionLoteAsync(int procesoId, int sociedadId, int? custodioId, int userId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.Database.SetCommandTimeout(300);

            var paramProcesoId = new SqlParameter("@ProcesoId", procesoId);
            var paramSociedadId = new SqlParameter("@SociedadId", sociedadId);
            var paramCustodioId = new SqlParameter("@CustodioId", (object?)custodioId ?? DBNull.Value);
            var paramUserId = new SqlParameter("@UserId", userId);

            string sql = @"
                -- 1. Insertar auditoría de cambios de saldo (Compra / Venta) para los que variaron su cantidad de acciones
                INSERT INTO dbo.AuditoriasNroAcciones 
                    (SociedadId, AccionistaId, NroAcciones, Accion, Descripcion, CreateUserId, CreateDate)
                SELECT 
                    @SociedadId,
                    asoc.AccionistaId,
                    cd.TotalAcciones,
                    CASE WHEN cd.TotalAcciones > ISNULL(asoc.NroAcciones, 0) THEN 'Compra' ELSE 'Venta' END,
                    CASE WHEN cd.TotalAcciones > ISNULL(asoc.NroAcciones, 0) 
                         THEN 'Compra de acciones en conciliación: +' + CAST(cd.TotalAcciones - ISNULL(asoc.NroAcciones, 0) AS VARCHAR(30)) + ' acciones (Saldo anterior: ' + CAST(ISNULL(asoc.NroAcciones, 0) AS VARCHAR(30)) + ', Nuevo: ' + CAST(cd.TotalAcciones AS VARCHAR(30)) + ')'
                         ELSE 'Venta de acciones en conciliación: ' + CAST(cd.TotalAcciones - ISNULL(asoc.NroAcciones, 0) AS VARCHAR(30)) + ' acciones (Saldo anterior: ' + CAST(ISNULL(asoc.NroAcciones, 0) AS VARCHAR(30)) + ', Nuevo: ' + CAST(cd.TotalAcciones AS VARCHAR(30)) + ')'
                    END,
                    @UserId,
                    GETDATE()
                FROM dbo.AccionistasSociedades asoc
                INNER JOIN dbo.ConciliacionesDetalles cd ON cd.AccionistaId = asoc.AccionistaId
                WHERE cd.ConciliacionId = @ProcesoId
                  AND asoc.SociedadId = @SociedadId
                  AND ((@CustodioId IS NULL AND asoc.CustodioId IS NULL) OR asoc.CustodioId = @CustodioId)
                  AND (asoc.NroAcciones IS NULL OR asoc.NroAcciones <> cd.TotalAcciones);

                -- 2. Actualizar saldos y estatus activo de todos los que vinieron en la conciliación
                UPDATE asoc
                SET asoc.NroAcciones = cd.TotalAcciones,
                    asoc.EstatusAccionistaId = 1,
                    asoc.UpdateDate = GETDATE(),
                    asoc.UpdateUserId = @UserId
                FROM dbo.AccionistasSociedades asoc
                INNER JOIN dbo.ConciliacionesDetalles cd ON cd.AccionistaId = asoc.AccionistaId
                WHERE cd.ConciliacionId = @ProcesoId
                  AND asoc.SociedadId = @SociedadId
                  AND ((@CustodioId IS NULL AND asoc.CustodioId IS NULL) OR asoc.CustodioId = @CustodioId);
            ";

            await context.Database.ExecuteSqlRawAsync(sql, paramProcesoId, paramSociedadId, paramCustodioId, paramUserId);
        }
    }
}