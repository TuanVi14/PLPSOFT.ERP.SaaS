using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace src.PLPSOFT.ERP.Infrastructure.Persistence.Configurations.MasterData
{
    public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.ToTable("CustomerAddresses");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AddressLine)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.City)
                .HasMaxLength(200);

            builder.Property(x => x.District)
                .HasMaxLength(200);

            builder.Property(x => x.Ward)
                .HasMaxLength(200);

            builder.Property(x => x.Country)
                .HasMaxLength(100);

            builder.Property(x => x.PostalCode)
                .HasMaxLength(20);

            builder.Property(x => x.IsPrimary)
                .HasDefaultValue(false);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.CustomerId);
        }
    }
}
