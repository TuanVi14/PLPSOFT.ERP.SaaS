using PLPSOFT.ERP.Domain.Entities.MasterData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Products")]
public class Product
{
    [Key]
    public long ProductID { get; set; }
    [Column("CompanyID")] // <--- ÉP KIỂU TƯỜNG MINH: Chỉ định chính xác tên cột trong DB
    public long CompanyID { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public long? CategoryID { get; set; }

    public long BaseUnitID { get; set; } // Khớp với cột trong ảnh DB

    public decimal StandardPrice { get; set; }

    [ForeignKey("CategoryID")]
    public virtual ProductCategory? Category { get; set; }

    [ForeignKey("BaseUnitID")] // <--- Rất quan trọng để không sinh ra cột 'ProductUnitID' ảo
    public virtual ProductUnit? Unit { get; set; }
    public long? ProductTypeID { get; set; }
}