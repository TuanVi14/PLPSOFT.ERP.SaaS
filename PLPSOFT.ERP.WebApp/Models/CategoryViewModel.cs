using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PLPSOFT.ERP.WebApp.Models
{
    public class CategoryViewModel
    {
        public long CategoryID { get; set; }

        [Required]
        public long CompanyID { get; set; }

        public long? ParentID { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryCode { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string CategoryName { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public IEnumerable<SelectListItem>? Companies { get; set; }
        public IEnumerable<SelectListItem>? ParentCategories { get; set; }
    }
}