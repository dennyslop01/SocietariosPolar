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
                .Include(b => b.TipoAccionista)
                .Include(b => b.TipoDocumento1)
                .Include(b => b.TipoDocumento2)
                .Include(b => b.TipoDocumento3)
                .Include(b => b.TipoDocumento4)
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
                .Include(b => b.TipoAccionista)
                .Include(b => b.TipoDocumento1)
                .Include(b => b.TipoDocumento2)
                .Include(b => b.TipoDocumento3)
                .Include(b => b.TipoDocumento4)
                .Include(b => b.DirigidoA)
                .Include(b => b.EstadoCivil)
                .Include(b => b.Banco)
                .Include(b => b.TipoCuenta)
                .Include(b => b.CondicionEspecial)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        //public async Task<List<Accionista>> GetBySociedadIdAsync(int sociedadId)
        //{
        //    using var context = await _contextFactory.CreateDbContextAsync();
        //    return await context.Set<Accionista>()
        //        .Include(b => b.TipoAccionista)
        //        .Include(b => b.DirigidoA)
        //        .Include(b => b.EstadoCivil)
        //        .Include(b => b.Banco)
        //        .Include(b => b.TipoCuenta)
        //        .Include(b => b.CondicionEspecial)
        //        .Where(x => x.Sociedad!.Id == sociedadId)
        //        .ToListAsync();
        //}

        public async Task AddAsync(AccionistaDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var tipoAccionista = await context.Set<TipoAccionista>().FindAsync(entity.TipoAccionistaId);
            if (tipoAccionista == null) throw new Exception($"Tipo de accionista con ID {entity.TipoAccionistaId} no existe.");

            DirigidoA? dirigidoa = null;
            if (entity.DirigidoAId != null)
            {
                if (entity.DirigidoAId != 0)
                {
                    dirigidoa = await context.Set<DirigidoA>().FindAsync(entity.DirigidoAId.Value);
                    if (dirigidoa == null) throw new Exception($"DirigidoA con ID {entity.DirigidoAId.Value} no existe.");
                }
            }

            EstadoCivil? estadocivil = null;
            if (entity.EstadoCivilId != null)
            {
                if (entity.EstadoCivilId != 0)
                {
                    estadocivil = await context.Set<EstadoCivil>().FindAsync(entity.EstadoCivilId.Value);
                    if (estadocivil == null) throw new Exception($"Estado Civil con ID {entity.EstadoCivilId.Value} no existe.");
                }
            }

            Banco? banco = null;
            if (entity.BancoId != null)
            {
                if (entity.BancoId != 0)
                {
                    banco = await context.Set<Banco>().FindAsync(entity.BancoId.Value);
                    if (banco == null) throw new Exception($"Banco con ID {entity.BancoId.Value} no existe.");
                }
            }

            TipoCuenta? tipocuenta = null;
            if (entity.TipoCuentaId != null)
            {
                if (entity.TipoCuentaId != 0)
                {
                    tipocuenta = await context.Set<TipoCuenta>().FindAsync(entity.TipoCuentaId.Value);
                    if (tipocuenta == null) throw new Exception($"Tipo de Cuenta con ID {entity.TipoCuentaId.Value} no existe.");
                }
            }

            CondicionEspecial? condicionespecial = null;
            if (entity.CondicionEspecialId != null)
            {
                if (entity.CondicionEspecialId != 0)
                {
                    condicionespecial = await context.Set<CondicionEspecial>().FindAsync(entity.CondicionEspecialId.Value);
                    if (condicionespecial == null) throw new Exception($"Condición Especial con ID {entity.CondicionEspecialId.Value} no existe.");
                }
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

            TipoDocumento? tipodoc4 = null;
            if (entity.TipoDocumento4Id != null)
            {
                if (entity.TipoDocumento4Id != 0)
                {
                    tipodoc4 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento4Id.Value);
                    if (tipodoc4 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento4Id.Value} no existe.");
                }
            }

            Accionista? newAccionista = new()
            {
                TipoAccionista = tipoAccionista,
                Nombre = entity.Nombre,
                Documento1 = entity.Documento1,
                Documento2 = entity.Documento2,
                Documento3 = entity.Documento3,
                Documento4 = entity.Documento4,
                TipoDocumento1 = tipodoc1,
                TipoDocumento2 = tipodoc2,
                TipoDocumento3 = tipodoc3,
                TipoDocumento4 = tipodoc4,
                FechaEmision1 = entity.FechaEmision1,
                FechaEmision2 = entity.FechaEmision2,
                FechaEmision3 = entity.FechaEmision3,
                FechaEmision4 = entity.FechaEmision4,
                FechaVencimiento1 = entity.FechaVencimiento1,
                FechaVencimiento2 = entity.FechaVencimiento2,
                FechaVencimiento3 = entity.FechaVencimiento3,
                FechaVencimiento4 = entity.FechaVencimiento4,
                DirigidoA = dirigidoa,
                EstadoCivil = estadocivil,
                NombreConyuge = entity.NombreConyuge,
                DocumentoConyuge1 = entity.DocumentoConyuge1,
                DocumentoConyuge2 = entity.DocumentoConyuge2,
                DocumentoConyuge3 = entity.DocumentoConyuge3,
                TipoDocumentoConyugeId1 = entity.TipoDocumentoConyugeId1,
                TipoDocumentoConyugeId2 = entity.TipoDocumentoConyugeId2,
                TipoDocumentoConyugeId3 = entity.TipoDocumentoConyugeId3,
                FechaEmisionConyuge1 = entity.FechaEmisionConyuge1,
                FechaEmisionConyuge2 = entity.FechaEmisionConyuge2,
                FechaEmisionConyuge3 = entity.FechaEmisionConyuge3,
                FechaVencimientoConyuge1 = entity.FechaVencimientoConyuge1,
                FechaVencimientoConyuge2 = entity.FechaVencimientoConyuge2,
                FechaVencimientoConyuge3 = entity.FechaVencimientoConyuge3,
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
                DocumentoApoderado1 = entity.DocumentoApoderado1,
                DocumentoApoderado2 = entity.DocumentoApoderado2,
                DocumentoApoderado3 = entity.DocumentoApoderado3,
                TipoDocumentoApoderadoId1 = entity.TipoDocumentoApoderadoId1,
                TipoDocumentoApoderadoId2 = entity.TipoDocumentoApoderadoId2,
                TipoDocumentoApoderadoId3 = entity.TipoDocumentoApoderadoId3,
                FechaEmisionApoderado1 = entity.FechaEmisionApoderado1,
                FechaEmisionApoderado2 = entity.FechaEmisionApoderado2,
                FechaEmisionApoderado3 = entity.FechaEmisionApoderado3,
                FechaVencimientoApoderado1 = entity.FechaVencimientoApoderado1,
                FechaVencimientoApoderado2 = entity.FechaVencimientoApoderado2,
                FechaVencimientoApoderado3 = entity.FechaVencimientoApoderado3,
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
            context.Entry(newAccionista.TipoAccionista).State = EntityState.Unchanged;
            if (dirigidoa != null) context.Entry(newAccionista.DirigidoA!).State = EntityState.Unchanged;
            if(estadocivil != null) context.Entry(newAccionista.EstadoCivil!).State = EntityState.Unchanged;
            if(banco != null) context.Entry(newAccionista.Banco!).State = EntityState.Unchanged;
            if(tipocuenta != null) context.Entry(newAccionista.TipoCuenta!).State = EntityState.Unchanged;
            if(condicionespecial != null) context.Entry(newAccionista.CondicionEspecial!).State = EntityState.Unchanged;
            if(tipodoc1 != null) context.Entry(newAccionista.TipoDocumento1!).State = EntityState.Unchanged;
            if(tipodoc2 != null) context.Entry(newAccionista.TipoDocumento2!).State = EntityState.Unchanged;
            if(tipodoc3 != null) context.Entry(newAccionista.TipoDocumento3!).State = EntityState.Unchanged;
            if(tipodoc4 != null) context.Entry(newAccionista.TipoDocumento4!).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            context.Entry(newAccionista.TipoAccionista).State = EntityState.Detached;
            if (dirigidoa != null) context.Entry(newAccionista.DirigidoA!).State = EntityState.Detached;
            if(estadocivil != null) context.Entry(newAccionista.EstadoCivil!).State = EntityState.Detached;
            if(banco != null) context.Entry(newAccionista.Banco!).State = EntityState.Detached;
            if(tipocuenta != null) context.Entry(newAccionista.TipoCuenta!).State = EntityState.Detached;
            if(condicionespecial != null) context.Entry(newAccionista.CondicionEspecial!).State = EntityState.Detached;
            if(tipodoc1 != null) context.Entry(newAccionista.TipoDocumento1!).State = EntityState.Detached;
            if(tipodoc2 != null) context.Entry(newAccionista.TipoDocumento2!).State = EntityState.Detached;
            if(tipodoc3 != null) context.Entry(newAccionista.TipoDocumento3!).State = EntityState.Detached;
            if(tipodoc4 != null) context.Entry(newAccionista.TipoDocumento4!).State = EntityState.Detached;
        }

        public async void Update(AccionistaDto entity)
        {
            using var context = _contextFactory.CreateDbContext();

            var tipoAccionista = await context.Set<TipoAccionista>().FindAsync(entity.TipoAccionistaId);
            if (tipoAccionista == null) throw new Exception($"Tipo de accionista con ID {entity.TipoAccionistaId} no existe.");

            var editaccionista = await context.Accionistas.FindAsync(entity.Id);
            if (editaccionista == null) throw new Exception($"Accionista con ID {entity.Id} no existe.");

            DirigidoA? dirigidoa = null;
            if (entity.DirigidoAId != null)
            {
                if (entity.DirigidoAId != 0)
                {
                    dirigidoa = await context.Set<DirigidoA>().FindAsync(entity.DirigidoAId.Value);
                    if (dirigidoa == null) throw new Exception($"DirigidoA con ID {entity.DirigidoAId.Value} no existe.");
                }
            }

            EstadoCivil? estadocivil = null;
            if (entity.EstadoCivilId != null)
            {
                if (entity.EstadoCivilId != 0)
                {
                    estadocivil = await context.Set<EstadoCivil>().FindAsync(entity.EstadoCivilId.Value);
                    if (estadocivil == null) throw new Exception($"Estado Civil con ID {entity.EstadoCivilId.Value} no existe.");
                }
            }

            Banco? banco = null;
            if (entity.BancoId != null)
            {
                if (entity.BancoId != 0)
                {
                    banco = await context.Set<Banco>().FindAsync(entity.BancoId.Value);
                    if (banco == null) throw new Exception($"Banco con ID {entity.BancoId.Value} no existe.");
                }
            }

            TipoCuenta? tipocuenta = null;
            if (entity.TipoCuentaId != null)
            {
                if (entity.TipoCuentaId != 0)
                {
                    tipocuenta = await context.Set<TipoCuenta>().FindAsync(entity.TipoCuentaId.Value);
                    if (tipocuenta == null) throw new Exception($"Tipo de Cuenta con ID {entity.TipoCuentaId.Value} no existe.");
                }
            }

            CondicionEspecial? condicionespecial = null;
            if (entity.CondicionEspecialId != null)
            {
                if (entity.CondicionEspecialId != 0)
                {
                    condicionespecial = await context.Set<CondicionEspecial>().FindAsync(entity.CondicionEspecialId.Value);
                    if (condicionespecial == null) throw new Exception($"Condición Especial con ID {entity.CondicionEspecialId.Value} no existe.");
                }
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

            TipoDocumento? tipodoc4 = null;
            if (entity.TipoDocumento4Id != null)
            {
                if (entity.TipoDocumento4Id != 0)
                {
                    tipodoc4 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento4Id.Value);
                    if (tipodoc4 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento4Id.Value} no existe.");
                }
            }

            editaccionista.TipoAccionista = tipoAccionista;
            editaccionista.Nombre = entity.Nombre;
            editaccionista.Documento1 = entity.Documento1;
            editaccionista.Documento2 = entity.Documento2;
            editaccionista.Documento3 = entity.Documento3;
            editaccionista.Documento4 = entity.Documento4;
            editaccionista.TipoDocumento1 = tipodoc1;
            editaccionista.TipoDocumento2 = tipodoc2;
            editaccionista.TipoDocumento3 = tipodoc3;
            editaccionista.TipoDocumento4 = tipodoc4;
            editaccionista.FechaEmision1 = entity.FechaEmision1;
            editaccionista.FechaEmision2 = entity.FechaEmision2;
            editaccionista.FechaEmision3 = entity.FechaEmision3;
            editaccionista.FechaEmision4 = entity.FechaEmision4;
            editaccionista.FechaVencimiento1 = entity.FechaVencimiento1;
            editaccionista.FechaVencimiento2 = entity.FechaVencimiento2;
            editaccionista.FechaVencimiento3 = entity.FechaVencimiento3;
            editaccionista.FechaVencimiento4 = entity.FechaVencimiento4;
            editaccionista.DirigidoA = dirigidoa;
            editaccionista.EstadoCivil = estadocivil;
            editaccionista.NombreConyuge = entity.NombreConyuge;
            editaccionista.DocumentoConyuge1 = entity.DocumentoConyuge1;
            editaccionista.DocumentoConyuge2 = entity.DocumentoConyuge2;
            editaccionista.DocumentoConyuge3 = entity.DocumentoConyuge3;
            editaccionista.TipoDocumentoConyugeId1 = entity.TipoDocumentoConyugeId1;
            editaccionista.TipoDocumentoConyugeId2 = entity.TipoDocumentoConyugeId2;
            editaccionista.TipoDocumentoConyugeId3 = entity.TipoDocumentoConyugeId3;
            editaccionista.FechaEmisionConyuge1 = entity.FechaEmisionConyuge1;
            editaccionista.FechaEmisionConyuge2 = entity.FechaEmisionConyuge2;
            editaccionista.FechaEmisionConyuge3 = entity.FechaEmisionConyuge3;
            editaccionista.FechaVencimientoConyuge1 = entity.FechaVencimientoConyuge1;
            editaccionista.FechaVencimientoConyuge2 = entity.FechaVencimientoConyuge2;
            editaccionista.FechaVencimientoConyuge3 = entity.FechaVencimientoConyuge3;
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
            editaccionista.DocumentoApoderado1 = entity.DocumentoApoderado1;
            editaccionista.DocumentoApoderado2 = entity.DocumentoApoderado2;
            editaccionista.DocumentoApoderado3 = entity.DocumentoApoderado3;
            editaccionista.TipoDocumentoApoderadoId1 = entity.TipoDocumentoApoderadoId1;
            editaccionista.TipoDocumentoApoderadoId2 = entity.TipoDocumentoApoderadoId2;
            editaccionista.TipoDocumentoApoderadoId3 = entity.TipoDocumentoApoderadoId3;
            editaccionista.FechaEmisionApoderado1 = entity.FechaEmisionApoderado1;
            editaccionista.FechaEmisionApoderado2 = entity.FechaEmisionApoderado2;
            editaccionista.FechaEmisionApoderado3 = entity.FechaEmisionApoderado3;
            editaccionista.FechaVencimientoApoderado1 = entity.FechaVencimientoApoderado1;
            editaccionista.FechaVencimientoApoderado2 = entity.FechaVencimientoApoderado2;
            editaccionista.FechaVencimientoApoderado3 = entity.FechaVencimientoApoderado3;
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

        public async Task<Accionista?> GetByRifAsync(string tipoDoc, string rif)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Set<Accionista>()
                .Include(b => b.TipoAccionista)
                .Include(b => b.TipoDocumento1)
                .Where(x => x.TipoDocumento1.Nombre == tipoDoc && x.Documento1 == rif)
                .FirstOrDefaultAsync();
        }

        public async Task<Accionista> AddReturnEntidadAsync(AccionistaDto entity)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            TipoDocumento? tipodoc1 = null;
            if (entity.TipoDocumento1Id != null)
            {
                if (entity.TipoDocumento1Id != 0)
                {
                    tipodoc1 = await context.Set<TipoDocumento>().FindAsync(entity.TipoDocumento1Id.Value);
                    if (tipodoc1 == null) throw new Exception($"Tipo Documento con ID {entity.TipoDocumento1Id.Value} no existe.");
                }
            }

            Accionista? newAccionista = new()
            {
                Nombre = entity.Nombre,
                Documento1 = entity.Documento1,
                TipoDocumento1 = tipodoc1,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreateUserId = entity.CreateUserId,
                UpdateUserId = entity.UpdateUserId,
            };

            await context.Set<Accionista>().AddAsync(newAccionista);
            if (tipodoc1 != null) context.Entry(newAccionista.TipoDocumento1!).State = EntityState.Unchanged;

            await context.SaveChangesAsync();
            if (tipodoc1 != null) context.Entry(newAccionista.TipoDocumento1!).State = EntityState.Detached;

            return newAccionista;
        }

    }
}
