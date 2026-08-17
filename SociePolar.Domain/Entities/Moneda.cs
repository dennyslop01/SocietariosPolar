using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Entities
{
    public class Moneda
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido.")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "El Símbolo es requerido.")]
        public string? Simbolo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
