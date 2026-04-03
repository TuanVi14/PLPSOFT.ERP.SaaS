namespace PLPSOFT.ERP.Domain.Entities.MasterData;

    public class Product
    {
        public long ProductID { get; set; }
        public long CompanyID { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public long ProductCategoryID { get; set; }
        public long BaseUnitID { get; set; }
        public decimal StandardPrice { get; set; }
        public bool IsActive { get; set; } = true;
        public int ProductTypeID { get; set; }

        // Các thuộc tính bổ sung để hiển thị tên thay vì ID trên giao diện
        public string? CategoryName { get; set; }
        public string? UnitName { get; set; }
    }
