using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class SociedadRepository(IDbContextFactory<SociedadDbContext> contextFactory) : ISociedad
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<Sociedad>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Sociedad>()
                .Include(b => b.Region)
                .Include(b => b.UnidadNegocio)
                .Include(b => b.Empresa)
                //.Include(b => b.TipoSociedad)
                .Include(b => b.EstatusSociedad)
                .Include(b => b.TipoSociedadActiva)
                .Include(b => b.Moneda)
                .Include(b => b.NombreDiario)
                .Include(b => b.TipoDocumento1)
                .Include(b => b.TipoDocumento2)
                .Include(b => b.TipoDocumento3)
                .ToListAsync();
        }

        public async Task<Sociedad?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Sociedad>()
                .Include(b => b.Region)
                .Include(b => b.UnidadNegocio)
                .Include(b => b.Empresa)
                //.Include(b => b.TipoSociedad)
                .Include(b => b.EstatusSociedad)
                .Include(b => b.TipoSociedadActiva)
                .Include(b => b.Moneda)
                .Include(b => b.NombreDiario)
                .Include(b => b.TipoDocumento1)
                .Include(b => b.TipoDocumento2)
                .Include(b => b.TipoDocumento3)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task AddAsync(SociedadDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var region = await context.Set<Region>().FindAsync(entity.RegionId);
            if (region == null) throw new Exception($"Region con ID {entity.RegionId} no existe.");

            var unidadNegocio = await context.Set<UnidadNegocio>().FindAsync(entity.UnidadNegocioId);
            if (unidadNegocio == null) throw new Exception($"UnidadNegocio con ID {entity.UnidadNegocioId} no existe.");

            var empresa = await context.Set<Empresa>().FindAsync(entity.EmpresaId);
            if (empresa == null) throw new Exception($"Empresa con ID {entity.EmpresaId} no existe.");

            //var tipoSociedad = await context.Set<TipoSociedad>().FindAsync(entity.TipoSociedadId);
            //if(tipoSociedad == null) throw new Exception($"TipoSociedad con ID {entity.TipoSociedadId} no existe.");

            var estatusSociedad = await context.Set<EstatusSociedad>().FindAsync(entity.EstatusSociedadId);
            if (estatusSociedad == null) throw new Exception($"EstatusSociedad con ID {entity.EstatusSociedadId} no existe.");

            var tipoSociedadActiva = await context.Set<TipoSociedadActiva>().FindAsync(entity.TipoSociedadActivaId);

            var moneda = await context.Set<Moneda>().FindAsync(entity.MonedaId);
            if (moneda == null) throw new Exception($"Moneda con ID {entity.MonedaId} no existe.");

            NombreDiario? nombre = null;
            if (entity.NombreDiarioId > 0)
            {
                nombre = await context.Set<NombreDiario>().FindAsync(entity.NombreDiarioId);
                if (nombre == null) throw new Exception($"NombreDiario con ID {entity.NombreDiarioId} no existe.");
            }

            TipoDocumento? tipodoc1 = null;
            if (entity.TipoDocumento1Id != null)
            {
                if (entity.TipoDocumento1Id != 0)
                {
                    tipodoc1 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento1Id.Value);
                    if (tipodoc1 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento1Id.Value} no existe.");
                }
            }

            TipoDocumento? tipodoc2 = null;
            if (entity.TipoDocumento2Id != null)
            {
                if (entity.TipoDocumento2Id != 0)
                {
                    tipodoc2 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento2Id.Value);
                    if (tipodoc2 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento2Id.Value} no existe.");
                }
            }

            TipoDocumento? tipodoc3 = null;
            if (entity.TipoDocumento3Id != null)
            {
                if (entity.TipoDocumento3Id != 0)
                {
                    tipodoc3 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento3Id.Value);
                    if (tipodoc3 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento3Id.Value} no existe.");
                }
            }

            Sociedad? newsociedad = new()
            {
                Region = region,
                UnidadNegocio = unidadNegocio,
                Empresa = empresa,
                NumeroSap = entity.NumeroSap,
                //TipoSociedad = tipoSociedad,
                EstatusSociedad = estatusSociedad,
                TipoSociedadActiva = tipoSociedadActiva,
                Objeto = entity.Objeto,
                Domicilio = entity.Domicilio,
                DireccionFiscal = entity.DireccionFiscal,
                DatosConstitucion = entity.DatosConstitucion,
                FechaConstitucion = entity.FechaConstitucion,
                FechaVencimiento = entity.FechaVencimiento,
                Duracion = entity.Duracion,
                NumeroAcciones = entity.NumeroAcciones,
                AplicaCapital = entity.AplicaCapital,
                Moneda = moneda,
                CapitalSuscrito = entity.CapitalSuscrito,
                CapitalPagado = entity.CapitalPagado,
                ClaseAcciones = entity.ClaseAcciones,
                FormaAdministracion = entity.FormaAdministracion,
                EjercicioEconomico = entity.EjercicioEconomico,
                NumeroExpediente = entity.NumeroExpediente,
                Observaciones = entity.Observaciones,
                Documento1 = entity.Documento1,
                Documento2 = entity.Documento2,
                Documento3 = entity.Documento3,
                TipoDocumento1 = tipodoc1,
                TipoDocumento2 = tipodoc2,
                TipoDocumento3 = tipodoc3,
                Nit = entity.Nit,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId ?? 0,
                UpdateUserId = entity.UpdateUserId ?? 0,
                ValorAccion = entity.ValorAccion,
                ValorPatrimonial = entity.ValorPatrimonial,
                AnoPublicacion = entity.AnoPublicacion,
                NumeroPublicacion = entity.NumeroPublicacion,
                FechaPublicacion = entity.FechaPublicacion,
                NombreDiario = nombre
            };

            await context.Set<Sociedad>().AddAsync(newsociedad);
            context.Entry(newsociedad.Region).State = EntityState.Unchanged;
            context.Entry(newsociedad.UnidadNegocio).State = EntityState.Unchanged;
            context.Entry(newsociedad.Empresa).State = EntityState.Unchanged;
            //context.Entry(newsociedad.TipoSociedad).State = EntityState.Unchanged;
            context.Entry(newsociedad.EstatusSociedad).State = EntityState.Unchanged;
            if (newsociedad.TipoSociedadActiva != null)
                context.Entry(newsociedad.TipoSociedadActiva).State = EntityState.Unchanged;
            context.Entry(newsociedad.Moneda).State = EntityState.Unchanged;
            if (newsociedad.NombreDiario != null)
                context.Entry(newsociedad.NombreDiario).State = EntityState.Unchanged;
            if (tipodoc1 != null) context.Entry(newsociedad.TipoDocumento1!).State = EntityState.Unchanged;
            if (tipodoc2 != null) context.Entry(newsociedad.TipoDocumento2!).State = EntityState.Unchanged;
            if (tipodoc3 != null) context.Entry(newsociedad.TipoDocumento3!).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(newsociedad.Region).State = EntityState.Detached;
            context.Entry(newsociedad.UnidadNegocio).State = EntityState.Detached;
            context.Entry(newsociedad.Empresa).State = EntityState.Detached;
            //context.Entry(newsociedad.TipoSociedad).State = EntityState.Detached;
            context.Entry(newsociedad.EstatusSociedad).State = EntityState.Detached;
            if (newsociedad.TipoSociedadActiva != null)
                context.Entry(newsociedad.TipoSociedadActiva).State = EntityState.Detached;
            context.Entry(newsociedad.Moneda).State = EntityState.Detached;
            if(newsociedad.NombreDiario != null)
                context.Entry(newsociedad.NombreDiario).State = EntityState.Detached;
            if (tipodoc1 != null) context.Entry(newsociedad.TipoDocumento1!).State = EntityState.Detached;
            if (tipodoc2 != null) context.Entry(newsociedad.TipoDocumento2!).State = EntityState.Detached;
            if (tipodoc3 != null) context.Entry(newsociedad.TipoDocumento3!).State = EntityState.Detached;
        }

        public async void Update(SociedadDto entity)
        {
            using var context = _contextFactory.CreateDbContext();

            var region = await context.Set<Region>().FindAsync(entity.RegionId);
            if (region == null) throw new Exception($"Region con ID {entity.RegionId} no existe.");

            var unidadNegocio = await context.Set<UnidadNegocio>().FindAsync(entity.UnidadNegocioId);
            if (unidadNegocio == null) throw new Exception($"UnidadNegocio con ID {entity.UnidadNegocioId} no existe.");

            var empresa = await context.Set<Empresa>().FindAsync(entity.EmpresaId);
            if (empresa == null) throw new Exception($"Empresa con ID {entity.EmpresaId} no existe.");

            //var tipoSociedad = await context.Set<TipoSociedad>().FindAsync(entity.TipoSociedadId);
            //if (tipoSociedad == null) throw new Exception($"TipoSociedad con ID {entity.TipoSociedadId} no existe.");

            var estatusSociedad = await context.Set<EstatusSociedad>().FindAsync(entity.EstatusSociedadId);
            if (estatusSociedad == null) throw new Exception($"EstatusSociedad con ID {entity.EstatusSociedadId} no existe.");

            TipoSociedadActiva? tipoSociedadActiva = null;
            if (entity.TipoSociedadActivaId != null)
            {
                tipoSociedadActiva = await context.Set<TipoSociedadActiva>().FindAsync(entity.TipoSociedadActivaId);
                if (tipoSociedadActiva == null) throw new Exception($"TipoSociedadActiva con ID {entity.TipoSociedadActivaId} no existe.");
            }

            var moneda = await context.Set<Moneda>().FindAsync(entity.MonedaId);
            if (moneda == null) throw new Exception($"Moneda con ID {entity.MonedaId} no existe.");

            NombreDiario? nombre = null;
            if (entity.NombreDiarioId > 0)
            {
                nombre = await context.Set<NombreDiario>().FindAsync(entity.NombreDiarioId);
                if (nombre == null) throw new Exception($"NombreDiario con ID {entity.NombreDiarioId} no existe.");
            }

            TipoDocumento? tipodoc1 = null;
            if (entity.TipoDocumento1Id != null)
            {
                if (entity.TipoDocumento1Id != 0)
                {
                    tipodoc1 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento1Id.Value);
                    if (tipodoc1 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento1Id.Value} no existe.");
                }
            }

            TipoDocumento? tipodoc2 = null;
            if (entity.TipoDocumento2Id != null)
            {
                if (entity.TipoDocumento2Id != 0)
                {
                    tipodoc2 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento2Id.Value);
                    if (tipodoc2 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento2Id.Value} no existe.");
                }
            }

            TipoDocumento? tipodoc3 = null;
            if (entity.TipoDocumento3Id != null)
            {
                if (entity.TipoDocumento3Id != 0)
                {
                    tipodoc3 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento3Id.Value);
                    if (tipodoc3 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento3Id.Value} no existe.");
                }
            }

            Sociedad? editsociedad = await context.Sociedades.FindAsync(entity.Id);
            if (editsociedad == null) throw new Exception($"Sociedad con ID {entity.Id} no existe.");

            editsociedad.Region = region;
            editsociedad.UnidadNegocio = unidadNegocio;
            editsociedad.Empresa = empresa;
            editsociedad.NumeroSap = entity.NumeroSap;
            //editsociedad.TipoSociedad = tipoSociedad;
            editsociedad.EstatusSociedad = estatusSociedad;
            if (entity.TipoSociedadActivaId != null)
                editsociedad.TipoSociedadActiva = tipoSociedadActiva;

            editsociedad.Objeto = entity.Objeto;
            editsociedad.Domicilio = entity.Domicilio;
            editsociedad.DireccionFiscal = entity.DireccionFiscal;
            editsociedad.DatosConstitucion = entity.DatosConstitucion;
            editsociedad.FechaConstitucion = entity.FechaConstitucion;
            editsociedad.FechaVencimiento = entity.FechaVencimiento;
            editsociedad.Duracion = entity.Duracion;
            editsociedad.NumeroAcciones = entity.NumeroAcciones;
            editsociedad.AplicaCapital = entity.AplicaCapital;
            editsociedad.Moneda = moneda;
            editsociedad.CapitalSuscrito = entity.CapitalSuscrito;
            editsociedad.CapitalPagado = entity.CapitalPagado;
            editsociedad.ClaseAcciones = entity.ClaseAcciones;
            editsociedad.FormaAdministracion = entity.FormaAdministracion;
            editsociedad.EjercicioEconomico = entity.EjercicioEconomico;
            editsociedad.NumeroExpediente = entity.NumeroExpediente;
            editsociedad.Observaciones = entity.Observaciones;
            editsociedad.Documento1 = entity.Documento1;
            editsociedad.Documento2 = entity.Documento2;
            editsociedad.Documento3 = entity.Documento3;
            editsociedad.TipoDocumento1 = tipodoc1;
            editsociedad.TipoDocumento2 = tipodoc2;
            editsociedad.TipoDocumento3 = tipodoc3;
            editsociedad.Nit = entity.Nit;
            editsociedad.UpdateDate = DateTime.UtcNow;
            editsociedad.UpdateUserId = entity.UpdateUserId ?? 0;
            editsociedad.ValorAccion = entity.ValorAccion;
            editsociedad.ValorPatrimonial = entity.ValorPatrimonial;
            editsociedad.AnoPublicacion = entity.AnoPublicacion;
            editsociedad.NumeroPublicacion = entity.NumeroPublicacion;
            editsociedad.FechaPublicacion = entity.FechaPublicacion;
            editsociedad.NombreDiario = nombre;

            context.Set<Sociedad>().Update(editsociedad);
            context.SaveChanges();
        }

        public async Task AddAsync(SociedadInactivaDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var region = await context.Set<Region>().FindAsync(entity.RegionId);
            if (region == null) throw new Exception($"Region con ID {entity.RegionId} no existe.");

            var unidadNegocio = await context.Set<UnidadNegocio>().FindAsync(entity.UnidadNegocioId);
            if (unidadNegocio == null) throw new Exception($"UnidadNegocio con ID {entity.UnidadNegocioId} no existe.");

            var empresa = await context.Set<Empresa>().FindAsync(entity.EmpresaId);
            if (empresa == null) throw new Exception($"Empresa con ID {entity.EmpresaId} no existe.");

            //var tipoSociedad = await context.Set<TipoSociedad>().FindAsync(entity.TipoSociedadId);
            //if(tipoSociedad == null) throw new Exception($"TipoSociedad con ID {entity.TipoSociedadId} no existe.");

            var estatusSociedad = await context.Set<EstatusSociedad>().FindAsync(entity.EstatusSociedadId);
            if (estatusSociedad == null) throw new Exception($"EstatusSociedad con ID {entity.EstatusSociedadId} no existe.");

            var tipoSociedadActiva = await context.Set<TipoSociedadActiva>().FindAsync(entity.TipoSociedadActivaId);

            var moneda = await context.Set<Moneda>().FindAsync(entity.MonedaId);
            if (moneda == null) throw new Exception($"Moneda con ID {entity.MonedaId} no existe.");

            NombreDiario? nombre = null;
            if (entity.NombreDiarioId > 0)
            {
                nombre = await context.Set<NombreDiario>().FindAsync(entity.NombreDiarioId);
                if (nombre == null) throw new Exception($"NombreDiario con ID {entity.NombreDiarioId} no existe.");
            }

            TipoDocumento? tipodoc1 = null;
            if (entity.TipoDocumento1Id != null)
            {
                if (entity.TipoDocumento1Id != 0)
                {
                    tipodoc1 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento1Id.Value);
                    if (tipodoc1 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento1Id.Value} no existe.");
                }
            }

            TipoDocumento? tipodoc2 = null;
            if (entity.TipoDocumento2Id != null)
            {
                if (entity.TipoDocumento2Id != 0)
                {
                    tipodoc2 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento2Id.Value);
                    if (tipodoc2 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento2Id.Value} no existe.");
                }
            }

            TipoDocumento? tipodoc3 = null;
            if (entity.TipoDocumento3Id != null)
            {
                if (entity.TipoDocumento3Id != 0)
                {
                    tipodoc3 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento3Id.Value);
                    if (tipodoc3 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento3Id.Value} no existe.");
                }
            }

            Sociedad? newsociedad = new()
            {
                Region = region,
                UnidadNegocio = unidadNegocio,
                Empresa = empresa,
                NumeroSap = entity.NumeroSap,
                //TipoSociedad = tipoSociedad,
                EstatusSociedad = estatusSociedad,
                TipoSociedadActiva = tipoSociedadActiva,
                Objeto = entity.Objeto,
                Domicilio = entity.Domicilio,
                DireccionFiscal = entity.DireccionFiscal,
                DatosConstitucion = entity.DatosConstitucion,
                FechaConstitucion = entity.FechaConstitucion,
                FechaVencimiento = entity.FechaVencimiento,
                Duracion = entity.Duracion,
                NumeroAcciones = entity.NumeroAcciones,
                AplicaCapital = entity.AplicaCapital,
                Moneda = moneda,
                CapitalSuscrito = entity.CapitalSuscrito,
                CapitalPagado = entity.CapitalPagado,
                ClaseAcciones = entity.ClaseAcciones,
                FormaAdministracion = entity.FormaAdministracion,
                EjercicioEconomico = entity.EjercicioEconomico,
                NumeroExpediente = entity.NumeroExpediente,
                Observaciones = entity.Observaciones,
                Documento1 = entity.Documento1,
                Documento2 = entity.Documento2,
                Documento3 = entity.Documento3,
                TipoDocumento1 = tipodoc1,
                TipoDocumento2 = tipodoc2,
                TipoDocumento3 = tipodoc3,
                Nit = entity.Nit,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId ?? 0,
                UpdateUserId = entity.UpdateUserId ?? 0,
                ValorAccion = entity.ValorAccion,
                ValorPatrimonial = entity.ValorPatrimonial,
                AnoPublicacion = entity.AnoPublicacion,
                NumeroPublicacion = entity.NumeroPublicacion,
                FechaPublicacion = entity.FechaPublicacion,
                NombreDiario = nombre
            };

            await context.Set<Sociedad>().AddAsync(newsociedad);
            context.Entry(newsociedad.Region).State = EntityState.Unchanged;
            context.Entry(newsociedad.UnidadNegocio).State = EntityState.Unchanged;
            context.Entry(newsociedad.Empresa).State = EntityState.Unchanged;
            //context.Entry(newsociedad.TipoSociedad).State = EntityState.Unchanged;
            context.Entry(newsociedad.EstatusSociedad).State = EntityState.Unchanged;
            if (newsociedad.TipoSociedadActiva != null)
                context.Entry(newsociedad.TipoSociedadActiva).State = EntityState.Unchanged;
            context.Entry(newsociedad.Moneda).State = EntityState.Unchanged;
            if (newsociedad.NombreDiario != null)
                context.Entry(newsociedad.NombreDiario).State = EntityState.Unchanged;
            if (tipodoc1 != null) context.Entry(newsociedad.TipoDocumento1!).State = EntityState.Unchanged;
            if (tipodoc2 != null) context.Entry(newsociedad.TipoDocumento2!).State = EntityState.Unchanged;
            if (tipodoc3 != null) context.Entry(newsociedad.TipoDocumento3!).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(newsociedad.Region).State = EntityState.Detached;
            context.Entry(newsociedad.UnidadNegocio).State = EntityState.Detached;
            context.Entry(newsociedad.Empresa).State = EntityState.Detached;
            //context.Entry(newsociedad.TipoSociedad).State = EntityState.Detached;
            context.Entry(newsociedad.EstatusSociedad).State = EntityState.Detached;
            if (newsociedad.TipoSociedadActiva != null)
                context.Entry(newsociedad.TipoSociedadActiva).State = EntityState.Detached;
            context.Entry(newsociedad.Moneda).State = EntityState.Detached;
            if (newsociedad.NombreDiario != null)
                context.Entry(newsociedad.NombreDiario).State = EntityState.Detached;
            if (tipodoc1 != null) context.Entry(newsociedad.TipoDocumento1!).State = EntityState.Detached;
            if (tipodoc2 != null) context.Entry(newsociedad.TipoDocumento2!).State = EntityState.Detached;
            if (tipodoc3 != null) context.Entry(newsociedad.TipoDocumento3!).State = EntityState.Detached;
        }

        public async void Update(SociedadInactivaDto entity)
        {
            using var context = _contextFactory.CreateDbContext();

            var region = await context.Set<Region>().FindAsync(entity.RegionId);
            if (region == null) throw new Exception($"Region con ID {entity.RegionId} no existe.");

            var unidadNegocio = await context.Set<UnidadNegocio>().FindAsync(entity.UnidadNegocioId);
            if (unidadNegocio == null) throw new Exception($"UnidadNegocio con ID {entity.UnidadNegocioId} no existe.");

            var empresa = await context.Set<Empresa>().FindAsync(entity.EmpresaId);
            if (empresa == null) throw new Exception($"Empresa con ID {entity.EmpresaId} no existe.");

            //var tipoSociedad = await context.Set<TipoSociedad>().FindAsync(entity.TipoSociedadId);
            //if (tipoSociedad == null) throw new Exception($"TipoSociedad con ID {entity.TipoSociedadId} no existe.");

            var estatusSociedad = await context.Set<EstatusSociedad>().FindAsync(entity.EstatusSociedadId);
            if (estatusSociedad == null) throw new Exception($"EstatusSociedad con ID {entity.EstatusSociedadId} no existe.");

            TipoSociedadActiva? tipoSociedadActiva = null;
            if (entity.TipoSociedadActivaId != null)
            {
                tipoSociedadActiva = await context.Set<TipoSociedadActiva>().FindAsync(entity.TipoSociedadActivaId);
                if (tipoSociedadActiva == null) throw new Exception($"TipoSociedadActiva con ID {entity.TipoSociedadActivaId} no existe.");
            }

            var moneda = await context.Set<Moneda>().FindAsync(entity.MonedaId);
            if (moneda == null) throw new Exception($"Moneda con ID {entity.MonedaId} no existe.");

            NombreDiario? nombre = null;
            if (entity.NombreDiarioId > 0)
            {
                nombre = await context.Set<NombreDiario>().FindAsync(entity.NombreDiarioId);
                if (nombre == null) throw new Exception($"NombreDiario con ID {entity.NombreDiarioId} no existe.");
            }

            TipoDocumento? tipodoc1 = null;
            if (entity.TipoDocumento1Id != null)
            {
                if (entity.TipoDocumento1Id != 0)
                {
                    tipodoc1 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento1Id.Value);
                    if (tipodoc1 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento1Id.Value} no existe.");
                }
            }

            TipoDocumento? tipodoc2 = null;
            if (entity.TipoDocumento2Id != null)
            {
                if (entity.TipoDocumento2Id != 0)
                {
                    tipodoc2 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento2Id.Value);
                    if (tipodoc2 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento2Id.Value} no existe.");
                }
            }

            TipoDocumento? tipodoc3 = null;
            if (entity.TipoDocumento3Id != null)
            {
                if (entity.TipoDocumento3Id != 0)
                {
                    tipodoc3 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento3Id.Value);
                    if (tipodoc3 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento3Id.Value} no existe.");
                }
            }

            Sociedad? editsociedad = await context.Sociedades.FindAsync(entity.Id);
            if (editsociedad == null) throw new Exception($"Sociedad con ID {entity.Id} no existe.");

            editsociedad.Region = region;
            editsociedad.UnidadNegocio = unidadNegocio;
            editsociedad.Empresa = empresa;
            editsociedad.NumeroSap = entity.NumeroSap;
            //editsociedad.TipoSociedad = tipoSociedad;
            editsociedad.EstatusSociedad = estatusSociedad;
            if (entity.TipoSociedadActivaId != null)
                editsociedad.TipoSociedadActiva = tipoSociedadActiva;

            editsociedad.Objeto = entity.Objeto;
            editsociedad.Domicilio = entity.Domicilio;
            editsociedad.DireccionFiscal = entity.DireccionFiscal;
            editsociedad.DatosConstitucion = entity.DatosConstitucion;
            editsociedad.FechaConstitucion = entity.FechaConstitucion;
            editsociedad.FechaVencimiento = entity.FechaVencimiento;
            editsociedad.Duracion = entity.Duracion;
            editsociedad.NumeroAcciones = entity.NumeroAcciones;
            editsociedad.AplicaCapital = entity.AplicaCapital;
            editsociedad.Moneda = moneda;
            editsociedad.CapitalSuscrito = entity.CapitalSuscrito;
            editsociedad.CapitalPagado = entity.CapitalPagado;
            editsociedad.ClaseAcciones = entity.ClaseAcciones;
            editsociedad.FormaAdministracion = entity.FormaAdministracion;
            editsociedad.EjercicioEconomico = entity.EjercicioEconomico;
            editsociedad.NumeroExpediente = entity.NumeroExpediente;
            editsociedad.Observaciones = entity.Observaciones;
            editsociedad.Documento1 = entity.Documento1;
            editsociedad.Documento2 = entity.Documento2;
            editsociedad.Documento3 = entity.Documento3;
            editsociedad.TipoDocumento1 = tipodoc1;
            editsociedad.TipoDocumento2 = tipodoc2;
            editsociedad.TipoDocumento3 = tipodoc3;
            editsociedad.Nit = entity.Nit;
            editsociedad.UpdateDate = DateTime.UtcNow;
            editsociedad.UpdateUserId = entity.UpdateUserId ?? 0;
            editsociedad.ValorAccion = entity.ValorAccion;
            editsociedad.ValorPatrimonial = entity.ValorPatrimonial;
            editsociedad.AnoPublicacion = entity.AnoPublicacion;
            editsociedad.NumeroPublicacion = entity.NumeroPublicacion;
            editsociedad.FechaPublicacion = entity.FechaPublicacion;
            editsociedad.NombreDiario = nombre;

            context.Set<Sociedad>().Update(editsociedad);
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

        public async void Activar(int Id, int accion)
        {
            using var context = _contextFactory.CreateDbContext();

            Sociedad? editsociedad = await context.Sociedades.FindAsync(Id);
            if (editsociedad == null) throw new Exception($"Sociedad con ID {Id} no existe.");

            var estatusSociedad = await context.Set<EstatusSociedad>().FindAsync(accion);
            if (estatusSociedad == null) throw new Exception($"EstatusSociedad con ID {accion} no existe.");

            TipoSociedadActiva? tipoSociedadActiva = null;
            if (accion != 1)
            {
                tipoSociedadActiva = await context.Set<TipoSociedadActiva>().FindAsync(1);
                if (tipoSociedadActiva == null) throw new Exception($"TipoSociedadActiva con ID {1} no existe.");
            }

            editsociedad.EstatusSociedad = estatusSociedad;
            editsociedad.TipoSociedadActiva = tipoSociedadActiva;            

            context.Set<Sociedad>().Update(editsociedad);
            context.SaveChanges();
        }
    }
}