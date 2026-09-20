using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Domain.Entities
{
    public class AccionistaSociedadHistorico
    {
        public int Id { get; set; }
        public Sociedad? Sociedad { get; set; }
        public Accionista? Accionista { get; set; }
        public Int64? NroAcciones { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
    }
}
