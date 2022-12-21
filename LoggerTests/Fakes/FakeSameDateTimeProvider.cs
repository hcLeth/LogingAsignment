using LogComponent.Interfaces;

namespace LoggerTests;

public class FakeSameDateTimeProvider : IDateTimeProvider
{
    private static DateTime DateTime => new DateTime(2019, 1, 1, 0, 0, 0);

    public DateTime GetCurrentTime()
    {
        return DateTime;
    }
}