using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class SystemTypeValue
    {
        public long TypeValueId { get; set; }

        public long TypeId { get; set; }

        public string ValueCode { get; set; } = null!;

        public string ValueName { get; set; } = null!;

        public int SortOrder { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public string? ExtraData { get; set; }

        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

        public virtual ICollection<ProductUnit> ProductUnits { get; set; } = new List<ProductUnit>();

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

        public virtual SystemType Type { get; set; } = null!;
    }
}
