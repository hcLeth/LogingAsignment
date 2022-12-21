using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogComponent.Interfaces;

namespace LogComponent
{
    public class AsyncLogInterface : ILogInterface, IDisposable
    {
        private readonly List<LogLine> _lines = new();
        
        private readonly ILineWriter _lineWriter;
        private readonly StreamWriter _writer;
        private readonly IFileCreator _fileCreator;
        private readonly IDateTimeProvider _dateTimeProvider;
        private  CancellationTokenSource _cancellationTokenSource;

        private bool _exit;
        private bool _quitWithFlush = false;
        private DateTime _curDate;

        public AsyncLogInterface(ILineWriter lineWriter, IFileCreator fileCreator,
            StreamWriter writer, IDateTimeProvider dateTimeProvider)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            
            _lineWriter = lineWriter;
            _writer = writer;
            _dateTimeProvider = dateTimeProvider;
            _fileCreator = fileCreator;

            _curDate = _dateTimeProvider.GetCurrentTime();
            if (!Directory.Exists(@"C:\LogTest")) 
                Directory.CreateDirectory(@"C:\LogTest");

            
            _writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

            _writer.AutoFlush = true;

            Task.Run(async () => await MainLoop(_cancellationTokenSource.Token));
        }
        

        private async Task MainLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && !_exit)
            {
                if (_lines.Count <= 0) continue;
                var handled = new List<LogLine>();

                for (var index = 0; index < _lines.Count; index++)
                {
                    var logLine = _lines[index];

                    if (_exit && !_quitWithFlush) continue;
                    handled.Add(logLine);

                    var stringBuilder = new StringBuilder();
                    if ((_dateTimeProvider.GetCurrentTime() - _curDate).Days != 0)
                    {
                        _curDate = _dateTimeProvider.GetCurrentTime();
                        _fileCreator.CreateWriteableFile(stringBuilder);
                    }

                    await _lineWriter.WriteLine(stringBuilder, logLine);
                }

                foreach (var t in handled)
                {
                    _lines.Remove(t);
                }

                if (_quitWithFlush && _lines.Count == 0) 
                    _exit = true;

                await Task.Delay(5000);
            }
        }

        public void StopWithoutFlush()
        {
            _exit = true;
        }

        public void StopWithFlush()
        {
            _quitWithFlush = true;
        }

        public void WriteLog(string s)
        {
             _lines.Add(new LogLine() { Text = s, Timestamp = _dateTimeProvider.GetCurrentTime() });
        }

        public void Dispose()
        {
            _writer?.Dispose();
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}