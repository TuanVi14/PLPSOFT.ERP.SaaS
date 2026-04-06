namespace PLPSOFT.ERP.Sales.SaaS.V2026.Models;
public class Supplier
{
    public long SupplierID { get; set; }
    public long CompanyID { get; set; }

    public string SupplierCode { get; set; }
    public string SupplierName { get; set; }

    public long SupplierTypeID { get; set; }
    public long? SupplierGroupID { get; set; }

    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? TaxCode { get; set; }

    public int? PaymentTermDays { get; set; }
    public decimal? CreditLimit { get; set; }

    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }   

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; } 

    public string? ExtraData { get; set; }

    public SupplierGroup? SupplierGroup { get; set; }
}
