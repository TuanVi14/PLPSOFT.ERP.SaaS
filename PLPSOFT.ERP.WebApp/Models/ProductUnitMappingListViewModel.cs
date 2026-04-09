using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PLPSOFT.ERP.WebApp.Models
{
    // ─── LIST ──────────────────────────────────────────────────────────────────
    public class ProductUnitMappingListViewModel
    {
        public long ProductID { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string BaseUnitName { get; set; } = string.Empty;

        public IEnumerable<ProductUnitMappingRowViewModel> Mappings { get; set; }
            = new List<ProductUnitMappingRowViewModel>();
    }

    public class ProductUnitMappingRowViewModel
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public long UnitID { get; set; }
        public string UnitCode { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public decimal ConversionRate { get; set; }
        public bool IsDefault { get; set; }
    }

    // ─── CREATE / EDIT FORM ────────────────────────────────────────────────────
    public class ProductUnitMappingFormViewModel
    {
        [Required]
        public long OriginalProductID { get; set; }   // ProductID ban đầu (khi edit: khóa cũ)

        [Required]
        public long OriginalUnitID { get; set; }      // UnitID ban đầu (khi edit: khóa cũ)

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Sản phẩm")]
        public long ProductID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn đơn vị tính")]
        [Display(Name = "Đơn vị tính")]
        public long UnitID { get; set; }

        [Required(ErrorMessage = "Tỷ lệ quy đổi không được trống")]
        [Display(Name = "Tỷ lệ quy đổi")]
        [Range(0.000001, 999_999_999, ErrorMessage = "Tỷ lệ phải lớn hơn 0")]
        public decimal ConversionRate { get; set; } = 1;

        [Display(Name = "Là đơn vị mặc định")]
        public bool IsDefault { get; set; }

        // Dropdown sources
        public IEnumerable<SelectListItem> ProductOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> UnitOptions { get; set; } = new List<SelectListItem>();

        public bool IsEdit => OriginalProductID > 0 && OriginalUnitID > 0;

        // Tên hiển thị khi edit
        public string ProductDisplayName { get; set; } = string.Empty;
        public string UnitDisplayName { get; set; } = string.Empty;
    }

}
