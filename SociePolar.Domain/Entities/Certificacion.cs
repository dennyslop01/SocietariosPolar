using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Domain.Entities
{
    public class Certificacion
    {
        public int Id { get; set; }
        public Sociedad Sociedad { get; set; }
        public Cargo Cargo { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
