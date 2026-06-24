using Microsoft.EntityFrameworkCore;
using SociePolar.Application.Interfaces;
using SociePolar.Domain.Dtos;
using SociePolar.Domain.Entities;
using SociePolar.Infrastructure.DataContext;

namespace SociePolar.Infrastructure.Repositories
{
    public class AccionistaRepository(IDbContextFactory<SociedadDbContext> contextFactory) : IAccionista
    {
        private readonly IDbContextFactory<SociedadDbContext> _contextFactory = contextFactory;

        public async Task<List<Accionista>> GetAllAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Accionista>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.Sociedad.EstatusSociedad)
                .Include(b => b.EstatusAccionista)
                .Include(b => b.TipoAccionista)
                .Include(b => b.DirigidoA)
                .Include(b => b.EstadoCivil)
                .Include(b => b.Banco)
                .Include(b => b.TipoCuenta)
                .Include(b => b.CondicionEspecial)
                .ToListAsync();
        }

        public async Task<Accionista?> GetByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Accionista>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.Sociedad.EstatusSociedad)
                .Include(b => b.EstatusAccionista)
                .Include(b => b.TipoAccionista)
                .Include(b => b.DirigidoA)
                .Include(b => b.EstadoCivil)
                .Include(b => b.Banco)
                .Include(b => b.TipoCuenta)
                .Include(b => b.CondicionEspecial)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Accionista>> GetBySociedadIdAsync(int sociedadId)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Accionista>()
                .Include(b => b.Sociedad)
                .Include(b => b.Sociedad.Empresa)
                .Include(b => b.Sociedad.EstatusSociedad)
                .Include(b => b.EstatusAccionista)
                .Include(b => b.TipoAccionista)
                .Include(b => b.DirigidoA)
                .Include(b => b.EstadoCivil)
                .Include(b => b.Banco)
                .Include(b => b.TipoCuenta)
                .Include(b => b.CondicionEspecial)
                .Where(x => x.Sociedad.Id == sociedadId)
                .ToListAsync();
        }

        public async Task AddAsync(AccionistaDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            var tipoAccionista = await context.Set<TipoAccionista>().FindAsync(entity.TipoAccionistaId);
            if (tipoAccionista == null) throw new Exception($"Tipo de accionista con ID {entity.TipoAccionistaId} no existe.");

            var estatusAccionista = await context.Set<EstatusAccionista>().FindAsync(entity.EstatusAccionistaId);
            if (estatusAccionista == null) throw new Exception($"Estatus de accionista con ID {entity.EstatusAccionistaId} no existe.");

            DirigidoA dirigidoa = null;
            if (entity.DirigidoAId != null)
            {
                if (entity.DirigidoAId != 0)
                {
                    dirigidoa = await context.Set<DirigidoA>().FindAsync(entity.DirigidoAId.Value);
                    if (dirigidoa == null) throw new Exception($"DirigidoA con ID {entity.DirigidoAId.Value} no existe.");
                }
            }

            EstadoCivil estadocivil = null;
            if (entity.EstadoCivilId != null)
            {
                if (entity.EstadoCivilId != 0)
                {
                    estadocivil = await context.Set<EstadoCivil>().FindAsync(entity.EstadoCivilId.Value);
                    if (estadocivil == null) throw new Exception($"Estado Civil con ID {entity.EstadoCivilId.Value} no existe.");
                }
            }

            Banco banco = null;
            if (entity.BancoId != null)
            {
                if (entity.BancoId != 0)
                {
                    banco = await context.Set<Banco>().FindAsync(entity.BancoId.Value);
                    if (banco == null) throw new Exception($"Banco con ID {entity.BancoId.Value} no existe.");
                }
            }

            TipoCuenta tipocuenta = null;
            if (entity.TipoCuentaId != null)
            {
                if (entity.TipoCuentaId != 0)
                {
                    tipocuenta = await context.Set<TipoCuenta>().FindAsync(entity.TipoCuentaId.Value);
                    if (tipocuenta == null) throw new Exception($"Tipo de Cuenta con ID {entity.TipoCuentaId.Value} no existe.");
                }
            }

            CondicionEspecial condicionespecial = null;
            if (entity.CondicionEspecialId != null)
            {
                if (entity.CondicionEspecialId != 0)
                {
                    condicionespecial = await context.Set<CondicionEspecial>().FindAsync(entity.CondicionEspecialId.Value);
                    if (condicionespecial == null) throw new Exception($"Condición Especial con ID {entity.CondicionEspecialId.Value} no existe.");
                }
            }

            Accionista newAccionista = new()
            {
                Sociedad = sociedad,
                TipoAccionista = tipoAccionista,
                EstatusAccionista = estatusAccionista,
                Nombre = entity.Nombre,
                Cedula = entity.Cedula,
                FechaEmision = entity.FechaEmision,
                FechaVencimiento = entity.FechaVencimiento,
                OtroDocumento = entity.OtroDocumento,
                FechaEmisionOtro = entity.FechaEmisionOtro,
                FechaVencimientoOtro = entity.FechaVencimientoOtro,
                Rif = entity.Rif,
                FechaEmisionRif = entity.FechaEmisionRif,
                FechaVencimientoRif = entity.FechaVencimientoRif,
                DirigidoA = dirigidoa,
                EstadoCivil = estadocivil,
                NombreConyuge = entity.NombreConyuge,
                CedulaConyuge = entity.CedulaConyuge,
                FechaEmisionConyuge = entity.FechaEmisionConyuge,
                FechaVencimientoConyuge = entity.FechaVencimientoConyuge,
                OtroDocumentoConyuge = entity.OtroDocumentoConyuge,
                FechaEmisionOtroConyuge = entity.FechaEmisionOtroConyuge,
                FechaVencimientoOtroConyuge = entity.FechaVencimientoOtroConyuge,
                SeparacionBienes = entity.SeparacionBienes,
                FechaNacimiento = entity.FechaNacimiento,
                FechaIngreso = entity.FechaIngreso,
                Email1 = entity.Email1,
                Email2 = entity.Email2,
                Email3 = entity.Email3,
                TelefonoMovil = entity.TelefonoMovil,
                Telefono1 = entity.Telefono1,
                Telefono2 = entity.Telefono2,
                Telefono3 = entity.Telefono3,
                Telefono4 = entity.Telefono4,
                Direccion1 = entity.Direccion1,
                Direccion2 = entity.Direccion2,
                GrupoFamiliar = entity.GrupoFamiliar,
                Nacionalidad = entity.Nacionalidad,
                DomiciliadoEn = entity.DomiciliadoEn,
                Banco = banco,
                NumeroCuenta = entity.NumeroCuenta,
                TipoCuenta = tipocuenta,
                NombreTitularCuenta = entity.NombreTitularCuenta,
                UltimaActualizacion = entity.UltimaActualizacion,
                AnoActualizacion = entity.AnoActualizacion,
                FaltaActualizar = entity.FaltaActualizar,
                TieneApoderado = entity.TieneApoderado,
                NombreApoderado = entity.NombreApoderado,
                DatosPoder = entity.DatosPoder,
                CedulaApoderado = entity.CedulaApoderado,
                FechaEmisionApoderado = entity.FechaEmisionApoderado,
                FechaVencimientoApoderado = entity.FechaVencimientoApoderado,
                OtroDocumentoApoderado = entity.OtroDocumentoApoderado,
                FechaEmisionOtroApoderado = entity.FechaEmisionOtroApoderado,
                FechaVencimientoOtroApoderado = entity.FechaVencimientoOtroApoderado,
                NombreContacto = entity.NombreContacto,
                TelefonoContacto = entity.TelefonoContacto,
                EmailContacto = entity.EmailContacto,
                CondicionEspecial = condicionespecial,
                Observaciones = entity.Observaciones,
                DocumentosRelacionados = entity.DocumentosRelacionados,
                AnoDuracion = entity.AnoDuracion,
                JuntaDirectiva = entity.JuntaDirectiva,
                VigenciaJunta = entity.VigenciaJunta,
                FechaVencimientoJunta = entity.FechaVencimientoJunta,
                RegistradaEn = entity.RegistradaEn,
                NombreSusesion = entity.NombreSusesion,
                CedulaSusesion = entity.CedulaSusesion,
                FechaEmisionSucesion = entity.FechaEmisionSucesion,
                FechaVencimientoSucesion = entity.FechaVencimientoSucesion,
                OtroDocumentoSucesion = entity.OtroDocumentoSucesion,
                FechaEmisionOtroSucesion= entity.FechaEmisionOtroSucesion,
                FechaVencimientoOtroSucesion = entity.FechaVencimientoOtroSucesion,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
                UpdateUserId = entity.UpdateUserId,
            };

            await context.Set<Accionista>().AddAsync(newAccionista);
            context.Entry(newAccionista.Sociedad).State = EntityState.Unchanged;
            context.Entry(newAccionista.TipoAccionista).State = EntityState.Unchanged;
            context.Entry(newAccionista.EstatusAccionista).State = EntityState.Unchanged;
            if (dirigidoa != null) context.Entry(newAccionista.DirigidoA).State = EntityState.Unchanged;
            if(estadocivil != null) context.Entry(newAccionista.EstadoCivil).State = EntityState.Unchanged;
            if(banco != null) context.Entry(newAccionista.Banco).State = EntityState.Unchanged;
            if(tipocuenta != null) context.Entry(newAccionista.TipoCuenta).State = EntityState.Unchanged;
            if(condicionespecial != null) context.Entry(newAccionista.CondicionEspecial).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(newAccionista.Sociedad).State = EntityState.Detached;
            context.Entry(newAccionista.TipoAccionista).State = EntityState.Detached;
            context.Entry(newAccionista.EstatusAccionista).State = EntityState.Detached;
            if (dirigidoa != null) context.Entry(newAccionista.DirigidoA).State = EntityState.Detached;
            if(estadocivil != null) context.Entry(newAccionista.EstadoCivil).State = EntityState.Detached;
            if(banco != null) context.Entry(newAccionista.Banco).State = EntityState.Detached;
            if(tipocuenta != null) context.Entry(newAccionista.TipoCuenta).State = EntityState.Detached;
            if(condicionespecial != null) context.Entry(newAccionista.CondicionEspecial).State = EntityState.Detached;
        }

        public async void Update(AccionistaDto entity)
        {
            using var context = _contextFactory.CreateDbContext();

            var sociedad = await context.Set<Sociedad>().FindAsync(entity.SociedadId);
            if (sociedad == null) throw new Exception($"Sociedad con ID {entity.SociedadId} no existe.");

            var tipoAccionista = await context.Set<TipoAccionista>().FindAsync(entity.TipoAccionistaId);
            if (tipoAccionista == null) throw new Exception($"Tipo de accionista con ID {entity.TipoAccionistaId} no existe.");

            var estatusAccionista = await context.Set<EstatusAccionista>().FindAsync(entity.EstatusAccionistaId);
            if (estatusAccionista == null) throw new Exception($"Estatus de accionista con ID {entity.EstatusAccionistaId} no existe.");

            Accionista editaccionista = await context.Accionistas.FindAsync(entity.Id);
            if (editaccionista == null) throw new Exception($"Accionista con ID {entity.Id} no existe.");

            DirigidoA dirigidoa = null;
            if (entity.DirigidoAId != null)
            {
                if (entity.DirigidoAId != 0)
                {
                    dirigidoa = await context.Set<DirigidoA>().FindAsync(entity.DirigidoAId.Value);
                    if (dirigidoa == null) throw new Exception($"DirigidoA con ID {entity.DirigidoAId.Value} no existe.");
                }
            }

            EstadoCivil estadocivil = null;
            if (entity.EstadoCivilId != null)
            {
                if (entity.EstadoCivilId != 0)
                {
                    estadocivil = await context.Set<EstadoCivil>().FindAsync(entity.EstadoCivilId.Value);
                    if (estadocivil == null) throw new Exception($"Estado Civil con ID {entity.EstadoCivilId.Value} no existe.");
                }
            }

            Banco banco = null;
            if (entity.BancoId != null)
            {
                if (entity.BancoId != 0)
                {
                    banco = await context.Set<Banco>().FindAsync(entity.BancoId.Value);
                    if (banco == null) throw new Exception($"Banco con ID {entity.BancoId.Value} no existe.");
                }
            }

            TipoCuenta tipocuenta = null;
            if (entity.TipoCuentaId != null)
            {
                if (entity.TipoCuentaId != 0)
                {
                    tipocuenta = await context.Set<TipoCuenta>().FindAsync(entity.TipoCuentaId.Value);
                    if (tipocuenta == null) throw new Exception($"Tipo de Cuenta con ID {entity.TipoCuentaId.Value} no existe.");
                }
            }

            CondicionEspecial condicionespecial = null;
            if (entity.CondicionEspecialId != null)
            {
                if (entity.CondicionEspecialId != 0)
                {
                    condicionespecial = await context.Set<CondicionEspecial>().FindAsync(entity.CondicionEspecialId.Value);
                    if (condicionespecial == null) throw new Exception($"Condición Especial con ID {entity.CondicionEspecialId.Value} no existe.");
                }
            }

            editaccionista.Sociedad = sociedad;
            editaccionista.TipoAccionista = tipoAccionista;
            editaccionista.EstatusAccionista = estatusAccionista;
            editaccionista.Nombre = entity.Nombre;
            editaccionista.Cedula = entity.Cedula;
            editaccionista.FechaEmision = entity.FechaEmision;
            editaccionista.FechaVencimiento = entity.FechaVencimiento;
            editaccionista.OtroDocumento = entity.OtroDocumento;
            editaccionista.FechaEmisionOtro = entity.FechaEmisionOtro;
            editaccionista.FechaVencimientoOtro = entity.FechaVencimientoOtro;
            editaccionista.Rif = entity.Rif;
            editaccionista.FechaEmisionRif = entity.FechaEmisionRif;
            editaccionista.FechaVencimientoRif = entity.FechaVencimientoRif;
            editaccionista.DirigidoA = dirigidoa;
            editaccionista.EstadoCivil = estadocivil;
            editaccionista.NombreConyuge = entity.NombreConyuge;
            editaccionista.CedulaConyuge = entity.CedulaConyuge;
            editaccionista.FechaEmisionConyuge = entity.FechaEmisionConyuge;
            editaccionista.FechaVencimientoConyuge = entity.FechaVencimientoConyuge;
            editaccionista.OtroDocumentoConyuge = entity.OtroDocumentoConyuge;
            editaccionista.FechaEmisionOtroConyuge = entity.FechaEmisionOtroConyuge;
            editaccionista.FechaVencimientoOtroConyuge = entity.FechaVencimientoOtroConyuge;
            editaccionista.SeparacionBienes = entity.SeparacionBienes;
            editaccionista.FechaNacimiento = entity.FechaNacimiento;
            editaccionista.FechaIngreso = entity.FechaIngreso;
            editaccionista.Email1 = entity.Email1;
            editaccionista.Email2 = entity.Email2;
            editaccionista.Email3 = entity.Email3;
            editaccionista.TelefonoMovil = entity.TelefonoMovil;
            editaccionista.Telefono1 = entity.Telefono1;
            editaccionista.Telefono2 = entity.Telefono2;
            editaccionista.Telefono3 = entity.Telefono3;
            editaccionista.Telefono4 = entity.Telefono4;
            editaccionista.Direccion1 = entity.Direccion1;
            editaccionista.Direccion2 = entity.Direccion2;
            editaccionista.GrupoFamiliar = entity.GrupoFamiliar;
            editaccionista.Nacionalidad = entity.Nacionalidad;
            editaccionista.DomiciliadoEn = entity.DomiciliadoEn;
            editaccionista.Banco = banco;
            editaccionista.NumeroCuenta = entity.NumeroCuenta;
            editaccionista.TipoCuenta = tipocuenta;
            editaccionista.NombreTitularCuenta = entity.NombreTitularCuenta;
            editaccionista.UltimaActualizacion = entity.UltimaActualizacion;
            editaccionista.AnoActualizacion = entity.AnoActualizacion;
            editaccionista.FaltaActualizar = entity.FaltaActualizar;
            editaccionista.TieneApoderado = entity.TieneApoderado;
            editaccionista.NombreApoderado = entity.NombreApoderado;
            editaccionista.DatosPoder = entity.DatosPoder;
            editaccionista.CedulaApoderado = entity.CedulaApoderado;
            editaccionista.FechaEmisionApoderado = entity.FechaEmisionApoderado;
            editaccionista.FechaVencimientoApoderado = entity.FechaVencimientoApoderado;
            editaccionista.OtroDocumentoApoderado = entity.OtroDocumentoApoderado;
            editaccionista.FechaEmisionOtroApoderado = entity.FechaEmisionOtroApoderado;
            editaccionista.FechaVencimientoOtroApoderado = entity.FechaVencimientoOtroApoderado;
            editaccionista.NombreContacto = entity.NombreContacto;
            editaccionista.TelefonoContacto = entity.TelefonoContacto;
            editaccionista.EmailContacto = entity.EmailContacto;
            editaccionista.CondicionEspecial = condicionespecial;
            editaccionista.Observaciones = entity.Observaciones;
            editaccionista.DocumentosRelacionados = entity.DocumentosRelacionados;
            editaccionista.AnoDuracion = entity.AnoDuracion;
            editaccionista.JuntaDirectiva = entity.JuntaDirectiva;
            editaccionista.VigenciaJunta = entity.VigenciaJunta;
            editaccionista.FechaVencimientoJunta = entity.FechaVencimientoJunta;
            editaccionista.RegistradaEn = entity.RegistradaEn;
            editaccionista.NombreSusesion = entity.NombreSusesion;
            editaccionista.CedulaSusesion = entity.CedulaSusesion;
            editaccionista.FechaEmisionSucesion = entity.FechaEmisionSucesion;
            editaccionista.FechaVencimientoSucesion = entity.FechaVencimientoSucesion;
            editaccionista.OtroDocumentoSucesion = entity.OtroDocumentoSucesion;
            editaccionista.FechaEmisionOtroSucesion = entity.FechaEmisionOtroSucesion;
            editaccionista.FechaVencimientoOtroSucesion = entity.FechaVencimientoOtroSucesion;
            editaccionista.UpdateDate = DateTime.UtcNow;
            editaccionista.UpdateUserId = entity.UpdateUserId;

            context.Set<Accionista>().Update(editaccionista);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<Accionista>().Find(id);
            if (entity != null)
            {
                context.Set<Accionista>().Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
