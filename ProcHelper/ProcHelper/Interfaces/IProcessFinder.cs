using System.Collections.Generic;

namespace ProcHelper
{
    public interface  IProcessFinder
    {
        List<ProcessDto> GetProcesses();
        List<ProcessDto> GetProcessesByName(string processName);
    }
}
