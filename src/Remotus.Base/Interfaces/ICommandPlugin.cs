using System.Collections.Generic;

namespace Remotus.Base
{
    public interface ICommandPlugin : IPlugin
    {
        // todo: more metadata?, such as logo

        IEnumerable<ICommandDescriptor> GetCommands();
    }
}
