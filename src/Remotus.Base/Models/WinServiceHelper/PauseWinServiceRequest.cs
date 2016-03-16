namespace FullCtrl.Base
{
    public class PauseWinServiceRequest : IReturn<PauseWinServiceResponse>
    {
        public string ServiceName { get; set; }
    }
}