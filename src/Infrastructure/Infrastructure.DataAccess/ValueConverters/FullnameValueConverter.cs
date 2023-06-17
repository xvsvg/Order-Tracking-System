using Domain.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.DataAccess.ValueConverters;

public class FullnameValueConverter : ValueConverter<FullName, string>
{
    public FullnameValueConverter()
        : base(x => $"{x.FirstName} {x.MiddleName} {x.LastName}",
            x => new FullName(string.Empty, string.Empty, string.Empty))
    {
    }
}