using System.Collections.Generic;

namespace Remotus.Base
{
    public interface  IProcessFinder
    {
        ProcessDto GetProcess(int processID);
        IEnumerable<ProcessDto> GetProcesses();
        IEnumerable<ProcessDto> GetProcessesByName(string processName);
    }
}
