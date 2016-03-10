using System.Collections.Generic;
using FullCtrl.Base;

namespace FullCtrl.Plugins.Sound
{
    public class SoundFunctionPlugin : IFunctionPlugin
    {
        public string Name      => "Sound function plugin";
        public string Version   => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new ToggleMuteAudioDeviceFunction();
        }
    }
}
