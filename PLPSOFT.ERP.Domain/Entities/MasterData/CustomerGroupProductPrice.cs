using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
        public class CustomerGroupProductPrice
    {
        public long GroupPriceId { get; set; }

        public long CustomerGroupId { get; set; }

        public long ProductId { get; set; }

        public decimal? Price { get; set; }

        public decimal? DiscountRate { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public bool IsActive { get; set; }
    }
}
