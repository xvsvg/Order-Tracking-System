using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OTS.Domain.Domain.Core.Contracts;
using OTS.Domain.Domain.Core.Implementations;

namespace OTS.Persistence.EntityTypeConfigurations;

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