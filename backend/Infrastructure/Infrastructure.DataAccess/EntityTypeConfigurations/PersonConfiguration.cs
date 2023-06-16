using Domain.Core.Contracts;
using Domain.Core.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityTypeConfigurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.PersonId);
        builder.HasOne(p => p.FullName);
        builder.Property(p => p.ContactInfo);

        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<Customer>("Customer")
            .HasValue<Courier>("Courier");
    }
}