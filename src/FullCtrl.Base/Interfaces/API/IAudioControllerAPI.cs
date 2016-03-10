using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IAudioControllerAPI
    {
        Task<IResponseBase<IEnumerable<AudioSession>>> GetAudioSessions();
        Task<IResponseBase<IEnumerable<AudioDevice>>> GetAudioDevices();
        Task<IResponseBase<object>> SetDefaultDevice(string deviceID);
        Task<IResponseBase<object>> ToggleDeviceMute(string deviceID);
    }
}
