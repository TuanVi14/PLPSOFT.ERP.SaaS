namespace PLPSOFT.ERP.Domain.Entities.MasterData;

public partial class Supplier
{
    public long CompanyID;

    public long SupplierId { get; set; }

    public long CompanyId { get; set; }

    public string SupplierCode { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public long SupplierTypeId { get; set; }

    public long? SupplierGroupId { get; set; }

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

    public virtual Company Company { get; set; } = null!;

    public virtual SupplierGroup? SupplierGroup { get; set; }

    public virtual SystemTypeValue SupplierType { get; set; } = null!;
}

