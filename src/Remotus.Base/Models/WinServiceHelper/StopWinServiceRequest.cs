namespace Remotus.Base
{
    public class StopWinServiceRequest : IReturn<StopWinServiceResponse>
    {
        public string ServiceName { get; set; }
    }
}