using System.Collections.Generic;

namespace FullCtrl.Base
{
    public interface IProcessHelper
    {
        List<ProcessDto> GetProcesses();
        List<ProcessDto> GetProcesses(string processName);
        ProcessDto StartProcess(string fileName);
        ProcessDto StartProcess(string fileName, string arguments);
        ProcessDto StartProcess(string fileName, string arguments, string workingDirectory);
        ProcessDto StartProcess(string fileName, string arguments, string workingDirectory, bool redirectStOutput);
        ProcessDto StartProcess(string fileName, string arguments, string workingDirectory, bool redirectStOutput, Credentials credentials);
        ProcessDto KillProcess(int processID);
        bool IsRunning(string processName);
    }
}