using Microsoft.AspNetCore.Mvc.Rendering;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace PLPSOFT.ERP.WebApp.Models;

public class ProductViewModel
{
    public long ProductID { get; set; }
    public long CompanyID { get; set; }
    public string ProductCode { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public long? CategoryID { get; set; }
    public long ProductTypeID { get; set; }
    public long BaseUnitID { get; set; }
    public decimal CostPrice { get; set; }
    public decimal StandardPrice { get; set; }
    public bool IsActive { get; set; }
    public Company? Company { get; set; }
    public ProductCategory? Category { get; set; }

    // Danh sách chọn (Dropdown)
    public IEnumerable<SelectListItem>? Companies { get; set; }
    public IEnumerable<SelectListItem>? Categories { get; set; }
    public IEnumerable<SelectListItem>? ProductTypes { get; set; }
    public IEnumerable<SelectListItem>? Units { get; set; }
}