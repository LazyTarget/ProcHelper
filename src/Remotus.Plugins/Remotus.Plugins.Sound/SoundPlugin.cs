using System.Collections.Generic;
using AudioSwitcher.AudioApi.CoreAudio;
using Remotus.Base;

namespace Remotus.Plugins.Sound
{
    public class SoundPlugin : IFunctionPlugin
    {
        internal static readonly CoreAudioController AudioController = new CoreAudioController();


        public string ID        => "ABA6417A-65A2-4761-9B01-AA9DFFC074C0";
        public string Name      => "Sound";
        public string Version   => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new GetAudioDevicesFunction.Descriptor();
            yield return new GetAudioSessionsFunction.Descriptor();
            yield return new SetDefaultAudioDeviceFunction.Descriptor();
            yield return new SetDefaultAudioCommunicationsDeviceFunction.Descriptor();
            yield return new SetAudioDeviceVolumeFunction.Descriptor();
            yield return new SetAudioDeviceMutedFunction.Descriptor();
            yield return new ToggleAudioDeviceMutedFunction.Descriptor();
            yield return new SetAudioSessionVolumeFunction.Descriptor();
            yield return new SetAudioSessionMutedFunction.Descriptor();
            yield return new ToggleAudioSessionMutedFunction.Descriptor();
        }
    }
}
