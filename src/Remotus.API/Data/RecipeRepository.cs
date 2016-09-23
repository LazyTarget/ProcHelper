using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotus.Base;
using Remotus.API.Net;

namespace Remotus.API.Data
{
    public class RecipeRepository
    {
        private readonly IDictionary<string, IList<HubTrigger>> _triggers = new Dictionary<string, IList<HubTrigger>>();
        private readonly IDictionary<string, IList<HubAction>> _actions = new Dictionary<string, IList<HubAction>>();
        private readonly IList<HubRecipe> _recipies = new List<HubRecipe>();
        private bool _loaded;

        public RecipeRepository()
        {
            
        }


        private void Load()
        {
            lock (_recipies)
            {
                _recipies.Clear();
                _triggers.Clear();
                _actions.Clear();

                var plugins = Program.Service?.GetPlugins()?.OfType<IHubPlugin>();
                if (plugins != null)
                {
                    foreach(var plugin in plugins)
                    {
                        var hubs = plugin?.GetHubs();
                        if (hubs == null)
                            continue;
                        foreach (var hubDescriptor in hubs)
                        {
                            if (hubDescriptor == null)
                                continue;
                            var triggers = hubDescriptor.GetTriggers();
                            var actions = hubDescriptor.GetActions();


                            IList<HubTrigger> lt;
                            if (!_triggers.TryGetValue(hubDescriptor.HubName, out lt))
                            {
                                lt = new List<HubTrigger>();
                                _triggers[hubDescriptor.HubName] = lt;
                            }


                            IList<HubAction> la;
                            if (!_actions.TryGetValue(hubDescriptor.HubName, out la))
                            {
                                la = new List<HubAction>();
                                _actions[hubDescriptor.HubName] = la;
                            }
                        }
                    }
                }


                _recipies.Add(new HubRecipe
                {
                    Trigger = new CustomHubTrigger(hubName: "SpotifyHub", eventName: "OnVolumeChange")
                    {

                    },
                    Action = new CustomHubAction
                    {
                        Action = async (context, args) =>
                        {
                            var customSerializerSettings = new CustomJsonSerializerSettings();
                            var serializer = JsonSerializer.Create(customSerializerSettings.Settings);
                            var eventArgsJson = serializer.SerializeJson(args.FirstOrDefault());
                            var eventArgsJObj = serializer.DeserializeJson<JObject>(eventArgsJson);

                            var oldVolume = eventArgsJObj?.Property("OldVolume")?.Value?.ToObject<double>();
                            var newVolume = eventArgsJObj?.Property("NewVolume")?.Value?.ToObject<double>();
                            System.Console.WriteLine($"SPOTIFY VOLUME CHANGE!! {oldVolume} => {newVolume}");
                        },
                    },
                });


                _recipies.Add(new HubRecipe
                {
                    Trigger = new CustomHubTrigger(hubName: "SpotifyHub", eventName: "OnDeviceMuteChanged")
                    {

                    },
                    Action = new CustomHubAction
                    {
                        Action = async (context, args) =>
                        {
                            var customSerializerSettings = new CustomJsonSerializerSettings();
                            var serializer = JsonSerializer.Create(customSerializerSettings.Settings);
                            var eventArgsJson = serializer.SerializeJson(args.FirstOrDefault());
                            var eventArgsJObj = serializer.DeserializeJson<JObject>(eventArgsJson);

                            var isMuted = eventArgsJObj?.Property("IsMuted")?.Value?.ToObject<bool?>();
                            if (isMuted.HasValue)
                            {
                                if (isMuted.GetValueOrDefault())
                                    System.Console.WriteLine($"SOUND DEVICE MUTED!!");
                                else
                                    System.Console.WriteLine($"SOUND DEVICE UN-MUTED!!");
                            }
                            else
                            {

                            }
                        },
                    },
                });
                _loaded = true;
            }
        }


        public IEnumerable<HubRecipe> GetRecipes()
        {
            if (!_loaded)
            {
                Load();
            }

            IEnumerable<HubRecipe> result;
            lock (_recipies)
            {
                result = _recipies.AsEnumerable();
            }
            return result;
        }

    }
}
