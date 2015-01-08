using System.Collections.Generic;

namespace ProcHelper
{
    public class ProcessesResponse
    {
        public ProcessesResponse()
        {
            Processes = new List<ProcessDto>();
        }

        public List<ProcessDto> Processes { get; set; }

    }
}