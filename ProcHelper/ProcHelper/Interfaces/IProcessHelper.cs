using System.Collections.Generic;

namespace ProcHelper
{
    public interface IProcessHelper
    {
        List<ProcessDto> GetProcesses();
        List<ProcessDto> GetProcesses(string processName);
        ProcessDto StartProcess(string fileName);
        ProcessDto StartProcess(string fileName, string arguments);
        bool IsRunning(string processName);
    }
}