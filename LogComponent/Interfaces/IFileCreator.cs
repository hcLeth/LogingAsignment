using System.Text;

namespace LogComponent.Interfaces;

public interface IFileCreator
{
    void CreateWriteableFile(StringBuilder fileName);
}