namespace Remotus.Base
{
    public class PauseWinServiceRequest : IReturn<PauseWinServiceResponse>
    {
        public string ServiceName { get; set; }
    }
}