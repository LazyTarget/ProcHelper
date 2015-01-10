using System.Collections.Generic;

namespace ProcHelper
{
    public class WinServicesResponse
    {
        public WinServicesResponse()
        {
            Services = new List<WinServiceDto>();
        }

        public int ServiceCount
        {
            get { return Services.Count; }
        }

        public List<WinServiceDto> Services { get; set; }

    }
}