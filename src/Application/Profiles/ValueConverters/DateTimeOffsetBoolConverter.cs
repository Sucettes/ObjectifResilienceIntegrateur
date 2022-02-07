using System;
using AutoMapper;

namespace Gwenael.Application.Profiles.ValueConverters
{
    public class DateTimeOffsetBoolConverter : IValueConverter<DateTimeOffset?, bool>
    {
        public bool Convert(DateTimeOffset? sourceMember, ResolutionContext context)
        {
            return sourceMember.HasValue;
        }
    }
    public class BoolDateTimeOffsetConverter : IValueConverter<bool, DateTimeOffset?>
    {
        public DateTimeOffset? Convert(bool sourceMember, ResolutionContext context)
        {
            return sourceMember ? DateTimeOffset.UtcNow.AddDays(365) : null;
        }
    }


}
