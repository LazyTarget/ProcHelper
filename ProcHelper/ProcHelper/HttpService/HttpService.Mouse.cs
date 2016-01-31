namespace ProcHelper
{
    public partial class HttpService
    {
        public MoveMouseResponse Any(GetMousePositionRequest request)
        {
            var response = _worker.GetMousePosition(request);
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
