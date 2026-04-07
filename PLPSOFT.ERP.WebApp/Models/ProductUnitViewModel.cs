
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace PLPSOFT.ERP.WebApp.Models
{
    public class ProductUnitViewModel
    {
        public long UnitID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã đơn vị")]
        public string UnitCode { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập tên đơn vị")]
        public string UnitName { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn loại đơn vị")]
        public long UnitTypeID { get; set; }

        public bool IsActive { get; set; } = true;

        // Danh sách hiển thị trên Dropdown
        public IEnumerable<SelectListItem>? UnitTypeOptions { get; set; }
    }
}