using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IAudioControllerAPI
    {
        Task<IResponseBase<IEnumerable<AudioSession>>> GetAudioEndpoints();
    }
}
