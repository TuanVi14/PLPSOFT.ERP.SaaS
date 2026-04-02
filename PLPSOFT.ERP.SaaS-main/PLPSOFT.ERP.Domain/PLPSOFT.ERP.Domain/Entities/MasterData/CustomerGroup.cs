using System;
using System.Collections.Generic;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public class CustomerGroup
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
