using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class TaxRate
    {
        public long TaxRateId { get; set; }

        public long CompanyId { get; set; }

        public string TaxCode { get; set; } = null!;

        public string TaxName { get; set; } = null!;

        public decimal Rate { get; set; }

        public DateOnly EffectiveFrom { get; set; }

        public DateOnly? EffectiveTo { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Company? Company { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
