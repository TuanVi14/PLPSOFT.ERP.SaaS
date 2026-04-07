namespace PLPSOFT.ERP.Domain.Entities.MasterData;

using System.ComponentModel.DataAnnotations;


public partial class ProductCategory
{
    public long CategoryId { get; set; }

    public long CompanyId { get; set; }

    public long? ParentId { get; set; }

    public string CategoryCode { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<ProductCategory> InverseParent { get; set; } = new List<ProductCategory>();

    public virtual ProductCategory? Parent { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
