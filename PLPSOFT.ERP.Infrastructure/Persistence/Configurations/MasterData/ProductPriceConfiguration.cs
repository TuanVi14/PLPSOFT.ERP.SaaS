using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PLPSOFT.ERP.Domain.Entities.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLPSOFT.ERP.Infrastructure.Persistence.Configurations.MasterData
{
    public class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
    {
        public void Configure(EntityTypeBuilder<ProductPrice> builder)
        {
            builder.ToTable("ProductPrices");

            builder.HasKey(x => x.PriceId);

            builder.Property(x => x.Price)
                   .HasColumnType("decimal(18,2)");

            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => x.BranchId);
            builder.HasIndex(x => x.CompanyId);
        }
    }
}
