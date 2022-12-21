using LogComponent;
using LogComponent.Interfaces;

namespace LoggerTests;

public class Tests
{
    private ILogInterface logger;

    [SetUp]
    public void Setup()
    {
        // Method intentionally left empty.
    }
    
    
    [Test]
    public async Task TestLogWritesSomethingWhenCalled()
    {
        // Arrange
        var memoryStream = new MemoryStream();
        memoryStream.Position = 0;
        var writer = new StreamWriter(memoryStream);
        var lineWriter = new FakeLineWriter();
        var fileCreator = new NewLogFileCreator(writer);
        var dateTimeProvider = new DateTimeProvider();

        // Act
        logger = new AsyncLogInterface(lineWriter, fileCreator, writer, dateTimeProvider);

        logger.WriteLog("test");

        // Assert
        using (var reader = new StreamReader(memoryStream))
        {
            memoryStream.Position = 0;
            var file = await reader.ReadToEndAsync();

            Assert.Multiple(() =>
            {
                Assert.That(file, Is.Not.Empty);
                Assert.That(file, Does.Contain("Timestamp"));
                Assert.That(file, Does.Contain("Data"));

                Assert.That(lineWriter.Lines, Is.Not.Empty);

                Assert.That(lineWriter.Lines[0], Is.EqualTo("test"));
            });

            logger.StopWithFlush();
        }
    }
    
    

    [Test]
    public async Task TestFileCreatorCalledOnDateChange()
    {
        // Arrange
        var memoryStream = new MemoryStream();
        memoryStream.Position = 0;
        var writer = new StreamWriter(memoryStream);
        var lineWriter = new FakeLineWriter();
        var dateTimeProvider = new FakeDateTimeProvider();
        var fileCreator = new FakeFileCreator();

        // Act
        logger = new AsyncLogInterface(lineWriter, fileCreator, writer, dateTimeProvider);

        for (var i = 5; i > 0; i--)
        {
            logger.WriteLog("test");
            await Task.Delay(20);
        }

        // Assert
        memoryStream.Position = 0;

            Assert.Multiple(() =>
            {
                Assert.That(fileCreator.HasBeenCalled, Is.True);
                Assert.That(lineWriter.Lines[0], Is.EqualTo("test"));
            });
            logger.StopWithoutFlush();
    }
    
    [Test]
    public async Task TestFileCreatorNotCalledWhenNoDateChange()
    {
        // Arrange
        var memoryStream = new MemoryStream();
        memoryStream.Position = 0;
        var writer = new StreamWriter(memoryStream);
        var lineWriter = new FakeLineWriter();
        var dateTimeProvider = new FakeSameDateTimeProvider();
        var fileCreator = new FakeFileCreator();

        // Act
        logger = new AsyncLogInterface(lineWriter, fileCreator, writer, dateTimeProvider);

        for (var i = 5; i > 0; i--)
        {
            logger.WriteLog("test");
            await Task.Delay(20);
        }

        // Assert
        memoryStream.Position = 0;

        Assert.Multiple(() =>
        {
            Assert.That(fileCreator.HasBeenCalled, Is.False);
            Assert.That(lineWriter.Lines[0], Is.EqualTo("test"));
        });
        logger.StopWithoutFlush();
    }
    


    [Test]
    public Task TestStopWithFlush()
    {
        // Arrange
        var memoryStream = new MemoryStream();
        memoryStream.Position = 0;
        var writer = new StreamWriter(memoryStream);
        var lineWriter = new FakeLineWriter();
        var fileCreator = new NewLogFileCreator(writer);
        var dateTimeProvider = new DateTimeProvider();

        // Act
        logger = new AsyncLogInterface(lineWriter, fileCreator, writer, dateTimeProvider);

        for (var i = 0; i < 15; i++)
        {
            logger.WriteLog("test");
        }

        logger.StopWithFlush();

        // Assert

        Assert.Multiple(() =>
            {
                Assert.That(lineWriter.Lines, Is.Not.Empty);
                Assert.That(lineWriter.Lines, Has.Count.EqualTo(15));

                Assert.That(lineWriter.Lines[0], Is.EqualTo("test"));
            });
        return Task.CompletedTask;
    }

    [Test]
    public async Task TestStopWithoutFlush()
    {
        // Arrange
        var memoryStream = new MemoryStream();
        memoryStream.Position = 0;
        var writer = new StreamWriter(memoryStream);
        var lineWriter = new FakeLineWriter();
        var fileCreator = new NewLogFileCreator(writer);
        var dateTimeProvider = new DateTimeProvider();

        // Act
        logger = new AsyncLogInterface(lineWriter, fileCreator, writer, dateTimeProvider);

        for (var i = 50; i > 0; i--)
        {
            await Task.Delay(2);
            logger.WriteLog("Number with No flush: " + i);
            
        }

        logger.StopWithoutFlush();

        // Assert
        Assert.Multiple(() =>
        {

            Assert.That(lineWriter.Lines, Is.Not.Empty);
            Assert.That(lineWriter.Lines, Has.Count.LessThan(50));
        });
    }
}
