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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
