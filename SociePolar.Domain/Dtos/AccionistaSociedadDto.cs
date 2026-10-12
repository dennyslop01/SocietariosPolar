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
        public int? CustodioId { get; set; }
        public Int64? NroAcciones { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }

    public class AccionistaSociedadVerDto
    {
        public int Id { get; set; }
        public int SociedadId { get; set; }
        public int AccionistaId { get; set; }
        public string? DocumentoIdentidad { get; set; }
        public string? SociedadNombre { get; set; }
        public string? AccionistaNombre { get; set; }
        public string? TipoAccionistaNombre { get; set; }
        public Int64? NroAcciones { get; set; }
        public decimal? Participacion { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        // Propiedades de UI para el Custodio
        public bool EsCustodio { get; set; }
        public bool MostrarCustodiados { get; set; }
        public List<AccionistaSociedadVerDto> SubCustodiados { get; set; } = new();
        public string SubSearchTerm { get; set; } = "";
        public int SubCurrentPage { get; set; } = 1;
        public int SubPageSize { get; set; } = 25;

        public IEnumerable<AccionistaSociedadVerDto> SubCustodiadosFiltrados
        {
            get
            {
                // Optimización: si el término está vacío o tiene menos de 2 caracteres, no filtra la lista en memoria
                if (string.IsNullOrWhiteSpace(SubSearchTerm) || SubSearchTerm.Trim().Length < 2)
                    return SubCustodiados;
                var term = SubSearchTerm.Trim().ToLowerInvariant();
                return SubCustodiados.Where(x =>
                    (x.AccionistaNombre ?? "").ToLowerInvariant().Contains(term) ||
                    (x.DocumentoIdentidad ?? "").ToLowerInvariant().Contains(term));
            }
        }

        public int TotalSubPages => Math.Max(1, (int)Math.Ceiling((double)SubCustodiadosFiltrados.Count() / SubPageSize));

        public IEnumerable<AccionistaSociedadVerDto> SubCustodiadosPaginados =>
            SubCustodiadosFiltrados.Skip((SubCurrentPage - 1) * SubPageSize).Take(SubPageSize);
    }
}