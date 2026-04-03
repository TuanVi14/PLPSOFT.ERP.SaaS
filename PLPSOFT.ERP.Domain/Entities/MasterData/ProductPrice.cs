using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public class ProductPrice
    {
        [Key] 
        public long PriceId { get; set; }

        public long ProductId { get; set; }

        public long BranchId { get; set; }

        public long CompanyId { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public decimal Price { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
