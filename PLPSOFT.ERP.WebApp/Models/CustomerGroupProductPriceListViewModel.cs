using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PLPSOFT.ERP.WebApp.Models
{
    public class CustomerGroupProductPriceListViewModel
    {
        public IEnumerable<CustomerGroupProductPriceRowViewModel> Items { get; set; } = new List<CustomerGroupProductPriceRowViewModel>();
        public CustomerGroupProductPriceFilterViewModel Filter { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public class CustomerGroupProductPriceRowViewModel
    {
        public long GroupPriceID { get; set; }
        public long CustomerGroupID { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public long ProductID { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public decimal? DiscountRate { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsActive { get; set; }
        public bool IsCurrentlyValid => IsActive
                                         && EffectiveFrom <= DateTime.Now
                                         && (EffectiveTo == null || EffectiveTo >= DateTime.Now);
    }

    public class CustomerGroupProductPriceFilterViewModel
    {
        public long? CustomerGroupID { get; set; }
        public long? ProductID { get; set; }
        public bool? IsActive { get; set; }
        public string? Keyword { get; set; }
    }

    // ─── CREATE / EDIT FORM ────────────────────────────────────────────────────
    public class CustomerGroupProductPriceFormViewModel
    {
        public long GroupPriceID { get; set; }   // 0 = create

        [Required(ErrorMessage = "Vui lòng chọn nhóm khách hàng")]
        [Display(Name = "Nhóm khách hàng")]
        public long CustomerGroupID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Sản phẩm")]
        public long ProductID { get; set; }

        [Display(Name = "Giá bán riêng")]
        [Range(0, 999_999_999_999, ErrorMessage = "Giá không hợp lệ")]
        public decimal? Price { get; set; }

        [Display(Name = "Chiết khấu (%)")]
        [Range(0, 100, ErrorMessage = "Chiết khấu phải từ 0 đến 100")]
        public decimal? DiscountRate { get; set; }

        [Required(ErrorMessage = "Ngày hiệu lực không được trống")]
        [Display(Name = "Hiệu lực từ")]
        [DataType(DataType.Date)]
        public DateTime EffectiveFrom { get; set; } = DateTime.Today;

        [Display(Name = "Hiệu lực đến")]
        [DataType(DataType.Date)]
        public DateTime? EffectiveTo { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;

        // Dropdown sources
        public IEnumerable<SelectListItem> CustomerGroupOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ProductOptions { get; set; } = new List<SelectListItem>();

        public bool IsEdit => GroupPriceID > 0;

        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            if (Price == null && DiscountRate == null)
                yield return new ValidationResult(
                    "Phải nhập ít nhất Giá bán riêng hoặc Chiết khấu.",
                    new[] { nameof(Price), nameof(DiscountRate) });

            if (EffectiveTo.HasValue && EffectiveTo < EffectiveFrom)
                yield return new ValidationResult(
                    "Ngày kết thúc phải sau ngày bắt đầu.",
                    new[] { nameof(EffectiveTo) });
        }
    }

}
