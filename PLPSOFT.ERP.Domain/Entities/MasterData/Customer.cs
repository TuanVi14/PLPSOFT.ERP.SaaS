namespace PLPSOFT.ERP.Domain.Entities.MasterData;
public class Customer
{
    public long CustomerID { get; set; }
    public long CompanyID { get; set; }

    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }

    public long? CustomerTypeID { get; set; }
    public long? CustomerGroupID { get; set; }

    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? TaxCode { get; set; }

    public int PaymentTermDays { get; set; }
    public decimal CreditLimit { get; set; }

    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string? ExtraData { get; set; }

    // Navigation
    public CustomerGroup CustomerGroup { get; set; }
    public List<CustomerAddress> Addresses { get; set; }
}