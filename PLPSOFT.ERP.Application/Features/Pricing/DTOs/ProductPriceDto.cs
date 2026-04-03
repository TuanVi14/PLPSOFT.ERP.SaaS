using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Application.Features.Pricing.DTOs
{
    public class ProductPriceDto
    {
        public long PriceId { get; set; }
        public long ProductId { get; set; }
        public long BranchId { get; set; }
        public long CompanyId { get; set; }
        public decimal Price { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }
}
