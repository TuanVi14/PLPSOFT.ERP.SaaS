using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLPSOFT.ERP.Domain.Entities.MasterData;

[Table("ProductUnits")]
public class ProductUnit
{
    [Key]
    [System.ComponentModel.DataAnnotations.Schema.Column("UnitID")]
    public long ProductUnitID { get; set; }

    public string UnitCode { get; set; } = string.Empty;
    public string UnitName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public long UnitTypeID { get; set; }
}