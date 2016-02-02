namespace FullCtrl.Base
{
    public class MouseInfoResponse
    {
        public IReturn<MouseInfoResponse> Request { get; set; }

        public Point CursorPosition { get; set; }

        public Point ScreenSize { get; set; }

        public Point VirtualScreenSize { get; set; }

        public int ScreenCount { get; set; }
    }
}
