namespace FullCtrl.Base
{
    public class StartWinServiceRequest : IReturn<StartWinServiceResponse>
    {
        public string ServiceName { get; set; }

        public string Arguments { get; set; }
    }
}