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
    public class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
    {
        public void Configure(EntityTypeBuilder<TaxRate> builder)
        {
            builder.ToTable("TaxRates");

            builder.HasKey(x => x.TaxRateId);

            builder.Property(x => x.TaxCode)
                   .HasMaxLength(20)
                   .IsUnicode(false)
                   .IsRequired();

            builder.Property(x => x.TaxName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Rate)
                   .HasColumnType("decimal(5,2)");

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("sysdatetime()");

            builder.HasIndex(x => new { x.CompanyId, x.TaxCode, x.EffectiveFrom })
                   .IsUnique();
        }
    }
}
