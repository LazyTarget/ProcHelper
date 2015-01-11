using System;
using System.Diagnostics;

namespace ProcHelper
{
    public class ProcessDto
    {
        private readonly Process _process;

        public ProcessDto(Process process)
        {
            _process = process;
        }

        public Process GetBase()
        {
            return _process;
        }


        public int Id
        {
            get { return _process.Id; }
        }

        public string ProcessName
        {
            get { return _process.TryGetProp(x => x.ProcessName); }
        }

        public string MachineName
        {
            get { return _process.TryGetProp(x => x.MachineName); }
        }

        public ProcessStartInfoDto StartInfo
        {
            get { return new ProcessStartInfoDto(_process.StartInfo); }
        }

        public DateTime StartTime
        {
            get { return _process.TryGetProp(x => x.StartTime); }
        }
        
        public bool Responding
        {
            get { return _process.TryGetProp(x => x.Responding); }
        }

        public bool HasExited
        {
            get { return _process.TryGetProp(x => x.HasExited); }
        }
        
        public int ExitCode
        {
            get { return HasExited ? _process.TryGetProp(x => x.ExitCode) : 0; }
        }

        public DateTime ExitTime
        {
            get { return HasExited ? _process.TryGetProp(x => x.ExitTime) : DateTime.MinValue; }
        }

        public TimeSpan PrivilegedProcessorTime
        {
            get { return _process.TryGetProp(x => x.PrivilegedProcessorTime); }
        }

        public TimeSpan TotalProcessorTime
        {
            get { return _process.TryGetProp(x => x.TotalProcessorTime); }
        }

        public TimeSpan UserProcessorTime
        {
            get { return _process.TryGetProp(x => x.UserProcessorTime); }
        }

        public ProcessModuleDto MainModule
        {
            get
            {
                return _process.TryGetProp(x => x.MainModule) != null ? new ProcessModuleDto(_process.MainModule) : null;
            }
        }
    }
}
