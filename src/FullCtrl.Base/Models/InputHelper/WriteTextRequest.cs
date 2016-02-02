namespace FullCtrl.Base
{
    public class WriteTextRequest : IReturn<WriteTextResponse>
    {
        public string Text { get; set; }
    }
}