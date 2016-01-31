namespace ProcHelper
{
    public class MouseInfoResponse
    {
        public ServiceStack.IReturn<MouseInfoResponse> Request { get; set; }

        public Point CursorPosition { get; set; }

        public Point ScreenSize { get; set; }

        public Point VirtualScreenSize { get; set; }

        public int ScreenCount { get; set; }
    }
}
