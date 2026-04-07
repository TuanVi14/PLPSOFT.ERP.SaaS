using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace PLPSOFT.ERP.Infrastructure.Persistence.Configurations.MasterData
{
    public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.ToTable("CustomerAddresses");

            builder.HasKey(x => x.CustomerAddressID);

            builder.Property(x => x.AddressLine1)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.AddressLine2)
                   .HasMaxLength(200);

            builder.Property(x => x.City)
                   .HasMaxLength(100);

            builder.Property(x => x.State)
                   .HasMaxLength(100);

            builder.Property(x => x.PostalCode)
                   .HasMaxLength(50)
                   .IsUnicode(false);

            builder.Property(x => x.Country)
                   .HasMaxLength(100);

            builder.Property(x => x.Latitude)
                   .HasColumnType("decimal(9,6)")
                   .IsRequired(false);

            builder.Property(x => x.Longitude)
                   .HasColumnType("decimal(9,6)")
                   .IsRequired(false);

            builder.Property(x => x.IsDefault)
                   .HasDefaultValue(false);

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("sysdatetime()");

            builder.HasOne(x => x.Customer)
                   .WithMany(x => x.Addresses)
                   .HasForeignKey(x => x.CustomerID);
        }
    }
}
