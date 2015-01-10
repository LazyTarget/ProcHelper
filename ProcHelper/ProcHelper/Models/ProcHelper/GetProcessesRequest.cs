namespace ProcHelper
{
    public class GetProcessesRequest : ServiceStack.IReturn<ProcessesResponse>
    {
        public string Name { get; set; }
    }
}