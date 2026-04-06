using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PLPSOFT.ERP.Sales.SaaS.V2026.Models
{
    public class Company
    {
        
        public long CompanyID { get; set; }

        public long CountryId { get; set; }
        public long? ProvinceId { get; set; }
        public long BaseCurrencyId { get; set; }

        public string CompanyCode { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

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

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }

        public byte[]? RowVersion { get; set; }

        // Navigation
        public ICollection<SupplierGroup> SupplierGroups { get; set; } = new List<SupplierGroup>();
        public ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
}

