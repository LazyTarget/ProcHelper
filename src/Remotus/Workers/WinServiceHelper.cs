using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using Remotus.Base;

namespace Remotus
{
    public class WinServiceHelper : IWinServiceHelper
    {
        public WinServiceDto GetService(string serviceName)
        {
            var services = GetServices().Where(x => x.ServiceName == serviceName).ToList();
            var result = services.FirstOrDefault();
            return result;
        }

        public List<WinServiceDto> GetServices()
        {
            var services = ServiceController.GetServices();
            var result = services.Select(x => new WinServiceDto(x)).ToList();
            return result;
        }

        public WinServiceDto StartService(string serviceName)
        {
            var service = GetService(serviceName);
            var controller = service.GetBase();
            controller.Start();
            return service;
        }

        public WinServiceDto StartService(string serviceName, string[] arguments)
        {
            var service = GetService(serviceName);
            var controller = service.GetBase();
            controller.Start(arguments ?? new string[0]);
            return service;
        }

        public WinServiceDto PauseService(string serviceName)
        {
            var service = GetService(serviceName);
            var controller = service.GetBase();
            controller.Pause();
            return service;
        }

        public WinServiceDto ContinueService(string serviceName)
        {
            var service = GetService(serviceName);
            var controller = service.GetBase();
            controller.Continue();
            return service;
        }

        public WinServiceDto StopService(string serviceName)
        {
            var service = GetService(serviceName);
            var controller = service.GetBase();
            controller.Stop();
            return service;
        }

        public bool IsRunning(string serviceName)
        {
            var service = GetService(serviceName);
            var res = service.Status == ServiceControllerStatus.Running;
            return res;
        }
    }
}
