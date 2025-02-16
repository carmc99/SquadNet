using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.LogManagement.LogReaders
{
    public interface ILogReader
    {
        event Action<string> OnLogLine;
        Task WatchAsync();
        Task UnwatchAsync();
    }
}
