using WindowsInput.Native;

namespace FullCtrl.Base
{
    public class KeyResponse
    {
        public IReturn<KeyResponse> Request { get; set; }

        public VirtualKeyCode VirtualKeyCode { get; set; }

        public bool IsDown { get; set; }
    }
}