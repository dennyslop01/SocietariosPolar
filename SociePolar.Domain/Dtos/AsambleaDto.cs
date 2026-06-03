using System.ComponentModel.DataAnnotations;

namespace SociePolar.Domain.Dtos
{
    public class AsambleaDto
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Sociedad válida.")] 
        public int? SociedadId { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Tipo de Asamblea válido.")]
        public int? TipoAsambleaId { get; set; }

        [Required(ErrorMessage = "La Fecha de Celebración es requerida.")] 
        public DateTime? FechaCelebracion { get; set; }

        [Required(ErrorMessage = "El Número de Acta es requerido.")] 
        public int? NumeroActa { get; set; }

        [Required(ErrorMessage = "La Fecha de Registro es requerida.")] 
        public DateTime? FechaRegistro { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Registro válido.")]
        public int? RegistroId { get; set; }

        [Required(ErrorMessage = "El Número de registro es requerido.")] 
        public int? NumeroRegistro { get; set; }
        
        [Required(ErrorMessage = "El Tomo es requerido.")] 
        public string? Tomo { get; set; }
        
        [Required(ErrorMessage = "El Año de Publicación es requerido.")] 
        public int? AnoPublicacion { get; set; }
        
        [Required(ErrorMessage = "La Fecha de Publicación es requerida.")] 
        public DateTime? FechaPublicacion { get; set; }
        
        [Required(ErrorMessage = "El Número de Publicación es requerido.")] 
        public int? NumeroPublicacion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Nombre de Diario válido.")] 
        public int? NombreDiarioId { get; set; }

        public int? IndicadorAsamblea { get; set; }

        [Required(ErrorMessage = "Aplica Reforma es requerido.")]
        public int? AplicaReforma { get; set; }

        // Quitamos el [Range] de aquí para que no valide siempre
        public int? TipoReformaId { get; set; }

        // Agregamos la lógica de validación condicional aquí
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Si el usuario selecciona "Sí" (1)
            if (AplicaReforma == 1)
            {
                // Validamos que el TipoReformaId sea mayor o igual a 1
                if (!TipoReformaId.HasValue || TipoReformaId < 1)
                {
                    yield return new ValidationResult(
                        "Debe seleccionar un Tipo de Reforma válido.",
                        new[] { nameof(TipoReformaId) }
                    );
                }
            }
        }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
