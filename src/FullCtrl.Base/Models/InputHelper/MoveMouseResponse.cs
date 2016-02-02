namespace FullCtrl.Base
{
    public class MoveMouseResponse
    {
        public IReturn<MoveMouseResponse> Request { get; set; }

        public Point Position { get; set; }
    }
}