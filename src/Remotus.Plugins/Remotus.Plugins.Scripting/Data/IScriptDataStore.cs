using System.Collections.Generic;
using Remotus.Base.Scripting;

namespace Remotus.Plugins.Scripting
{
    public interface IScriptDataStore
    {
        IEnumerable<Script> GetScripts();
    }
}