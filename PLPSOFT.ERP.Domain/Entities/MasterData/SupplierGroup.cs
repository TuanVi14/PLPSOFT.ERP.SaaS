namespace PLPSOFT.ERP.Domain.Entities.MasterData;
public class SupplierGroup
{
    public long SupplierGroupID { get; set; }
    public long CompanyID { get; set; }

    public string GroupCode { get; set; }
    public string GroupName { get; set; }

    public bool IsActive { get; set; }

    public List<Supplier> Suppliers { get; set; }
}