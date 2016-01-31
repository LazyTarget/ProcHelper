namespace ProcHelper
{
    public class PowershellHelper : IPowershellHelper
    {
        private IProcessHelper _processHelper;

        public PowershellHelper(IProcessHelper processHelper)
        {
            _processHelper = processHelper;
        }


        public PowershellResponse RunFile(PowershellFileRequest request)
        {
            var args = string.Format(@"\& ""{0}""", request.Query);
            var processInfo = _processHelper.StartProcess("powershell", args, null, request.RedirectStandardOutput);

            var response = new PowershellResponse
            {
                Request = request,
            };
            return response;
        }

        public PowershellResponse RunQuery(PowershellQueryRequest request)
        {
            var args = string.Format(@"\& ""{0}""", request.Query);
            var processInfo = _processHelper.StartProcess("powershell", args, null, request.RedirectStandardOutput);

            var response = new PowershellResponse
            {
                Request = request,
            };
            return response;
        }
    }
}
