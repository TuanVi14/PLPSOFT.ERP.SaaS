namespace PLPSOFT.ERP.WebApp.Models;
public class ProductViewModel
{
    public long ProductID { get; set; }
    public string ProductCode { get; set; } = "";
    public string ProductName { get; set; } = "";
    public string? CategoryName { get; set; }
    public string? UnitName { get; set; }
    public decimal StandardPrice { get; set; }
    public long? CategoryID { get; internal set; }
    public long BaseUnitID { get; set; }
    public long ProductTypeID { get; set; }
}