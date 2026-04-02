using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PLPSOFT.ERP.Domain.Entities.MasterData;

namespace src.PLPSOFT.ERP.Infrastructure.Persistence.Configurations.MasterData
{
    public class CustomerGroupConfiguration : IEntityTypeConfiguration<CustomerGroup>
    {
        public void Configure(EntityTypeBuilder<CustomerGroup> builder)
        {
            builder.ToTable("CustomerGroups");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasMany(x => x.Customers)
                .WithOne(x => x.CustomerGroup)
                .HasForeignKey(x => x.CustomerGroupId);
        }
    }
}
