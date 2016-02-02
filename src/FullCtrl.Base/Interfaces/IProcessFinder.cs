using System.Collections.Generic;

namespace FullCtrl.Base
{
    public interface  IProcessFinder
    {
        ProcessDto GetProcess(int processID);
        List<ProcessDto> GetProcesses();
        List<ProcessDto> GetProcessesByName(string processName);
    }
}
