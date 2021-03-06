﻿using System.Collections.Generic;

namespace Remotus.Base
{
    public interface IWinServiceHelper
    {
        WinServiceDto GetService(string serviceName);
        List<WinServiceDto> GetServices();
        WinServiceDto StartService(string serviceName);
        WinServiceDto StartService(string serviceName, string[] arguments);
        WinServiceDto PauseService(string serviceName);
        WinServiceDto ContinueService(string serviceName);
        WinServiceDto StopService(string serviceName);
        bool IsRunning(string serviceName);
    }
}