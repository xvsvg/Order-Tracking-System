using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OTS.Domain.Domain.Core.Implementations;

namespace OTS.Persistence.EntityTypeConfigurations;

public class CourierConfiguration : IEntityTypeConfiguration<Courier>
{
    public void Configure(EntityTypeBuilder<Courier> builder)
    {
        builder.HasOne(c => c.Customer)
            .WithOne()
            .HasForeignKey<Customer>(c => c.PersonId);
    }
}