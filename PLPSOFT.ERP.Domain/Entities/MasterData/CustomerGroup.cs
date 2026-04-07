namespace PLPSOFT.ERP.Domain.Entities.MasterData;
public class CustomerGroup
{
    public long CustomerGroupID { get; set; }
    public long CompanyID { get; set; }

    public string GroupCode { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    // Navigation
    public List<Customer> Customers { get; set; } = new();
}