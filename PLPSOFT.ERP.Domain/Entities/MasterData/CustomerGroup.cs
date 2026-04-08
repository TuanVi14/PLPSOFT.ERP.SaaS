using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData;

public partial class CustomerGroup
{
    public long CustomerGroupId { get; set; }

    public long CompanyId { get; set; }

    public string GroupCode { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<CustomerGroupProductPrice> CustomerGroupProductPrices { get; set; } = new List<CustomerGroupProductPrice>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
