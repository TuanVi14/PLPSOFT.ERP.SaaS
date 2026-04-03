namespace PLPSOFT.ERP.WebApp.Models
{
    public class ProductUnitViewModel
    {
        public long ProductUnitID { get; set; } // Khớp tên với Entity
        public string UnitCode { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
    }
}