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
    public class ProductUnitMappingConfiguration : IEntityTypeConfiguration<ProductUnitMapping>
    {
        public void Configure(EntityTypeBuilder<ProductUnitMapping> builder)
        {
            builder.ToTable("ProductUnitMappings");

            builder.HasKey(x => new { x.ProductId, x.UnitId });

            builder.Property(x => x.ConversionRate)
                   .HasColumnType("decimal(18,6)");

            builder.HasIndex(x => x.UnitId);
        }
    }
}
