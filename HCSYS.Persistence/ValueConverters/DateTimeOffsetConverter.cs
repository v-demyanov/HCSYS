using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HCSYS.Persistence.ValueConverters;

public class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
{
    public DateTimeOffsetConverter() 
        : base(d => d.ToUniversalTime(), d => d.ToUniversalTime())
    {
    }
}
