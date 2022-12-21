using System.Text;
using LogComponent;
using LogComponent.Interfaces;

namespace LoggerTests;

public class FakeLineWriter : ILineWriter
{
    public List<string> Lines { get; } = new List<string>();
    

    public async Task WriteLine(StringBuilder stringBuilder, LogLine logLine)
    {
        stringBuilder.Append(logLine.Text);
        Lines.Add(stringBuilder.ToString());
    }
}