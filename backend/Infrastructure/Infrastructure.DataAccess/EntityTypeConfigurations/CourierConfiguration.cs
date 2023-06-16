using Domain.Core.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityTypeConfigurations;

public class CourierConfiguration : IEntityTypeConfiguration<Courier>
{
    public void Configure(EntityTypeBuilder<Courier> builder)
    {
        builder.HasOne(c => c.Customer)
            .WithOne()
            .HasForeignKey<Customer>(c => c.PersonId);
    }
}