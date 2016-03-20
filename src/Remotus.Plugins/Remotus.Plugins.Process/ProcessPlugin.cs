using System.Collections.Generic;
using Remotus.Base;

namespace Remotus.Plugins.Process
{
    public class ProcessPlugin : IFunctionPlugin
    {
        public string ID => "8315E347-633E-4990-AF12-C0FFC4527485";
        public string Name => nameof(ProcessPlugin);
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
