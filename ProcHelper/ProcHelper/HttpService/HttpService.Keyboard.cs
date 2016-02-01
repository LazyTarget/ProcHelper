namespace ProcHelper
{
    public partial class HttpService
    {
        public KeyResponse Any(IsKeyDown request)
        {
            var response = _worker.IsKeyDown(request);
            return response;
        }


        //public KeyResponse Any(IsKeyUp request)
        //{
        //    var response = _worker.IsKeyUp(request);
        //    return response;
        //}


        public KeyResponse Any(KeyDownRequest request)
        {
            var response = _worker.KeyDown(request);
            return response;
        }


        public KeyResponse Any(KeyUpRequest request)
        {
            var response = _worker.KeyUp(request);
            return response;
        }


        public KeyResponse Any(KeyPressRequest request)
        {
            var response = _worker.KeyPress(request);
            return response;
        }


        public WriteTextResponse Any(WriteTextRequest request)
        {
            var response = _worker.WriteTextEntry(request);
            return response;
        }
    }
}
