using System.Collections.Generic;
using System.Linq;
using FullCtrl.Base;

namespace FullCtrl
{
    public class Worker
    {
        private IProcessHelper _processHelper;
        private IWinServiceHelper _winServiceHelper;
        private IPowershellHelper _powershellHelper;
        private IInputHelper _inputHelper;

        public Worker()
        {
            _processHelper = new ProcessHelper();
            _winServiceHelper = new WinServiceHelper();
            _powershellHelper = new PowershellHelper(_processHelper);
            _inputHelper = new InputHelper();
        }



        #region ProcHelper

        public ProcessesResponse GetProcesses(GetProcessesRequest request)
        {
            List<ProcessDto> processes;
            if (request != null && !string.IsNullOrEmpty(request.Name))
                processes = _processHelper.GetProcesses(request.Name);
            else
                processes = _processHelper.GetProcesses();

            var response = new ProcessesResponse
            {
                Processes = processes,
            };
            return response;
        }


        public StartProcessResponse StartProcess(StartProcessRequest request)
        {
            var process = _processHelper.StartProcess(request.FileName, request.Arguments, request.WorkingDirectory, request.RedirectStandardOutput);

            var response = new StartProcessResponse
            {
                Process = process,
            };
            if (request.RedirectStandardOutput)
            {
                var p = process.GetBase();
                response.StandardOutput = p.StandardOutput.ReadToEnd();
                response.StandardError = p.StandardError.ReadToEnd();
            }
            return response;
        }

        public KillProcessResponse KillProcess(KillProcessRequest request)
        {
            var process = _processHelper.KillProcess(request.ProcessID);
            var response = new KillProcessResponse
            {
                Process = process,
            };
            return response;
        }

        #endregion


        #region Powershell

        
        public PowershellResponse RunPowershellFile(PowershellFileRequest request)
        {
            var response = _powershellHelper.RunFile(request);
            return response;
        }

        public PowershellResponse RunPowershellQuery(PowershellQueryRequest request)
        {
            var response = _powershellHelper.RunQuery(request);
            return response;
        }

        #endregion


        #region WinServiceHelper


        public WinServicesResponse GetWinServices(GetWinServicesRequest request)
        {
            var services = _winServiceHelper.GetServices();
            if (request != null && !string.IsNullOrEmpty(request.Name))
                services = services.Where(x => x.ServiceName == request.Name).ToList();
            var response = new WinServicesResponse
            {
                Services = services,
            };
            return response;
        }

        public StartWinServiceResponse StartWinService(StartWinServiceRequest request)
        {
            var argArray = request.Arguments != null ? request.Arguments.Split(' ') : null;
            var service = _winServiceHelper.StartService(request.ServiceName, argArray);
            var response = new StartWinServiceResponse
            {
                Service = service,
            };
            return response;
        }

        public PauseWinServiceResponse PauseWinService(PauseWinServiceRequest request)
        {
            var service = _winServiceHelper.PauseService(request.ServiceName);
            var response = new PauseWinServiceResponse
            {
                Service = service,
            };
            return response;
        }

        public ContinueWinServiceResponse ContinueWinService(ContinueWinServiceRequest request)
        {
            var service = _winServiceHelper.ContinueService(request.ServiceName);
            var response = new ContinueWinServiceResponse
            {
                Service = service,
            };
            return response;
        }

        public StopWinServiceResponse StopWinService(StopWinServiceRequest request)
        {
            var service = _winServiceHelper.StopService(request.ServiceName);
            var response = new StopWinServiceResponse
            {
                Service = service,
            };
            return response;
        }



        #endregion


        #region InputHelper


        #region Mouse

        public MouseInfoResponse GetMouseInfo(MouseInfoRequest request)
        {
            var pos = _inputHelper.GetMousePosition();
            
            var response = new MouseInfoResponse
            {
                Request = request,
                CursorPosition = pos,
            };
            

            var screens = System.Windows.Forms.Screen.AllScreens;
            response.ScreenCount = screens.Length;


            // The screen size of the current screen (based on cursor position)
            var rect = System.Windows.Forms.Screen.GetBounds(new System.Drawing.Point(pos.X, pos.Y));
            response.ScreenSize = new Point(rect.Width, rect.Height);

            
            // The combined screen size
            rect = new System.Drawing.Rectangle();
            foreach (var screen in screens)
            {
                rect = System.Drawing.Rectangle.Union(rect, screen.Bounds);
            }
            response.VirtualScreenSize = new Point(rect.Width, rect.Height);

            return response;
        }

        public MoveMouseResponse MoveMouseBy(MoveMouseBy request)
        {
            _inputHelper.MoveMouseBy(request.X, request.Y);

            var pos = _inputHelper.GetMousePosition();

            var response = new MoveMouseResponse
            {
                Request = request,
                Position = pos,
            };
            return response;
        }

        public MoveMouseResponse MoveMouseTo(MoveMouseTo request)
        {
            _inputHelper.MoveMouseTo(request.X, request.Y);

            var pos = _inputHelper.GetMousePosition();

            var response = new MoveMouseResponse
            {
                Request = request,
                Position = pos,
            };
            return response;
        }

        public MoveMouseResponse MoveMouseToPositionOnVirtualDesktop(MoveMouseToPositionOnVirtualDesktop request)
        {
            _inputHelper.MoveMouseToPositionOnVirtualDesktop(request.X, request.Y);

            var pos = _inputHelper.GetMousePosition();

            var response = new MoveMouseResponse
            {
                Request = request,
                Position = pos,
            };
            return response;
        }

        #endregion


        #region Keyboard

        public KeyResponse IsKeyDown(IsKeyDown request)
        {
            var isDown = _inputHelper.IsKeyDown(request.VirtualKeyCode);

            var response = new KeyResponse
            {
                Request = request,
                VirtualKeyCode = request.VirtualKeyCode,
                IsDown = isDown,
            };
            return response;
        }

        public KeyResponse IsKeyUp(IsKeyUp request)
        {
            var isUp = _inputHelper.IsKeyUp(request.VirtualKeyCode);

            var response = new KeyResponse
            {
                Request = request,
                VirtualKeyCode = request.VirtualKeyCode,
                IsDown = !isUp,
            };
            return response;
        }

        public KeyResponse KeyDown(KeyDownRequest request)
        {
            _inputHelper.KeyDown(request.VirtualKeyCode);

            var response = new KeyResponse
            {
                Request = request,
                VirtualKeyCode = request.VirtualKeyCode,
                IsDown = _inputHelper.IsKeyDown(request.VirtualKeyCode),
                //IsDown = true,
            };
            return response;
        }

        public KeyResponse KeyUp(KeyUpRequest request)
        {
            _inputHelper.KeyUp(request.VirtualKeyCode);

            var response = new KeyResponse
            {
                Request = request,
                VirtualKeyCode = request.VirtualKeyCode,
                IsDown = _inputHelper.IsKeyDown(request.VirtualKeyCode),
                //IsDown = false,
            };
            return response;
        }

        public KeyResponse KeyPress(KeyPressRequest request)
        {
            _inputHelper.KeyPress(request.VirtualKeyCode);

            var response = new KeyResponse
            {
                Request = request,
                VirtualKeyCode = request.VirtualKeyCode,
                IsDown = _inputHelper.IsKeyDown(request.VirtualKeyCode),
            };
            return response;
        }

        public WriteTextResponse WriteTextEntry(WriteTextRequest request)
        {
            _inputHelper.WriteTextEntry(request.Text);

            var response = new WriteTextResponse
            {
                Request = request,
                Text = request.Text,
            };
            return response;
        }

        #endregion


        #endregion

    }
}
