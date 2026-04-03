using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace PLPSOFT.ERP.Infrastructure.Persistence.Configurations.MasterData
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(x => x.CustomerID);

            builder.Property(x => x.CustomerCode)
                   .HasMaxLength(20)
                   .IsUnicode(false)
                   .IsRequired();

            builder.Property(x => x.CustomerName)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.Phone)
                   .HasMaxLength(50)
                   .IsUnicode(false);

            builder.Property(x => x.Email)
                   .HasMaxLength(100)
                   .IsUnicode(false);

            builder.Property(x => x.TaxCode)
                   .HasMaxLength(50)
                   .IsUnicode(false);

            builder.Property(x => x.CreditLimit)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("sysdatetime()");

            builder.HasIndex(x => new { x.CompanyID, x.CustomerCode })
                   .IsUnique();

            builder.HasOne(x => x.CustomerGroup)
                   .WithMany(x => x.Customers)
                   .HasForeignKey(x => x.CustomerGroupID);
        }
    }
}
