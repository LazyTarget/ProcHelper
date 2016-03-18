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
            yield return new GetProcessFunction.Descriptor();
            yield return new GetProcessesFunction.Descriptor();
            yield return new StartProcessFunction.Descriptor();
            yield return new KillProcessFunction.Descriptor();
        }
    }
}
