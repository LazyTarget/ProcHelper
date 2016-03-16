namespace FullCtrl.Base
{
    public class WriteTextResponse
    {
        public IReturn<WriteTextResponse> Request { get; set; }
        public string Text { get; set; }
    }
}