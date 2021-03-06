﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Remotus.Base;

namespace Remotus
{
    public class ProcessHelper : IProcessHelper, IProcessFinder
    {
        private IProcessFinder _processFinder = new ProcessFinder();


        public ProcessDto GetProcess(int processID)
        {
            var process = _processFinder.GetProcess(processID);
            return process;
        }

        public IEnumerable<ProcessDto> GetProcesses()
        {
            var processes = _processFinder.GetProcesses();
            return processes;
        }

        public IEnumerable<ProcessDto> GetProcessesByName(string processName)
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
            //credentials = Credentials.Debug;
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

        public ProcessDto SwitchToMainWindow(IntPtr mainWindowHandle)
        {
            var hdl = mainWindowHandle.ToInt32();
            if (hdl <= 0)
            {
                return null;
            }

            var processes = GetProcesses();
            var proc = processes.FirstOrDefault(x => x.MainWindowHandle == hdl);

            var doWork = proc != null && !proc.HasExited && proc.MainWindowHandle == hdl;
            if (doWork)
            {
                try
                {
                    SwitchToThisWindow(mainWindowHandle, false);
                }
                catch (Exception ex)
                {
                    
                }
            }
            else
            {
                
            }

            return proc;
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



        [DllImport("user32.dll")]
        protected static extern void SwitchToThisWindow(IntPtr hWnd, bool turnon);
    }
}
