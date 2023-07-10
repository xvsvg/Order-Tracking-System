using Domain.Core.Contracts;
using Infrastructure.DataAccess.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.EntityTypeConfigurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.PersonId);
        builder.Property(p => p.FullName).HasConversion<FullnameValueConverter>();
        builder.Property(p => p.ContactInfo).HasConversion<ContactInfoValueConverter>();

        builder.UseTpcMappingStrategy();
    }
}