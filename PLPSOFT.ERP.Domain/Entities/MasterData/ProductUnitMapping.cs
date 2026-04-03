using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
      public class ProductUnitMapping
    {
        public long ProductId { get; set; }

        public long UnitId { get; set; }

        public decimal ConversionRate { get; set; }

        public bool IsDefault { get; set; }
    }
}
