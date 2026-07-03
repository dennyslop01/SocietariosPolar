using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Domain.Entities
{
    public class AccionistaSociedad
    {
        public int Id { get; set; }
        public Sociedad? Sociedad { get; set; }
        public EstatusAccionista? EstatusAccionista { get; set; }
        public TipoAccionista? TipoAccionista { get; set; }


        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
