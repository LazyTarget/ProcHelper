using WindowsInput.Native;

namespace ProcHelper
{
    public class KeyUpRequest : ServiceStack.IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}
