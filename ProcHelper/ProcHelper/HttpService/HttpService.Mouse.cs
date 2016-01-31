namespace ProcHelper
{
    public partial class HttpService
    {
        public MouseInfoResponse Any(MouseInfoRequest request)
        {
            var response = _worker.GetMouseInfo(request);
            return response;
        }


        public MoveMouseResponse Any(MoveMouseBy request)
        {
            var response = _worker.MoveMouseBy(request);
            return response;
        }


        public MoveMouseResponse Any(MoveMouseTo request)
        {
            var response = _worker.MoveMouseTo(request);
            return response;
        }


        public MoveMouseResponse Any(MoveMouseToPositionOnVirtualDesktop request)
        {
            var response = _worker.MoveMouseToPositionOnVirtualDesktop(request);
            return response;
        }
    }
}
