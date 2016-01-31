namespace ProcHelper
{
    public class MoveMouseResponse
    {
        public ServiceStack.IReturn<MoveMouseResponse> Request { get; set; }

        public Point Position { get; set; }
    }
}