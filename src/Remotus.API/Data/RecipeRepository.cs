using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotus.Base;

namespace Remotus.API.Data
{
    public class RecipeRepository
    {
        private readonly IList<HubRecipe> _data = new List<HubRecipe>();
        private bool _loaded;

        public RecipeRepository()
        {

        }


        private void Load()
        {
            lock (_data)
            {
                _data.Clear();

                _data.Add(new HubRecipe
                {
                    Trigger = new HubTrigger
                    {
                        HubName = "SpotifyHub",
                        EventName = "OnVolumeChange",
                    },
                    Action = new CustomHubAction
                    {
                        Action = async (args) =>
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


                _data.Add(new HubRecipe
                {
                    Trigger = new HubTrigger
                    {
                        HubName = "SoundHub",
                        EventName = "OnDeviceMuteChanged",
                    },
                    Action = new CustomHubAction
                    {
                        Action = async (args) =>
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
            lock (_data)
            {
                result = _data.AsEnumerable();
            }
            return result;
        }

    }
}
