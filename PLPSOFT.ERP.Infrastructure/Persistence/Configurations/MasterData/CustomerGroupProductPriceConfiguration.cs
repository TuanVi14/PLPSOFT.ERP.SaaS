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
    public class CustomerGroupProductPriceConfiguration : IEntityTypeConfiguration<CustomerGroupProductPrice>
    {
        public void Configure(EntityTypeBuilder<CustomerGroupProductPrice> builder)
        {
            builder.ToTable("CustomerGroupProductPrices");

            builder.HasKey(x => x.GroupPriceId);

            builder.Property(x => x.Price)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.DiscountRate)
                   .HasColumnType("decimal(5,2)");

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            builder.HasIndex(x => x.CustomerGroupId);
            builder.HasIndex(x => x.ProductId);
        }
    }
}
