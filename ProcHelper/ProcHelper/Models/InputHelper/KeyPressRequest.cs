using WindowsInput.Native;

namespace ProcHelper
{
    public class KeyPressRequest : ServiceStack.IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}
