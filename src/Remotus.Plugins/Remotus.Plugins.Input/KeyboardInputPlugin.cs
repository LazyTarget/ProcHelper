using System;
using System.Collections.Generic;
using WindowsInput;
using WindowsInput.Native;
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
            yield return new InvokeKeyDownFunction.Descriptor();
            yield return new InvokeKeyUpFunction.Descriptor();
            yield return new InvokeKeyPressFunction.Descriptor();
            yield return new WriteTextFunction.Descriptor();
        }



        protected internal static KeyResponse GetKeyInfo(VirtualKeyCode virtualKeyCode)
        {
            var isDown = InputSimulator.InputDeviceState.IsKeyDown(virtualKeyCode);
            var res = new KeyResponse
            {
                VirtualKeyCode = virtualKeyCode,
                IsDown = isDown,
            };
            return res;
        }

        public void Dispose()
        {
            
        }
    }
}
