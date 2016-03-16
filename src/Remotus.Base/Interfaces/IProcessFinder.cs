using System.Collections.Generic;

namespace FullCtrl.Base
{
    public interface  IProcessFinder
    {
        ProcessDto GetProcess(int processID);
        IEnumerable<ProcessDto> GetProcesses();
        IEnumerable<ProcessDto> GetProcessesByName(string processName);
    }
}
