namespace PLPSOFT.ERP.WebApp.Models
{
    public class CategoryViewModel
    {
        public long CategoryID { get; set; } // Đổi sang long
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}