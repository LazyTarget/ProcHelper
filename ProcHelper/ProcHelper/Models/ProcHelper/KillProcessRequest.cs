namespace ProcHelper
{
    public class KillProcessRequest : ServiceStack.IReturn<KillProcessResponse>
    {
        public int ProcessID { get; set; }
    }
}