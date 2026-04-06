using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLPSOFT.ERP.Domain.Entities.MasterData;

public partial class ProductUnit
{
    public long UnitId { get; set; }

    public string UnitCode { get; set; } = null!;

    public string UnitName { get; set; } = null!;

    public long UnitTypeId { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ProductUnitMapping> ProductUnitMappings { get; set; } = new List<ProductUnitMapping>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual SystemTypeValue UnitType { get; set; } = null!;
}