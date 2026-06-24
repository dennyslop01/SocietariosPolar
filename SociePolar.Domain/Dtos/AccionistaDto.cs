using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Dtos
{
    public class AccionistaDto
    {
        public int Id { get; set; }
        public int? SociedadId { get; set; }
        public int? EstatusAccionistaId { get; set; }
        public int? TipoAccionistaId { get; set; }
        public string? Nombre { get; set; }
        public string? Cedula { get; set; }
        public DateTime? FechaEmision { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string? OtroDocumento { get; set; }
        public DateTime? FechaEmisionOtro { get; set; }
        public DateTime? FechaVencimientoOtro { get; set; }
        public string? Rif { get; set; }
        public DateTime? FechaEmisionRif { get; set; }
        public DateTime? FechaVencimientoRif { get; set; }
        public int? DirigidoAId { get; set; }
        public int? EstadoCivilId { get; set; }
        public string? NombreConyuge { get; set; }
        public string? CedulaConyuge { get; set; }
        public DateTime? FechaEmisionConyuge { get; set; }
        public DateTime? FechaVencimientoConyuge { get; set; }
        public string? OtroDocumentoConyuge { get; set; }
        public DateTime? FechaEmisionOtroConyuge { get; set; }
        public DateTime? FechaVencimientoOtroConyuge { get; set; }
        public int? SeparacionBienes { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaIngreso { get; set; }

        [StringLength(100, ErrorMessage = "El correo no puede exceder los 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo debe contener un dominio válido (ej. .com).")]
        public string? Email1 { get; set; }

        [StringLength(100, ErrorMessage = "El correo no puede exceder los 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo debe contener un dominio válido (ej. .com).")]
        public string? Email2 { get; set; }

        [StringLength(100, ErrorMessage = "El correo no puede exceder los 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo debe contener un dominio válido (ej. .com).")]
        public string? Email3 { get; set; }
        public string? TelefonoMovil { get; set; }
        public string? Telefono1 { get; set; }
        public string? Telefono2 { get; set; }
        public string? Telefono3 { get; set; }
        public string? Telefono4 { get; set; }
        public string? Direccion1 { get; set; }
        public string? Direccion2 { get; set; }
        public string? GrupoFamiliar { get; set; }
        public string? Nacionalidad { get; set; }
        public string? DomiciliadoEn { get; set; }
        public int? BancoId { get; set; }
        public string? NumeroCuenta { get; set; }
        public int? TipoCuentaId { get; set; }
        public string? NombreTitularCuenta { get; set; }
        public DateTime? UltimaActualizacion { get; set; }
        public int? AnoActualizacion { get; set; }
        public int? FaltaActualizar { get; set; }
        public int? TieneApoderado { get; set; }
        public string? NombreApoderado { get; set; }
        public string? DatosPoder { get; set; }
        public string? CedulaApoderado { get; set; }
        public DateTime? FechaEmisionApoderado { get; set; }
        public DateTime? FechaVencimientoApoderado { get; set; }
        public string? OtroDocumentoApoderado { get; set; }
        public DateTime? FechaEmisionOtroApoderado { get; set; }
        public DateTime? FechaVencimientoOtroApoderado { get; set; }
        public string? NombreContacto { get; set; }
        public string? TelefonoContacto { get; set; }

        [StringLength(100, ErrorMessage = "El correo no puede exceder los 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo debe contener un dominio válido (ej. .com).")]
        public string? EmailContacto { get; set; }
        public int? CondicionEspecialId { get; set; }
        public string? Observaciones { get; set; }
        public string? DocumentosRelacionados { get; set; }


        public int? AnoDuracion { get; set; }
        public string? JuntaDirectiva { get; set; }
        public int? VigenciaJunta { get; set; }
        public DateTime? FechaVencimientoJunta { get; set; }
        public string? RegistradaEn { get; set; }

        public string? NombreSusesion { get; set; }
        public string? CedulaSusesion { get; set; }
        public string? FechaEmisionSucesion { get; set; }
        public string? FechaVencimientoSucesion { get; set; }
        public string? OtroDocumentoSucesion { get; set; }
        public string? FechaEmisionOtroSucesion { get; set; }
        public string? FechaVencimientoOtroSucesion { get; set; }



        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
