using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class SystemType
    {
        public long TypeId { get; set; }

        public string TypeCode { get; set; } = null!;

        public string TypeName { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<SystemTypeValue> SystemTypeValues { get; set; } = new List<SystemTypeValue>();
    }

}
