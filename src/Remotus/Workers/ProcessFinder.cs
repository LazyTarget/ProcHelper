using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Remotus.Base;

namespace Remotus
{
    public class ProcessFinder : IProcessFinder
    {
        public ProcessDto GetProcess(int processID)
        {
            var process = Process.GetProcessById(processID);
            var result = new ProcessDto(process);
            return result;
        }

        public IEnumerable<ProcessDto> GetProcesses()
        {
            var processes = Process.GetProcesses();
            var result = processes.Select(x => new ProcessDto(x));
            return result;
        }


        public IEnumerable<ProcessDto> GetProcessesByName(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            var result = processes.Select(x => new ProcessDto(x));
            return result;
        }


    }
}
