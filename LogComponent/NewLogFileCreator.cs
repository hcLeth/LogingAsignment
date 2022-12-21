using System;
using System.IO;
using System.Text;
using LogComponent.Interfaces;

namespace LogComponent;

public class NewLogFileCreator : IFileCreator
{
    private StreamWriter _writer;
    public NewLogFileCreator( StreamWriter writer)
    {
        _writer = writer;
    }

    public void CreateWriteableFile(StringBuilder stringBuilder)
    {
        _writer = File.AppendText(@"C:\LogTest\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");

        _writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

        stringBuilder.Append(Environment.NewLine);

        _writer.Write(stringBuilder.ToString());

        _writer.AutoFlush = true;
    }
}