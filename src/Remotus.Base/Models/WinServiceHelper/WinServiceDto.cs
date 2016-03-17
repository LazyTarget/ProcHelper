using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using Microsoft.Win32;

namespace Remotus.Base
{
    public class WinServiceDto
    {
        private readonly ServiceController _winServiceController;
        private string _imagePath;

        public WinServiceDto(ServiceController winServiceController)
        {
            _winServiceController = winServiceController;
        }

        public ServiceController GetBase()
        {
            return _winServiceController;
        }


        public string ServiceName
        {
            get { return _winServiceController.ServiceName; }
        }

        public string DisplayName
        {
            get { return _winServiceController.DisplayName; }
        }

        public string MachineName
        {
            get { return _winServiceController.MachineName; }
        }

        public ServiceControllerStatus Status
        {
            get { return _winServiceController.Status; }
        }

        public string ImagePath
        {
            get
            {
                if (_imagePath == null)
                {
                    _imagePath = GetImagePath();
                }
                return _imagePath;
            }
        }

        public bool CanPauseAndContinue
        {
            get { return _winServiceController.CanPauseAndContinue; }
        }

        public bool CanStop
        {
            get { return _winServiceController.CanStop; }
        }

        public bool CanShutdown
        {
            get { return _winServiceController.CanShutdown; }
        }

        public ServiceType ServiceType
        {
            get { return _winServiceController.ServiceType; }
        }

        public List<WinServiceDto> ServicesDependedOn
        {
            get { return _winServiceController.ServicesDependedOn.Select(x => new WinServiceDto(x)).ToList(); }
            //get { return null; }
        }

        public List<WinServiceDto> DependentServices
        {
            //get { return _winServiceController.DependentServices.Select(x => new WinServiceDto(x)).ToList(); }
            get { return null; }        // todo: prevent infinite loops
        }


        
        private string GetImagePath()
        {
            try
            {
                string registryPath = @"SYSTEM\CurrentControlSet\Services\" + ServiceName;
                
                RegistryKey key;
                if (!string.IsNullOrEmpty(MachineName) && 
                    MachineName != "." && 
                    MachineName.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase))
                {
                    key = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, this.MachineName).OpenSubKey(registryPath);
                }
                else
                {
                    key = Registry.LocalMachine.OpenSubKey(registryPath);
                }

                var value = key.GetValue("ImagePath").ToString();
                key.Close();
                var var = ExpandEnvironmentVariables(value);
                return var;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        private string ExpandEnvironmentVariables(string path)
        {
            if (!string.IsNullOrEmpty(MachineName) &&
                MachineName != "." &&
                MachineName.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase))
            {
                var systemRootKey = @"Software\Microsoft\Windows NT\CurrentVersion\";

                var key = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, MachineName)
                                     .OpenSubKey(systemRootKey);
                var expandedSystemRoot = key.GetValue("SystemRoot").ToString();
                key.Close();

                path = path.Replace("%SystemRoot%", expandedSystemRoot);
                return path;
            }
            else
            {
                var var = Environment.ExpandEnvironmentVariables(path);
                return var;
            }
        }


        public override string ToString()
        {
            if (ServiceName != null)
            {
                var res = ServiceName;
                if (!string.IsNullOrEmpty(MachineName) && MachineName != ".")
                    res = string.Format("{0} @{1}", ServiceName, MachineName);
                res = string.Format("{0} [{1}]", res, Status);
                return res;
            }
            else
                return base.ToString();
        }

    }
}
