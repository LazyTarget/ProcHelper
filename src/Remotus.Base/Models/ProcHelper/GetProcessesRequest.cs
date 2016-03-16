namespace FullCtrl.Base
{
    public class GetProcessesRequest : IReturn<ProcessesResponse>
    {
        public string Name { get; set; }
    }
}