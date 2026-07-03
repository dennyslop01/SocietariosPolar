using SociePolar.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SociePolar.Domain.Entities
{
    public class TipoDocumento : IBaseEntity
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreateUserId { get; set; }
        public int UpdateUserId { get; set; }
    }
}
