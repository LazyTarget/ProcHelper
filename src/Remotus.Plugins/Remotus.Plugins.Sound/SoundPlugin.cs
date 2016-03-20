using System.Collections.Generic;
using Remotus.Base;

namespace Remotus.Plugins.Sound
{
    public class SoundPlugin : IFunctionPlugin, ICommandPlugin
    {
        public string ID        => "ABA6417A-65A2-4761-9B01-AA9DFFC074C0";
        public string Name      => nameof(SoundPlugin);
        public string Version   => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new GetAudioDevicesFunction.Descriptor();
            yield return new GetAudioSessionsFunction.Descriptor();
            yield return new ToggleMuteAudioDeviceFunction.Descriptor();
        }

        public IEnumerable<ICommandDescriptor> GetCommands()
        {
            yield return new ToggleMuteActivePlaybackDeviceCommand.Descriptor();
        }
    }
}
