using Domain.Core.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityTypeConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.OrderId);
        builder.Property(x => x.DispatchDate);
        builder.Property(x => x.DeliveryDate);

        builder.HasOne(x => x.Courier)
            .WithOne()
            .HasForeignKey<Courier>(c => c.Customer!.PersonId);
    }
}