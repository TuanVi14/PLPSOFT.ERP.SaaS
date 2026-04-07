namespace PLPSOFT.ERP.Domain.Entities.MasterData;

public partial class SupplierGroup
{
    public long SupplierGroupId { get; set; }

    public long CompanyId { get; set; }

    public string GroupCode { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}