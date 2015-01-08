using System;

namespace ProcHelper
{
    public class RequestDto : ServiceStack.IReturn<ResponseDto>
    {
        public RequestDto()
        {
            ID = Guid.NewGuid();
        }

        public Guid ID { get; set; }

        public string Value { get; set; }
    }
}