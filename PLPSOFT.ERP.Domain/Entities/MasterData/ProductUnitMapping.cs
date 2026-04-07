using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{     
public partial class ProductUnitMapping
{
    public long ProductId { get; set; }

    public long UnitId { get; set; }

    public decimal ConversionRate { get; set; }

    public bool IsDefault { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ProductUnit Unit { get; set; } = null!;
}

}
