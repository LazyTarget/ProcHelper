using System.Drawing;
using System.Windows.Forms;

namespace Remotus
{
    public class DeviceHelper
    {
        public Bitmap TakeScreenshot()
        {
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(printscreen);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            //printscreen.Save(@"C:\Users\Peter\Downloads\temp\printscreen.jpg", ImageFormat.Jpeg);
            return printscreen;
        }

    }
}
