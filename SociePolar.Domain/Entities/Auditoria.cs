using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Domain.Entities
{
    public class Auditoria
    {
        public int Id { get; set; }
        public int ModuloId { get; set; }
        public string? Accion { get; set; }
        public string? Descripcion { get; set; }
        public int CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
