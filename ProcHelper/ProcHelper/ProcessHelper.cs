using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProcHelper
{
    public class ProcessHelper : IProcessHelper
    {
        private ProcessFinder _processFinder = new ProcessFinder();




        public List<ProcessDto> GetProcesses()
        {
            var processes = _processFinder.GetProcesses();
            return processes;
        }

        public List<ProcessDto> GetProcesses(string processName)
        {
            var processes = _processFinder.GetProcessesByName(processName);
            return processes;
        }
        
        public ProcessDto StartProcess(string fileName)
        {
            var procInfo = StartProcess(fileName, null);
            return procInfo;
        }

        public ProcessDto StartProcess(string fileName, string arguments)
        {
            var procInfo = StartProcess(fileName, null, null);
            return procInfo;
        }

        public ProcessDto StartProcess(string fileName, string arguments, string workingDirectory)
        {
            var processStartInfo = new ProcessStartInfo(fileName, arguments)
            {
                WorkingDirectory = workingDirectory,
            };
            var process = Process.Start(processStartInfo);

            var procInfo = new ProcessDto(process);
            return procInfo;
        }

        public ProcessDto KillProcess(int processID)
        {
            var processDto = _processFinder.GetProcess(processID);
            var process = processDto.GetBase();
            process.Kill();

            return processDto;
        }


        public bool IsRunning(string processName)
        {
            var processses = _processFinder.GetProcessesByName(processName);
            if (processses != null && processses.Any())
            {
                return true;
            }
            return false;
        }


    }
}
