namespace Remotus.Base
{
    public class KillProcessResponse
    {
        public bool HasExited
        {
            get { return Process != null && Process.HasExited; }
        }

        public ProcessDto Process { get; set; }

    }
}