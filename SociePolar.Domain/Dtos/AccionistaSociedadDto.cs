using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Domain.Dtos
{
    public class AccionistaSociedadDto
    {
        public int Id { get; set; }
        public int? SociedadId { get; set; }
        public int? EstatusAccionistaId { get; set; }
        public int? TipoAccionistaId { get; set; }



        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
