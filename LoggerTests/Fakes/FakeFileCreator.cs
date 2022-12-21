using System.Text;
using LogComponent.Interfaces;

namespace LoggerTests;

public class FakeFileCreator : IFileCreator
{
    public bool HasBeenCalled { get; private set; }

    
    public void CreateWriteableFile(StringBuilder fileName)
    {
        HasBeenCalled = true;
    }
}