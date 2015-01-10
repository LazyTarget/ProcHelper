using System.Collections.Generic;

namespace ProcHelper
{
    public interface  IProcessFinder
    {
        ProcessDto GetProcess(int processID);
        List<ProcessDto> GetProcesses();
        List<ProcessDto> GetProcessesByName(string processName);
    }
}
