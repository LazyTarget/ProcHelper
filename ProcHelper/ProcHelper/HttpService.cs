using System;
using ServiceStack;

namespace ProcHelper
{
    public class HttpService : ServiceStack.Service
    {
        public ResponseDto Get(RequestDto request)
        {
            var response = new ResponseDto
            {
                Request = request,
                Success = true,
            };
            return response;
        }

    }


    public class RequestDto : IReturn<ResponseDto>
    {
        public RequestDto()
        {
            ID = Guid.NewGuid();
        }

        public Guid ID { get; set; }

        public string Value { get; set; }
    }


    public class ResponseDto
    {
        public RequestDto Request { get; set; }

        public bool Success { get; set; }

    }
}
