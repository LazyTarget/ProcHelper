using System.Reflection;
using Funq;

namespace ProcHelper
{
    public class HttpAppHost : ServiceStack.AppHostHttpListenerBase
    {
        public HttpAppHost()
            : base("ProcHelper", new Assembly[] { typeof(HttpService).Assembly })
        {
        }

        public HttpAppHost(string handlerPath)
            : base("ProcHelper", handlerPath, new Assembly[] { typeof(HttpService).Assembly })
        {
        }



        public override void Configure(Container container)
        {
            // Process
            Routes.Add<GetProcessesRequest>("/Process")
                  .Add<GetProcessesRequest>("/Process/{Name}")
                  .Add<StartProcessRequest>("/Process/Start")
                  .Add<StartProcessRequest>("/Process/Start/{FileName}")
                  .Add<StartProcessRequest>("/Process/Start/{FileName}/{Arguments}")
                  .Add<StartProcessRequest>("/Process/Start/{FileName}/{Arguments}/{WorkingDirectory}")
                  .Add<KillProcessRequest>("/Process/Kill/{ProcessID}");

            // WinService
            Routes.Add<GetWinServicesRequest>("/WinService")
                  .Add<GetWinServicesRequest>("/WinService/{Name}")
                  .Add<StartWinServiceRequest>("/WinService/Start")
                  .Add<StartWinServiceRequest>("/WinService/Start/{ServiceName}")
                  .Add<StartWinServiceRequest>("/WinService/Start/{ServiceName}/{Arguments}")
                  .Add<PauseWinServiceRequest>("/WinService/Pause")
                  .Add<PauseWinServiceRequest>("/WinService/Pause/{ServiceName}")
                  .Add<ContinueWinServiceRequest>("/WinService/Continue")
                  .Add<ContinueWinServiceRequest>("/WinService/Continue/{ServiceName}")
                  .Add<StopWinServiceRequest>("/WinService/Stop")
                  .Add<StopWinServiceRequest>("/WinService/Stop/{ServiceName}");

            // Powershell
            Routes.Add<PowershellFileRequest>("/Powershell/FileName/{FileName}")
                  .Add<PowershellQueryRequest>("/Powershell/Query/{Query}");



            // Input
            Routes.Add<GetMousePositionRequest>("/Input/Mouse/Position")
                  .Add<MoveMouseBy>("/Input/Mouse/MoveBy/{X}/{Y}")
                  .Add<MoveMouseTo>("/Input/Mouse/MoveTo/{X}/{Y}")
                  .Add<MoveMouseToPositionOnVirtualDesktop>("/Input/Mouse/MoveToVirtual/{X}/{Y}");
        }

    }
}
