namespace ProcHelper
{
    public class StartProcessResponse
    {
        public string StandardOutput { get; set; }

        public string StandardError { get; set; }
        
        public ProcessDto Process { get; set; }
    }
}