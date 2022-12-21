using LogComponent.Interfaces;

namespace LoggerTests;

public class FakeFileNameGenerator : IFileNameGenerator
{
    public string GenerateFileName()
    {
        return @"C:\LogTest\Log" + "test.log";
    }
}