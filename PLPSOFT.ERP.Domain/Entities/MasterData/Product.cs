using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Domain.Entities.MasterData
{
    public partial class Product
    {
        public long ProductId { get; set; }

        public long CompanyId { get; set; }

        public string ProductCode { get; set; } = null!;

        public string? Sku { get; set; }

        public string? Barcode { get; set; }

        public string ProductName { get; set; } = null!;

        public long? CategoryId { get; set; }

        public long ProductTypeId { get; set; }

        public string? Brand { get; set; }

        public string? Origin { get; set; }

        public long BaseUnitId { get; set; }

        public long? DefaultTaxRateId { get; set; }

        public decimal CostPrice { get; set; }

        public decimal StandardPrice { get; set; }

        public bool TrackInventory { get; set; }

        public bool AllowBackorder { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Volume { get; set; }

        public decimal? MinStock { get; set; }

        public decimal? MaxStock { get; set; }

        public int? WarrantyMonths { get; set; }

        public bool IsSerialized { get; set; }

        public bool IsBatchManaged { get; set; }

        public bool ExpireDateRequired { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public string? ExtraData { get; set; }

 
    }
}
