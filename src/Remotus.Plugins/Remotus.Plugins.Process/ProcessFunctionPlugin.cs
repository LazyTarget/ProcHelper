using System.Collections.Generic;
using Remotus.Base;

namespace Remotus.Plugins.Process
{
    public class ProcessFunctionPlugin : IFunctionPlugin
    {
        public string Name => nameof(ProcessFunctionPlugin);
        public string Version => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new GetProcessFunction();
            yield return new GetProcessesFunction();
            yield return new StartProcessFunction();
            yield return new KillProcessFunction();
        }
    }
}
