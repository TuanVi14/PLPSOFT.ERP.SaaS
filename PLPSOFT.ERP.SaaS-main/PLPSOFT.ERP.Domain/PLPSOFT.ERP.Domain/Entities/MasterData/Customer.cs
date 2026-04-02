using System;
using System.Collections.Generic;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TaxCode { get; set; }
        public Guid? CustomerGroupId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public CustomerGroup CustomerGroup { get; set; }
        public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
    }
}
