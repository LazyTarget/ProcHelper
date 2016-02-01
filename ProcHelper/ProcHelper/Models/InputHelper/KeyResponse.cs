using WindowsInput.Native;

namespace ProcHelper
{
    public class KeyResponse
    {
        public ServiceStack.IReturn<KeyResponse> Request { get; set; }

        public VirtualKeyCode VirtualKeyCode { get; set; }

        public bool IsDown { get; set; }
    }
}