namespace Remotus.Base
{
    public interface IError
    {
        string ErrorMessage { get; }
        bool Handled { get; set; }
        void Throw();
    }
}
