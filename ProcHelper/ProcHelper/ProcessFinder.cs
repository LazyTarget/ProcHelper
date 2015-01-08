using System.Collections.Generic;
using System.Diagnostics;

namespace ProcHelper
{
    public class ProcessFinder
    {
        public IEnumerable<Process> FindProcesses()
        {
            var processes = Process.GetProcesses();
            return processes;
        }

    }
}
