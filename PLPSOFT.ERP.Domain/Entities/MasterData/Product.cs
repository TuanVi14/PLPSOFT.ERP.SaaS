using PLPSOFT.ERP.Domain.Entities.MasterData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PLPSOFT.ERP.Domain.Entities.MasterData;
public partial class Product
{
    public long ProductId { get; set; }

    public long CompanyId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string? Sku { get; set; }

    public string? Barcode { get; set; }

    public string ProductName { get; set; } = null!;

    public long? CategoryId { get; set; }

    public long ProductTypeId { get; set; }

    public string? Brand { get; set; }

    public string? Origin { get; set; }

    public long BaseUnitId { get; set; }

    public long? DefaultTaxRateId { get; set; }

    public decimal CostPrice { get; set; }

    public decimal StandardPrice { get; set; }

    public bool TrackInventory { get; set; }

    public bool AllowBackorder { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Volume { get; set; }

    public decimal? MinStock { get; set; }

    public decimal? MaxStock { get; set; }

    public int? WarrantyMonths { get; set; }

    public bool IsSerialized { get; set; }

    public bool IsBatchManaged { get; set; }

    public bool ExpireDateRequired { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public string? ExtraData { get; set; }

    public virtual ProductUnit BaseUnit { get; set; } = null!;

    public virtual ProductCategory? Category { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<CustomerGroupProductPrice> CustomerGroupProductPrices { get; set; } = new List<CustomerGroupProductPrice>();

    public virtual TaxRate? DefaultTaxRate { get; set; }

    public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();

    public virtual SystemTypeValue ProductType { get; set; } = null!;

    public virtual ICollection<ProductUnitMapping> ProductUnitMappings { get; set; } = new List<ProductUnitMapping>();
}
