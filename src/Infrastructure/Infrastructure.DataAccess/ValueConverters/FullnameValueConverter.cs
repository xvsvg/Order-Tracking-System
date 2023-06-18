using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.DataAccess.ValueConverters;

public class FullnameValueConverter : ValueConverter<FullName, string>
{
    public FullnameValueConverter()
        : base(x => x.ToString(), x => ConvertFromString(x))
    {
    }

    private static FullName ConvertFromString(string name)
    {
        var fullName = name.Split(' ');

        return new FullName(fullName[0], fullName[1], fullName[2]);
    }
}