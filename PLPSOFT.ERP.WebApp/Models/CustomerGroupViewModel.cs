using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PLPSOFT.ERP.WebApp.Models
{
    public class CustomerGroupViewModel
    {
        public long CustomerGroupID { get; set; }
        public long CompanyID { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public IEnumerable<SelectListItem>? Companies { get; set; }
    }
}
