using System.Text;
using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.DataAccess.ValueConverters;

public class ContactInfoValueConverter : ValueConverter<IEnumerable<ContactInfo>, string>
{
    public ContactInfoValueConverter()
        : base(x => ConvertTo(x), x => ConvertFrom(x))
    {
    }

    private static string ConvertTo(IEnumerable<ContactInfo> c)
    {
        return c.Select(x => x.Contact)
            .Aggregate((current, next) => current + $"{next}")
            .ToString()
            .Trim();
    }

    private static IEnumerable<ContactInfo> ConvertFrom(string c)
    {
        return c.Split(' ')
            .Select(name => new ContactInfo(name))
            .ToList();
    }
}