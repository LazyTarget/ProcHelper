using System;
using System.Diagnostics;

namespace FullCtrl.Base
{
    public class ProcessDto : IProcessDto
    {
        private readonly Process _process;
        private string _standardOutput;
        private string _standardError;


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


        public string StandardOutput
        {
            get
            {
                string res;
                try
                {
                    res = ReadStandardOutput();
                    _standardOutput += res;
                    res = _standardOutput;
                }
                catch (Exception ex)
                {
                    res = _standardOutput;
                }
                return res;
            }
        }

        public string StandardError
        {
            get
            {
                string res;
                try
                {
                    res = ReadStandardError();
                    _standardError += res;
                    res = _standardError;
                }
                catch (Exception ex)
                {
                    res = _standardError;
                }
                return res;
            }
        }


        private string ReadStandardOutput()
        {
            string res = null;
            var proc = GetBase();
            var startInfo = proc?.StartInfo;
            if (startInfo != null && startInfo.RedirectStandardOutput)
            {
                res = proc.StandardOutput.ReadToEnd();
            }
            return res;
        }

        private string ReadStandardError()
        {
            string res = null;
            var proc = GetBase();
            var startInfo = proc?.StartInfo;
            if (startInfo != null && startInfo.RedirectStandardError)
            {
                res = proc.StandardError.ReadToEnd();
            }
            return res;
        }

    }
}
