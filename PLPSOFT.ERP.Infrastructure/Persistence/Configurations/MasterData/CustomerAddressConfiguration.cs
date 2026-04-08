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
            builder.HasKey(x => x.AddressId);

            builder.Property(x => x.AddressId).HasColumnName("AddressID");
            builder.Property(x => x.CustomerId).HasColumnName("CustomerID");
            builder.Property(x => x.Address).HasMaxLength(500);
            builder.Property(x => x.ReceiverName).HasMaxLength(255);
            builder.Property(x => x.Province).HasMaxLength(100);
            builder.Property(x => x.District).HasMaxLength(100);
            builder.Property(x => x.Ward).HasMaxLength(100);
            builder.Property(x => x.Phone).HasMaxLength(30).IsUnicode(false);
            builder.Property(x => x.Note).HasMaxLength(500);
            builder.Property(x => x.Latitude).HasColumnType("decimal(18,8)").IsRequired(false);
            builder.Property(x => x.Longitude).HasColumnType("decimal(18,8)").IsRequired(false);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("(sysdatetime())");

            builder.HasOne(x => x.Customer)
                   .WithMany(x => x.CustomerAddresses)
                   .HasForeignKey(x => x.CustomerId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
