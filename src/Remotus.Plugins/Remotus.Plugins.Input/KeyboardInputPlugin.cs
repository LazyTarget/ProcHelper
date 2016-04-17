using System;
using System.Collections.Generic;
using WindowsInput;
using Remotus.Base;

namespace Remotus.Plugins.Input
{
    public class KeyboardInputPlugin : IFunctionPlugin
    {
        internal static readonly IInputSimulator InputSimulator = new InputSimulator();


        public string ID => "026A8610-EA80-4AA4-984A-B1B8D877A4FA";
        public string Name => "Keyboard";
        public string Version => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new GetKeyInfoFunction.Descriptor();
        }
    }
}
