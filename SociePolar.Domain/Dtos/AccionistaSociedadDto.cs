using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Dtos
{
    public class AccionistaSociedadDto
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Sociedad válida.")] 
        public int? SociedadId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Accionista válida.")] 
        public int? AccionistaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Estatus válida.")] 
        public int? EstatusAccionistaId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }

    public class AccionistaSociedadVerDto
    {
        public int Id { get; set; }
        public string? SociedadNombre { get; set; }
        public string? AccionistaNombre { get; set; }
        public string? TipoAccionistaNombre { get; set; }
        public int? NroAcciones { get; set; }
        public decimal? Participacion { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
