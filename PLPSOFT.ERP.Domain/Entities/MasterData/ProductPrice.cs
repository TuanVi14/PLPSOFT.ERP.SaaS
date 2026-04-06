using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class ProductPrice
    {
        public long PriceId { get; set; }

        public long ProductId { get; set; }

        public long BranchId { get; set; }

        public long CompanyId { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public decimal Price { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public virtual Branch Branch { get; set; } = null!;

        public virtual Company Company { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}
