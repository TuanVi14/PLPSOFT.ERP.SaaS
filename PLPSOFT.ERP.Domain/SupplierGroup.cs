namespace PLPSOFT.ERP.Sales.SaaS.V2026.Models;
public class SupplierGroup
{
    public long SupplierGroupID { get; set; }
    public long CompanyID { get; set; }

    public string GroupCode { get; set; }
    public string GroupName { get; set; }

    public bool IsActive { get; set; }

    // Navigation
    public List<Supplier> Suppliers { get; set; }
}