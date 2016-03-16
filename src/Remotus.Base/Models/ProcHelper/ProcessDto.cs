using System;
using System.Diagnostics;

namespace FullCtrl.Base
{
    public class ProcessDto : IProcessDto
    {
        private readonly Process _process;
        private int _id;
        private string _processName;
        private string _machineName;
        private ProcessStartInfoDto _startInfo;
        private DateTime _startTime;
        private string _standardOutput;
        private string _standardError;
        private bool _responding;
        private bool _hasExited;
        private int _exitCode;
        private DateTime _exitTime;
        private TimeSpan _privilegedProcessorTime;
        private TimeSpan _totalProcessorTime;
        private TimeSpan _userProcessorTime;
        private ProcessModuleDto _mainModule;
        private int _mainWindowHandle;
        private string _mainWindowTitle;


        public ProcessDto()
        {
            _exitTime = DateTime.MinValue;
        }

        public ProcessDto(Process process)
            : this()
        {
            _process = process;
        }

        public Process GetBase()
        {
            return _process;
        }


        public int Id
        {
            get { return _process?.Id ?? _id; }
            set { _id = value; }
        }

        public string ProcessName
        {
            get { return _process?.TryGetProp(x => x.ProcessName) ?? _processName; }
            set { _processName = value; }
        }

        public string MachineName
        {
            get { return _process?.TryGetProp(x => x.MachineName) ?? _machineName; }
            set { _machineName = value; }
        }

        public ProcessStartInfoDto StartInfo
        {
            get
            {
                if (_process != null)
                    return new ProcessStartInfoDto(_process.StartInfo);
                return _startInfo;
            }
            set { _startInfo = value; }
        }

        public DateTime StartTime
        {
            get { return _process?.TryGetProp(x => x.StartTime) ?? _startTime; }
            set { _startTime = value; }
        }
        
        public bool Responding
        {
            get { return _process?.TryGetProp(x => x.Responding) ?? _responding; }
            set { _responding = value; }
        }

        public bool HasExited
        {
            get { return _process?.TryGetProp(x => x.HasExited) ?? _hasExited; }
            set { _hasExited = value; }
        }
        
        public int ExitCode
        {
            get { return HasExited ? _process?.TryGetProp(x => x.ExitCode) ?? _exitCode : _exitCode; }
            set { _exitCode = value; }
        }

        public DateTime ExitTime
        {
            get { return HasExited ? _process?.TryGetProp(x => x.ExitTime) ?? _exitTime : _exitTime; }
            set { _exitTime = value; }
        }

        public TimeSpan PrivilegedProcessorTime
        {
            get { return _process?.TryGetProp(x => x.PrivilegedProcessorTime) ?? _privilegedProcessorTime; }
            set { _privilegedProcessorTime = value; }
        }

        public TimeSpan TotalProcessorTime
        {
            get { return _process?.TryGetProp(x => x.TotalProcessorTime) ?? _totalProcessorTime; }
            set { _totalProcessorTime = value; }
        }

        public TimeSpan UserProcessorTime
        {
            get { return _process?.TryGetProp(x => x.UserProcessorTime) ?? _userProcessorTime; }
            set { _userProcessorTime = value; }
        }

        public string MainWindowTitle
        {
            get { return _process?.TryGetProp(x => x.MainWindowTitle) ?? _mainWindowTitle; }
            set { _mainWindowTitle = value; }
        }

        public int MainWindowHandle
        {
            get { return _process?.TryGetProp(x => x.MainWindowHandle.ToInt32()) ?? _mainWindowHandle; }
            set { _mainWindowHandle = value; }
        }

        public ProcessModuleDto MainModule
        {
            get
            {
                var mainModule = _process?.TryGetProp(x => x.MainModule);
                if (mainModule != null)
                    return new ProcessModuleDto(mainModule);
                return _mainModule;
            }
            set { _mainModule = value; }
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
            set { _standardOutput = value; }
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
            set { _standardError = value; }
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
