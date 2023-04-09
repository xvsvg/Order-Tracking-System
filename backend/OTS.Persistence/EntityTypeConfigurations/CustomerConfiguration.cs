using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OTS.Domain.Domain.Core.Implementations;

namespace OTS.Persistence.EntityTypeConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasOne(c => c.AccountCredentials)
            .WithOne()
            .HasForeignKey<Customer>(c => c.PersonId);

        builder.Navigation(c => c.OrderHistory).HasField("_orderHistory");
    }
}