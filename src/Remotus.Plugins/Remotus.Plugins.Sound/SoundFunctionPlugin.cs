using System.Collections.Generic;
using Remotus.Base;

namespace Remotus.Plugins.Sound
{
    public class SoundFunctionPlugin : IFunctionPlugin
    {
        public string Name      => nameof(SoundFunctionPlugin);
        public string Version   => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new GetAudioDevicesFunction.Descriptor();
            yield return new GetAudioSessionsFunction.Descriptor();
            yield return new ToggleMuteAudioDeviceFunction.Descriptor();
        }
    }
}
