using System.Diagnostics;
using System.Threading;

namespace FullCtrl
{
    public class ResourceMonitor
    {
        private PerformanceCounter _totalCpu;

        public object GetCurrentCpuUsage()
        {
            if (_totalCpu == null)
            {
                _totalCpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                var f1 = _totalCpu.NextValue();
                Thread.Sleep(1000);
            }
            var f2 = _totalCpu.NextValue();
            return f2;
        }
    }
}
