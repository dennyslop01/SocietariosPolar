using Microsoft.EntityFrameworkCore;
using SociePolar.Domain.Entities;

namespace SociePolar.Infrastructure.DataContext
{
    public class SociedadDbContext(DbContextOptions<SociedadDbContext> options) : DbContext(options)
    {
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<ClaseLibro> ClaseLibros { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<EstatusSociedad> EstatusSociedades { get; set; }
        public DbSet<NombreDiario> NombreDiarios { get; set; }
        public DbSet<Region> Regiones { get; set; }
        public DbSet<Registro> Registros { get; set; }
        public DbSet<TipoAsamblea> TiposAsambleas { get; set; }
        public DbSet<TipoReforma> TiposReformas { get; set; }
        public DbSet<TipoSociedad> TiposSociedades { get; set; }
        public DbSet<TipoSociedadActiva> TiposSociedadActivas { get; set; }
        public DbSet<UnidadNegocio> UnidadesNegocios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Sociedad> Sociedades { get; set; }
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<Autoridad> Autoridades { get; set; }
        public DbSet<Asamblea> Asambleas { get; set; }
        public DbSet<Certificacion> Certificaciones { get; set; }
        public DbSet<TipoLibro> TiposLibros { get; set; }
        public DbSet<LibroSocietario> LibrosSocietarios { get; set; }
        public DbSet<TipoAccionista> TiposAccionistas { get; set; }
        public DbSet<EstatusAccionista> EstatusAccionistas { get; set; }
        public DbSet<ModalidadPago> ModalidadesPago { get; set; }
        public DbSet<TipoCuenta> TiposCuentas { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Accionista> Accionistas { get; set; }
        public DbSet<DirigidoA> DirigidosA { get; set; }
        public DbSet<EstadoCivil> EstadosCiviles { get; set; }
        public DbSet<CondicionEspecial> CondicionesEspeciales { get; set; }
        public DbSet<TipoDocumento> TiposDocumentos { get; set; }
        public DbSet<AccionistaSociedad> AccionistasSociedades { get; set; }
        public DbSet<Titulo> Titulos { get; set; }
        public DbSet<TipoDocumentoSoporte> TiposDocumentosSoporte { get; set; }
        public DbSet<DocumentoModulo> DocumentosModulos { get; set; }
        public DbSet<DividendoPreliminar> DividendosPreliminares { get; set; }
        public DbSet<DividendoPreliminarDetalle> DividendosPreliminaresDetalles { get; set; }
        public DbSet<DividendoDefinitivo> DividendosDefinitivos { get; set; }
        public DbSet<DividendoDefinitivoDetalle> DividendosDefinitivosDetalles { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<Conciliacion> Conciliaciones { get; set; }
        public DbSet<ConciliacionDetalle> ConciliacionesDetalles { get; set; }
        public DbSet<AuditoriaNroAccion> AuditoriasNroAcciones { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sociedad>() 
                .ToTable(tb => tb.HasTrigger("trg_Sociedades_Update"));

            modelBuilder.Entity<Sociedad>() 
                .ToTable(tb => tb.HasTrigger("trg_Sociedades_Delete"));

            modelBuilder.Entity<Autoridad>() 
                .ToTable(tb => tb.HasTrigger("trg_Autoridades_Update"));

            modelBuilder.Entity<Autoridad>() 
                .ToTable(tb => tb.HasTrigger("trg_Autoridades_Delete"));

            modelBuilder.Entity<Asamblea>()
                 .ToTable(tb => tb.HasTrigger("trg_Asambleas_Update"));

            modelBuilder.Entity<Asamblea>()
                .ToTable(tb => tb.HasTrigger("trg_Asambleas_Delete"));

            modelBuilder.Entity<Certificacion>()
                 .ToTable(tb => tb.HasTrigger("trg_Certificaciones_Update"));

            modelBuilder.Entity<Certificacion>()
                .ToTable(tb => tb.HasTrigger("trg_Certificaciones_Delete"));

            modelBuilder.Entity<LibroSocietario>()
                 .ToTable(tb => tb.HasTrigger("trg_LibrosSocietarios_Update"));

            modelBuilder.Entity<LibroSocietario>()
                .ToTable(tb => tb.HasTrigger("trg_LibrosSocietarios_Delete"));

            modelBuilder.Entity<Accionista>()
                 .ToTable(tb => tb.HasTrigger("trg_Accionistas_Update"));

            modelBuilder.Entity<Accionista>()
                .ToTable(tb => tb.HasTrigger("trg_Accionistas_Delete"));

            modelBuilder.Entity<Titulo>()
                 .ToTable(tb => tb.HasTrigger("trg_Titulos_Update"));

            modelBuilder.Entity<Titulo>()
                .ToTable(tb => tb.HasTrigger("trg_Titulos_Delete"));







            modelBuilder.Entity<Sociedad>()
                .Property(e => e.ValorAccion)
                .HasPrecision(20, 10); // 10 decimales

            modelBuilder.Entity<Sociedad>()
                .Property(e => e.ValorPatrimonial)
                .HasPrecision(20, 10); // 10 decimales

            modelBuilder.Entity<DividendoPreliminar>()
                .Property(e => e.MontoPagadoTesoreria)
                .HasPrecision(18, 4); // 10 decimales

            modelBuilder.Entity<DividendoPreliminar>()
                .Property(e => e.MontoPagadoAccionistas)
                .HasPrecision(18, 4); // 10 decimales
        }
    }
}
