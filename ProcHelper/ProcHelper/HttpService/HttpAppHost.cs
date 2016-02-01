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



            // Input - Mouse
            Routes.Add<MouseInfoRequest>("/Input/Mouse")
                  .Add<MoveMouseBy>("/Input/Mouse/MoveBy/{X}/{Y}")
                  .Add<MoveMouseTo>("/Input/Mouse/MoveTo/{X}/{Y}")
                  .Add<MoveMouseToPositionOnVirtualDesktop>("/Input/Mouse/MoveToVirtual/{X}/{Y}");

            // Input - Keyboard
            Routes.Add<IsKeyDown>           ("/Input/Key/IsDown")
                  .Add<IsKeyDown>           ("/Input/Key/IsDown/{VirtualKeyCode}")
                  .Add<IsKeyUp>             ("/Input/Key/IsUp")
                  .Add<IsKeyUp>             ("/Input/Key/IsUp/{VirtualKeyCode}")
                  .Add<KeyDownRequest>      ("/Input/Key/Down")
                  .Add<KeyDownRequest>      ("/Input/Key/Down/{VirtualKeyCode}")
                  .Add<KeyUpRequest>        ("/Input/Key/Up")
                  .Add<KeyUpRequest>        ("/Input/Key/Up/{VirtualKeyCode}")
                  .Add<KeyPressRequest>     ("/Input/Key/Press")
                  .Add<KeyPressRequest>     ("/Input/Key/Press/{VirtualKeyCode}")

                  .Add<WriteTextRequest>    ("/Input/Text/Write")
                  .Add<WriteTextRequest>    ("/Input/Text/Write/{Text}");
        }

    }
}
