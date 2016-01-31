namespace ProcHelper
{
    public class MoveMouseTo : ServiceStack.IReturn<MoveMouseResponse>
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
