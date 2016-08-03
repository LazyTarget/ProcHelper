using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Remotus.Base;
using Remotus.Base.Models.Hub;
using Remotus.Base.Models.Payloads;
using SpotifyAPI.Local;

namespace Remotus.Plugins.Spotify
{
    public class SpotifyPlugin : IFunctionPlugin, IServicePlugin
    {
        private ServiceStatus _status;
        //private ClientHubManager _pluginHub;

        public SpotifyPlugin()
        {
            
        }
        
        public string ID        => "79A54741-590C-464D-B1E9-0CC606771493";
        public string Name      => "Spotify";
        public string Version   => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new PlayFunction.Descriptor();
            yield return new PauseFunction.Descriptor();
            yield return new PlayUriFunction.Descriptor();
            yield return new TogglePlayingFunction.Descriptor();
            yield return new PreviousTrackFunction.Descriptor();
            yield return new NextTrackFunction.Descriptor();
            yield return new GetStatusFunction.Descriptor();
            yield return new GetProfileFunction.Descriptor();
        }

        public ServiceStatus Status
        {
            get { return _status; }
            private set
            {
                var old = _status;
                if (old != value)
                {
                    _status = value;
                    var e = new ServiceStatusChangedEventArgs(old, value);
                    InvokeOnStatusChanged(e);
                }
            }
        }

        public event EventHandler<ServiceStatusChangedEventArgs> OnStatusChanged;


        public async Task Init(IExecutionContext context)
        {
            if (Status != ServiceStatus.None &&
                Status != ServiceStatus.Stopped)
            {
                // Already initialized
                return;
            }

            Status = ServiceStatus.Initializing;


            //var agent = context.HubAgentFactory.Create("Spotify");
            //_pluginHub = new ClientHubManager(connection);


            Action act = async () =>
            {
                var diagHub = context.HubAgentFactory.Create("DiagnosticsHub", null);
                await diagHub.Connector.Connect();

                while (true)
                {
                    var m1 = new DebugMessage("null", null, "Mesg1", 1);
                    var m2 = new DebugMessage("null", null, "Mesg2", 2);
                    var m3 = new DebugMessage("null", null, "Mesg2", 3);
                    var m4 = new DebugMessage("null", null, "Mesg2", 4);
                    var msg = new HubMessage();
                    msg.Method = "Write";
                    msg.Args = new[] { JToken.FromObject(m1) };
                    await diagHub.Invoke(msg);
                    msg.Args = new[] { JToken.FromObject(m2.Message) };
                    await diagHub.Invoke(msg);
                    msg.Args = new[] { m3.Message };
                    await diagHub.Invoke(msg);
                    msg.Args = new[] { m4.Message };
                    await diagHub.Invoke(msg);

                    Thread.Sleep(1500);
                }
            };

#if DEBUG
            ThreadPool.QueueUserWorkItem(state => act());
#endif


            Status = ServiceStatus.Initialized;
        }


        public async Task Start()
        {
            Status = ServiceStatus.Starting;

            Worker.ConnectLocalIfNotConnected();
            Worker.LocalApi.OnPlayStateChange += LocalApi_OnPlayStateChange;
            Worker.LocalApi.OnTrackTimeChange += LocalApi_OnTrackTimeChange;
            Worker.LocalApi.OnTrackChange += LocalApi_OnTrackChange;
            Worker.LocalApi.OnVolumeChange += LocalApi_OnVolumeChange;
            Worker.LocalApi.ListenForEvents = true;

            Status = ServiceStatus.Running;
        }


        public async Task Stop()
        {
            Status = ServiceStatus.Stopping;

            Worker.LocalApi.ListenForEvents = false;
            Worker.LocalApi.OnPlayStateChange -= LocalApi_OnPlayStateChange;
            Worker.LocalApi.OnTrackTimeChange -= LocalApi_OnTrackTimeChange;
            Worker.LocalApi.OnTrackChange -= LocalApi_OnTrackChange;
            Worker.LocalApi.OnVolumeChange -= LocalApi_OnVolumeChange;

            Status = ServiceStatus.Stopped;
        }



        private void LocalApi_OnPlayStateChange(object sender, PlayStateEventArgs args)
        {
            
        }

        private void LocalApi_OnTrackChange(object sender, TrackChangeEventArgs args)
        {

        }

        private void LocalApi_OnTrackTimeChange(object sender, TrackTimeChangeEventArgs args)
        {

        }

        private void LocalApi_OnVolumeChange(object sender, VolumeChangeEventArgs args)
        {
            
        }



        protected virtual void InvokeOnStatusChanged(ServiceStatusChangedEventArgs e)
        {
            try
            {
                // todo: Run in seperate thread?
                OnStatusChanged?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                
            }
        }


        public void Dispose()
        {

        }
    }
}
