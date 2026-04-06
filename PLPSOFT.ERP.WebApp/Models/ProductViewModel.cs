using Microsoft.AspNetCore.Mvc.Rendering;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace PLPSOFT.ERP.WebApp.Models;

public class ProductViewModel
{
    public long ProductID { get; set; }

    // ========================
    // 🔹 THÔNG TIN CƠ BẢN
    // ========================
    public long CompanyID { get; set; }
    public string ProductCode { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public long? CategoryID { get; set; }
    public long ProductTypeID { get; set; }
    public long BaseUnitID { get; set; }

    // ========================
    // 🔹 ĐỊNH DANH & PHÂN LOẠI
    // ========================
    public string? Sku { get; set; }
    public string? Barcode { get; set; }
    public string? Brand { get; set; }
    public string? Origin { get; set; }

    // ========================
    // 🔹 GIÁ & THUẾ
    // ========================
    public decimal CostPrice { get; set; }
    public decimal StandardPrice { get; set; }
    public long? DefaultTaxRateID { get; set; }

    // ========================
    // 🔹 KHO & VẬN HÀNH
    // ========================
    public bool TrackInventory { get; set; }
    public bool AllowBackorder { get; set; }

    public decimal? Weight { get; set; }
    public decimal? Volume { get; set; }

    public decimal? MinStock { get; set; }
    public decimal? MaxStock { get; set; }

    // ========================
    // 🔹 KỸ THUẬT & BẢO HÀNH
    // ========================
    public int? WarrantyMonths { get; set; }

    public bool IsSerialized { get; set; }
    public bool IsBatchManaged { get; set; }
    public bool ExpireDateRequired { get; set; }

    // ========================
    // 🔹 TRẠNG THÁI
    // ========================
    public bool IsActive { get; set; }

    // ========================
    // 🔹 NAVIGATION (OPTIONAL)
    // ========================
    public Company? Company { get; set; }
    public ProductCategory? Category { get; set; }

    // ========================
    // 🔹 DROPDOWN DATA
    // ========================
    public IEnumerable<SelectListItem>? Companies { get; set; }
    public IEnumerable<SelectListItem>? Categories { get; set; }
    public IEnumerable<SelectListItem>? ProductTypes { get; set; }
    public IEnumerable<SelectListItem>? Units { get; set; }
    public IEnumerable<SelectListItem>? TaxRates { get; set; } // 🔥 THÊM MỚI
}