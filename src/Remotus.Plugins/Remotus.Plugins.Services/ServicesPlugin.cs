using System.Collections.Generic;
using Remotus.Base;

namespace Remotus.Plugins.Services
{
    public class ServicesPlugin : IFunctionPlugin
    {
        public string ID        => "F396E551-818F-4490-8CC9-14754C405D72";
        public string Name      => "Services";
        public string Version   => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new GetServiceFunction.Descriptor();
            yield return new GetServicesFunction.Descriptor();
            yield return new SearchServicesFunction.Descriptor();
            yield return new StartServiceFunction.Descriptor();
            yield return new StopServiceFunction.Descriptor();
            yield return new PauseServiceFunction.Descriptor();
            yield return new ContinueServiceFunction.Descriptor();
        }
    }
}
