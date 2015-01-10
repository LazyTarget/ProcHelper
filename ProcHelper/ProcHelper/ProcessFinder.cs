using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProcHelper
{
    public class ProcessFinder : IProcessFinder
    {
        public ProcessDto GetProcess(int processID)
        {
            var process = Process.GetProcessById(processID);
            var result = new ProcessDto(process);
            return result;
        }

        public List<ProcessDto> GetProcesses()
        {
            var processes = Process.GetProcesses().ToList();
            var result = processes.Select(x => new ProcessDto(x)).ToList();
            return result;
        }


        public List<ProcessDto> GetProcessesByName(string processName)
        {
            var processes = Process.GetProcessesByName(processName).ToList();
            var result = processes.Select(x => new ProcessDto(x)).ToList();
            return result;
        }


    }
}
