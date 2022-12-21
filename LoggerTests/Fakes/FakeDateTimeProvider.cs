using LogComponent.Interfaces;

namespace LoggerTests;

public class FakeDateTimeProvider : IDateTimeProvider
{
    private int _timesCalled;
    public DateTime GetCurrentTime()
    {
        var returnTime = DateTime.Now.AddDays(_timesCalled);
        _timesCalled++;
        return returnTime;
    }
}