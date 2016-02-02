using WindowsInput.Native;

namespace FullCtrl.Base
{
    public class IsKeyUp : IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}
