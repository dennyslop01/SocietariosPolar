using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Domain.Dtos
{
    public class CertificacionDto
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Sociedad válida.")] 
        public int SociedadId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Cargo válido.")] 
        public int CargoId { get; set; }
        [Required(ErrorMessage = "La Fecha es requerida.")] 
        public DateTime? Fecha { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
