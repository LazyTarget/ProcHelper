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
            return StartProcess(fileName, null);
        }

        public ProcessDto StartProcess(string fileName, string arguments)
        {
            var processStartInfo = new ProcessStartInfo(fileName, arguments)
            {
                
            };
            var process = Process.Start(processStartInfo);

            var procInfo = new ProcessDto(process);
            return procInfo;
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
