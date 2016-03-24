using NUnit.Framework;
using Remotus.Web.Rendering;

namespace Remotus.Web.Tests.RendererTests.FileTemplateObjectRendererTests
{
    [TestFixture]
    public abstract class FileTemplateObjectRendererTestBase : ObjectRendererTestBase
    {
        public override IObjectRenderer GetSUT()
        {
            var fileTemplate = GetFileTemplate();
            var sut = new FileTemplateObjectRenderer(fileTemplate);
            return sut;
        }

        public abstract FileTemplateObjectRenderer.IFileTemplate GetFileTemplate();
    }
}
