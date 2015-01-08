namespace ProcHelper
{
    public class HttpService : ServiceStack.Service
    {
        private ProcessHelper _helper = new ProcessHelper();


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
}
