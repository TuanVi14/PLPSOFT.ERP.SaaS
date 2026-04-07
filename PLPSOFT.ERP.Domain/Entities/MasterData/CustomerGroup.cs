using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class CustomerGroup
    {
        public long CustomerGroupId { get; set; }

<<<<<<< HEAD
    public string GroupCode { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
=======
        public long CompanyId { get; set; }
>>>>>>> f4d4dcc04a18270395633e0584ae01094247ca77

        public string GroupCode { get; set; } = null!;

<<<<<<< HEAD
    // Navigation
    public List<Customer> Customers { get; set; } = new();
}
=======
        public string GroupName { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public virtual Company Company { get; set; } = null!;

        public virtual ICollection<CustomerGroupProductPrice> CustomerGroupProductPrices { get; set; } = new List<CustomerGroupProductPrice>();

        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
>>>>>>> f4d4dcc04a18270395633e0584ae01094247ca77
