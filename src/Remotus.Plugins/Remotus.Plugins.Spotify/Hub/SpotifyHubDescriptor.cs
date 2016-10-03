using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotus;
using Remotus.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Remotus.Plugins.Spotify.Hub
{
    public class SpotifyHubDescriptor : IHubDescriptor
    {
        public string HubName => "SpotifyHub";
        
        public IEnumerable<HubTrigger> GetTriggers()
        {
            var result = new List<HubTrigger>();
            result.Add(new OnVolumeChangeHubTrigger());
            result.Add(new OnTrackChangeHubTrigger());
            return result;
        }

        public IEnumerable<HubAction> GetActions()
        {
            var result = new List<HubAction>();
            result.Add(new InvokeFunctionHubAction
            {
                PluginID = "79A54741-590C-464D-B1E9-0CC606771493",  // Spotify
                FunctionID = new PauseFunction.Descriptor().ID,     // Pause
            });
            //result.Add(new CustomHubAction
            //{
            //    Action = async (context, args) =>
            //    {
            //        var customSerializerSettings = new CustomJsonSerializerSettings();
            //        var serializer = JsonSerializer.Create(customSerializerSettings.Settings);
            //        var eventArgsJson = serializer.SerializeJson(args.FirstOrDefault());
            //        var eventArgsJObj = serializer.DeserializeJson<JObject>(eventArgsJson);

            //        var oldVolume = eventArgsJObj?.Property("OldVolume")?.Value?.ToObject<double>();
            //        var newVolume = eventArgsJObj?.Property("NewVolume")?.Value?.ToObject<double>();
            //        System.Console.WriteLine($"SPOTIFY VOLUME CHANGE!! {oldVolume} => {newVolume}");
            //    },
            //});
            return result;
        }
    }
}
