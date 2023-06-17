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
        builder.Property(x => x.Name);

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.OrderHistory)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Courier)
            .WithMany(x => x.DeliveryList)
            .OnDelete(DeleteBehavior.SetNull);
    }
}