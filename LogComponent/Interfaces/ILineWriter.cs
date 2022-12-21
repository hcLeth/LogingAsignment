using System.Text;
using System.Threading.Tasks;

namespace LogComponent.Interfaces;

public interface ILineWriter
{
    Task WriteLine(StringBuilder stringBuilder, LogLine logLine);
}