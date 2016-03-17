using System;
using System.Threading.Tasks;

namespace Remotus.Base
{
    [Obsolete]
    public interface IAudioControllerAPI
    {
        //Task<IResponseBase<IEnumerable<AudioSession>>> GetAudioSessions(string deviceID);
        //Task<IResponseBase<IEnumerable<AudioDevice>>> GetAudioDevices(AudioDeviceType? deviceType, AudioDeviceState? deviceState);
        Task<IResponseBase<object>> SetDefaultDevice(string deviceID);
        Task<IResponseBase<object>> ToggleDeviceMute(string deviceID);
    }
}
