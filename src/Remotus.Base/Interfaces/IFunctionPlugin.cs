using System.Collections.Generic;

namespace FullCtrl.Base
{
    public interface IFunctionPlugin : IPlugin
    {
        // todo: more metadata?, such as logo

        IEnumerable<IFunctionDescriptor> GetFunctions();
    }
}
