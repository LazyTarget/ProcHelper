using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProcHelper
{
    public class ProcessFinder : IProcessFinder
    {
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
