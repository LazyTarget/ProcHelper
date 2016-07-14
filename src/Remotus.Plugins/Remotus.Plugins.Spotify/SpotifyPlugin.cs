using System;
using System.Collections.Generic;
using Remotus.API.Hubs.Client;
using Remotus.Base;
using SpotifyAPI.Local;

namespace Remotus.Plugins.Spotify
{
    public class SpotifyPlugin : IFunctionPlugin, IServicePlugin
    {
        private ClientHubManager _pluginHub;


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

        public ServiceStatus Status { get; private set; }


        public void Init(IExecutionContext context)
        {
            if (Status != ServiceStatus.None &&
                Status != ServiceStatus.Stopped)
            {
                // Already initialized
                return;
            }

            Status = ServiceStatus.Initializing;
            
            _pluginHub = new ClientHubManager(connection);


            Status = ServiceStatus.Initialized;
        }


        public void Start()
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


        public void Stop()
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


        public void Dispose()
        {
            
        }

    }
}
