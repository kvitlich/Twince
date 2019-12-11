using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
