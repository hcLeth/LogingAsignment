using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LogComponent;
using LogComponent.Interfaces;

namespace Application
{
    internal abstract class Program
    {
        private static async Task Main(string[] args)
        {
            var fileNameGenerator = new FileNameGenerator();
            var writer = new StreamWriter(fileNameGenerator.GenerateFileName());
            var fileCreator = new NewLogFileCreator(writer);
            var lineWriter = new LineWriter(writer);
            var dateTimeProvider = new DateTimeProvider();
            
            
            ILogInterface  logger = new AsyncLogInterface(lineWriter, fileCreator, writer, dateTimeProvider);

            for (var i = 0; i < 15; i++)
            {
                logger.WriteLog("Number with Flush: " + i);
            }

            logger.StopWithFlush();
            
            var fileNameGenerator2 = new FileNameGenerator();
            var writer2 = new StreamWriter(fileNameGenerator2.GenerateFileName());
            var fileCreator2 = new NewLogFileCreator(writer2);
            var lineWriter2 = new LineWriter(writer2);

            using (var logger2 = new AsyncLogInterface(lineWriter2, fileCreator2, writer2, dateTimeProvider))
            {
                for (var i = 50; i > 0; i--)
                {
                    await Task.Delay(20);
                    logger2.WriteLog("Number with No flush: " + i);
                }
                logger2.StopWithoutFlush();
            }
            Console.ReadLine();
        }
    }
    
}
