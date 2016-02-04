using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IKeyboardAPI
    {
        Task<IResponseBase<KeyResponse>> IsKeyDown(IsKeyDown request);
        Task<IResponseBase<KeyResponse>> KeyDown(KeyDownRequest request);
        Task<IResponseBase<KeyResponse>> KeyUp(KeyUpRequest request);
        Task<IResponseBase<KeyResponse>> KeyPress(KeyPressRequest request);
        Task<IResponseBase<object>> WriteTextEntry(string text);
    }
}
