namespace FullCtrl.Base
{
    public class StartProcessRequest : IReturn<StartProcessResponse>
    {
        public string FileName { get; set; }

        public string Arguments { get; set; }

        public string WorkingDirectory { get; set; }

        public bool RedirectStandardOutput { get; set; }
    }
}