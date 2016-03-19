using System.IO;

namespace Remotus.Web.Helpers
{
    public interface IObjectRenderer
    {
        bool CanRender(object value);
        void Render(TextWriter textWriter, object value);
    }
}