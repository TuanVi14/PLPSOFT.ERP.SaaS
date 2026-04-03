namespace PLPSOFT.ERP.Domain.Entities.MasterData;

using System.ComponentModel.DataAnnotations;

public class ProductCategory
    {
    // Đổi từ ProductCategoryID thành CategoryID để khớp với file SQL bạn đã chạy
    [Key]
    public long CategoryID { get; set; }
        public long CompanyID { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
