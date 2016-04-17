using System;
using System.Collections.Generic;
using WindowsInput;
using Remotus.Base;

namespace Remotus.Plugins.Input
{
    public class MouseInputPlugin : IFunctionPlugin
    {
        internal static readonly IInputSimulator InputSimulator = new InputSimulator();


        public string ID => "FF4BF1CE-8E10-407A-B425-E1DEC8D0A41D";
        public string Name => "Mouse";
        public string Version => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new GetMousePositionFunction.Descriptor();
            yield return new MoveMouseByFunction.Descriptor();
            yield return new MoveMouseToFunction.Descriptor();
            yield return new MoveMouseToOnVirtualDesktopFunction.Descriptor();
        }
    }
}
