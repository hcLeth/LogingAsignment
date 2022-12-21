using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LogComponent.Interfaces;

namespace LogComponent;

public class LineWriter : ILineWriter
{
    private readonly StreamWriter _writer;
    
    public LineWriter(StreamWriter writer = null)
    {
        _writer = writer;
    }
    
    
    public async Task WriteLine(StringBuilder stringBuilder, LogLine logLine)
    {

        stringBuilder.Append(logLine.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        stringBuilder.Append("\t");
        stringBuilder.Append(logLine.GetFormattedLineText());
        stringBuilder.Append("\t");

        stringBuilder.Append(Environment.NewLine);

        await _writer.WriteAsync(stringBuilder.ToString());
    }
}