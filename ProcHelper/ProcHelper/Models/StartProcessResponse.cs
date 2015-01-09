namespace ProcHelper
{
    public class StartProcessResponse
    {
        public StartProcessResponse()
        {
            
        }

        public bool Started
        {
            get { return Process != null && !Process.HasExited; }
        }

        public ProcessDto Process { get; set; }

    }
}