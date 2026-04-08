using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class Customer
    {
        public long CustomerId { get; set; }

        public long CompanyId { get; set; }

        public string CustomerCode { get; set; } = null!;

        public string CustomerName { get; set; } = null!;

        public long CustomerTypeId { get; set; }

        public long? CustomerGroupId { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? TaxCode { get; set; }

        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Facebook { get; set; }

        public string? Zalo { get; set; }

        public decimal LoyaltyPoint { get; set; }

        public decimal CreditLimit { get; set; }

        public int PaymentTermDays { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public bool IsDeleted { get; set; }

        public string? ExtraData { get; set; }

        public virtual Company Company { get; set; } = null!;

        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; } = new List<CustomerAddress>();

        public virtual CustomerGroup? CustomerGroup { get; set; }

        public virtual SystemTypeValue CustomerType { get; set; } = null!;
    }
}
