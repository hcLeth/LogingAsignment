using System;
using LogComponent.Interfaces;

namespace LogComponent;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetCurrentTime()
    {
        return TimeZoneInfo.ConvertTime(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"));
    }
}