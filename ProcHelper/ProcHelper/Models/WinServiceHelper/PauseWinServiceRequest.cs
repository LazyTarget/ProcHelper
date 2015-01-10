namespace ProcHelper
{
    public class PauseWinServiceRequest : ServiceStack.IReturn<PauseWinServiceResponse>
    {
        public string ServiceName { get; set; }
    }
}