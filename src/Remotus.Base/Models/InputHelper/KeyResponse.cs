using WindowsInput.Native;

namespace Remotus.Base
{
    public class KeyResponse
    {
        public IReturn<KeyResponse> Request { get; set; }

        public VirtualKeyCode VirtualKeyCode { get; set; }

        public bool IsDown { get; set; }
    }
}