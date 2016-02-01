using WindowsInput.Native;

namespace ProcHelper
{
    public class KeyDownRequest : ServiceStack.IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}
