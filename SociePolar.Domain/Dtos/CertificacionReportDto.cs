using System;

namespace SociePolar.Domain.Dtos
{
    public class CertificacionReportDto
    {
        public int TipoAccionistaId { get; set; } // 1 = Natural, 2 = Jurídica, 3 = Sucesión

        // Datos Sociedad
        public int SociedadId { get; set; }
        public string SociedadNombre { get; set; } = string.Empty;
        public string SociedadNombreCorto { get; set; } = string.Empty;
        public string SociedadRif { get; set; } = string.Empty;
        public string SociedadDomicilio { get; set; } = "Caracas";
        public string SociedadDatosConstitucion { get; set; } = string.Empty;

        // Datos Secretario / Custodio
        public string SecretarioNombre { get; set; } = "Patricia Hernández R.";
        public string SecretarioNacionalidad { get; set; } = "venezolana";
        public string SecretarioCedula { get; set; } = "V-10.533.569";
        public string SecretarioCargo { get; set; } = "Secretario de la Junta Directiva";

        // Datos Accionista
        public int AccionistaId { get; set; }
        public string AccionistaNombre { get; set; } = string.Empty;
        public string AccionistaNacionalidad { get; set; } = "venezolano";
        public string AccionistaDocumento { get; set; } = string.Empty;
        public string AccionistaDomicilio { get; set; } = "Caracas";
        public string AccionistaRegistro { get; set; } = string.Empty;

        // Datos Sucesión
        public string ExpedienteSucesion { get; set; } = string.Empty;
        public string RifSucesion { get; set; } = string.Empty;

        // Acciones y Financiero
        public long NroAcciones { get; set; }
        public string NroAccionesEnLetras { get; set; } = string.Empty;
        public decimal ValorNominal { get; set; }
        public string ValorNominalEnLetras { get; set; } = string.Empty;
        public decimal PorcentajeAcciones { get; set; }
        public string FechaBalance { get; set; } = string.Empty;
        public decimal ValorPatrimonial { get; set; }
        public string ValorPatrimonialEnLetras { get; set; } = string.Empty;

        // Fecha de emisión de la certificación
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public string DiaEnLetras { get; set; } = string.Empty;
        public int Dia { get; set; }
        public string MesNombre { get; set; } = string.Empty;
        public string AnoEnLetras { get; set; } = string.Empty;
        public int Ano { get; set; }
    }
}
