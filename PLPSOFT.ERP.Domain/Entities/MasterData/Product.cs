using PLPSOFT.ERP.Domain.Entities.MasterData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public long ProductID { get; set; }

        [Column("CompanyID")]
        public long CompanyID { get; set; }

        public string ProductCode { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public long? CategoryID { get; set; }

        public long BaseUnitID { get; set; }

        public decimal StandardPrice { get; set; }

        // THÊM DÒNG NÀY: Để logic Xóa mềm hoạt động và hết lỗi Build
        public bool IsDelete { get; set; } = false;

        public bool IsActive { get; set; } = true;

        [ForeignKey("CategoryID")]
        public virtual ProductCategory? Category { get; set; }

        [ForeignKey("BaseUnitID")]
        public virtual ProductUnit? Unit { get; set; }

        public long? ProductTypeID { get; set; }
    }
}