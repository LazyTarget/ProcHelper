using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FullCtrl.Base;

namespace FullCtrl
{
    public class ProcessHelper : IProcessHelper
    {
        private IProcessFinder _processFinder = new ProcessFinder();




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
            var procInfo = StartProcess(fileName, arguments, null);
            return procInfo;
        }

        public ProcessDto StartProcess(string fileName, string arguments, string workingDirectory)
        {
            var procInfo = StartProcess(fileName, arguments, workingDirectory, false);
            return procInfo;
        }

        public ProcessDto StartProcess(string fileName, string arguments, string workingDirectory, bool redirectStOutput)
        {
            Credentials credentials = null;
#if DEBUG
            credentials = Credentials.Debug;
#endif

            var procInfo = StartProcess(fileName, arguments, workingDirectory, redirectStOutput, credentials);
            return procInfo;
        }

        public ProcessDto StartProcess(string fileName, string arguments, string workingDirectory, bool redirectStOutput, Credentials credentials)
        {
            var processStartInfo = new ProcessStartInfo(fileName, arguments)
            {
                WorkingDirectory = workingDirectory,
            };
            
            if (credentials != null && !credentials.IsEmpty)
            {
                processStartInfo.UserName = credentials.Username;
                processStartInfo.Password = credentials.SecurePassword;
                processStartInfo.Domain = credentials.Domain;
            }

            if (redirectStOutput)
            {
                processStartInfo.UseShellExecute = false;
                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.RedirectStandardOutput = true;
            }
            //if (Program._envChanged)
            //{
            //    // todo: Add config 'ManualSearchPaths' and ignore setting EnvironmentVar "Path"
            //    processStartInfo.UseShellExecute = false;
            //}


            this.Log().Debug("ProcessStart.UserName: " + processStartInfo.UserName);
            this.Log().Debug("ProcessStart.LoadUserProfile: " + processStartInfo.LoadUserProfile);
            this.Log().Debug("ProcessStart.CreateNoWindow: " + processStartInfo.CreateNoWindow);
            this.Log().Debug("ProcessStart.UseShellExecute: " + processStartInfo.UseShellExecute);
            this.Log().Debug("ProcessStart.WorkingDirectory: " + processStartInfo.WorkingDirectory);

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
