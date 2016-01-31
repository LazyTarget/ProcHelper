namespace ProcHelper
{
    public class MoveMouseBy : ServiceStack.IReturn<MoveMouseResponse>
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
