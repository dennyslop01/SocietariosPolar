using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Dtos
{
    public class AccionistaMostrarDto
    {
        public int Id { get; set; }
        public string? TipoAccionistaV { get; set; }
        public string? Nombre { get; set; }
        public string? TipoDocumentoV { get; set; }
        public string? Documento { get; set; }
        public string? Telefono { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class AccionistaDto
    {
        public int Id { get; set; }
        public int? TipoAccionistaId { get; set; }
        public string? Nombre { get; set; }
        public int? TipoDocumento1Id { get; set; }
        public int? TipoDocumento2Id { get; set; }
        public int? TipoDocumento3Id { get; set; }
        public int? TipoDocumento4Id { get; set; }
        public string? Documento1 { get; set; }
        public string? Documento2 { get; set; }
        public string? Documento3 { get; set; }
        public string? Documento4 { get; set; }
        public DateTime? FechaEmision1 { get; set; }
        public DateTime? FechaVencimiento1 { get; set; }
        public DateTime? FechaEmision2 { get; set; }
        public DateTime? FechaVencimiento2 { get; set; }
        public DateTime? FechaEmision3 { get; set; }
        public DateTime? FechaVencimiento3 { get; set; }
        public DateTime? FechaEmision4 { get; set; }
        public DateTime? FechaVencimiento4 { get; set; }
        public int? DirigidoAId { get; set; }
        public int? EstadoCivilId { get; set; }
        public string? NombreConyuge { get; set; }
        public int? TipoDocumentoConyugeId1 { get; set; }
        public int? TipoDocumentoConyugeId2 { get; set; }
        public int? TipoDocumentoConyugeId3 { get; set; }
        public string? DocumentoConyuge1 { get; set; }
        public DateTime? FechaEmisionConyuge1 { get; set; }
        public DateTime? FechaVencimientoConyuge1 { get; set; }
        public string? DocumentoConyuge2 { get; set; }
        public DateTime? FechaEmisionConyuge2 { get; set; }
        public DateTime? FechaVencimientoConyuge2 { get; set; }
        public string? DocumentoConyuge3 { get; set; }
        public DateTime? FechaEmisionConyuge3 { get; set; }
        public DateTime? FechaVencimientoConyuge3 { get; set; }
        public int? SeparacionBienes { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaIngreso { get; set; }

        [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo debe contener un dominio válido (ej. .com).")]
        public string? Email1 { get; set; }

        [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo debe contener un dominio válido (ej. .com).")]
        public string? Email2 { get; set; }

        [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo debe contener un dominio válido (ej. .com).")]
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
        public int? TipoDocumentoApoderadoId1 { get; set; }
        public int? TipoDocumentoApoderadoId2 { get; set; }
        public int? TipoDocumentoApoderadoId3 { get; set; }
        public string? DocumentoApoderado1 { get; set; }
        public DateTime? FechaEmisionApoderado1 { get; set; }
        public DateTime? FechaVencimientoApoderado1 { get; set; }
        public string? DocumentoApoderado2 { get; set; }
        public DateTime? FechaEmisionApoderado2 { get; set; }
        public DateTime? FechaVencimientoApoderado2 { get; set; }
        public string? DocumentoApoderado3 { get; set; }
        public DateTime? FechaEmisionApoderado3 { get; set; }
        public DateTime? FechaVencimientoApoderado3 { get; set; }
        public string? NombreContacto { get; set; }
        public string? TelefonoContacto { get; set; }

        [RegularExpression(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo debe contener un dominio válido (ej. .com).")]
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
