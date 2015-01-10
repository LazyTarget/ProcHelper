namespace ProcHelper
{
    public class StartProcessRequest : ServiceStack.IReturn<StartProcessResponse>
    {
        public string FileName { get; set; }

        public string Arguments { get; set; }

        public string WorkingDirectory { get; set; }
    }
}