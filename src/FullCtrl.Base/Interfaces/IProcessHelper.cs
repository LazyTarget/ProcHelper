using System;
using System.Collections.Generic;

namespace FullCtrl.Base
{
    public interface IProcessHelper : IProcessFinder
    {
        //List<ProcessDto> GetProcesses();
        //List<ProcessDto> GetProcessesByName(string processName);
        ProcessDto StartProcess(string fileName);
        ProcessDto StartProcess(string fileName, string arguments);
        ProcessDto StartProcess(string fileName, string arguments, string workingDirectory);
        ProcessDto StartProcess(string fileName, string arguments, string workingDirectory, bool redirectStOutput);
        ProcessDto StartProcess(string fileName, string arguments, string workingDirectory, bool redirectStOutput, Credentials credentials);
        ProcessDto SwitchToMainWindow(IntPtr mainWindowHandle);
        ProcessDto KillProcess(int processID);
        bool IsRunning(string processName);
    }
}