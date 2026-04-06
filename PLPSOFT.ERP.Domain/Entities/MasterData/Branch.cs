using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class Branch
    {
        public long BranchId { get; set; }

        public string BranchName { get; set; } = null!;

        public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
    }
}
