namespace Remotus.Base
{
    public class KillProcessRequest : IReturn<KillProcessResponse>
    {
        public int ProcessID { get; set; }
    }
}