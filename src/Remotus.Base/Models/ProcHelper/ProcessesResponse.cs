using System.Collections.Generic;

namespace FullCtrl.Base
{
    public class ProcessesResponse
    {
        public ProcessesResponse()
        {
            Processes = new List<ProcessDto>();
        }

        public int ProcessCount
        {
            get { return Processes.Count; }
        }

        public List<ProcessDto> Processes { get; set; }

    }
}