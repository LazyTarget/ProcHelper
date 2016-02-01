namespace ProcHelper
{
    public class WriteTextRequest : ServiceStack.IReturn<WriteTextResponse>
    {
        public string Text { get; set; }
    }
}