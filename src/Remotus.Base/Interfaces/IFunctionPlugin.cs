using System.Collections.Generic;

namespace Remotus.Base
{
    public interface IFunctionPlugin : IPlugin
    {
        // todo: more metadata?, such as logo

        IEnumerable<IFunctionDescriptor> GetFunctions();
    }
}
