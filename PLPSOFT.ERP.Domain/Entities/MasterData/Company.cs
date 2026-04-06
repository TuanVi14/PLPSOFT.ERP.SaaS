using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class Company
    {
        public long CompanyId { get; set; }

        public long CountryId { get; set; }

        public long? ProvinceId { get; set; }

        public long BaseCurrencyId { get; set; }

        public string CompanyCode { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public string? ShortName { get; set; }

        public string? LegalName { get; set; }

        public string? TaxCode { get; set; }

        public string? BusinessLicenseNo { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? AddressLine { get; set; }

        public string? PostalCode { get; set; }

        public string? LogoUrl { get; set; }

        public string? Notes { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public long? UpdatedBy { get; set; }

        public byte[] RowVersion { get; set; } = null!;

        public virtual ICollection<CustomerGroup> CustomerGroups { get; set; } = new List<CustomerGroup>();

        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public virtual ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public virtual ICollection<SupplierGroup> SupplierGroups { get; set; } = new List<SupplierGroup>();

        public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

        public virtual ICollection<TaxRate> TaxRates { get; set; } = new List<TaxRate>();
    }
}
