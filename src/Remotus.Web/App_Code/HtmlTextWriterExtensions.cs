using System;
using System.Web.UI;

namespace Remotus.Web
{
    public static class HtmlTextWriterExtensions
    {
        public static IDisposable BeginTag(this HtmlTextWriter writer, string tagName)
        {
            writer.WriteBeginTag(tagName);

            var disposable = new NotifyableDispose(() =>
            {
                writer.WriteEndTag(tagName);
            });
            return disposable;
        }


        public static IDisposable BeginTagAttributes(this HtmlTextWriter writer)
        {
            var disposable = new NotifyableDispose(() =>
            {
                writer.Write(HtmlTextWriter.TagRightChar);
            });
            return disposable;
        }


        private class NotifyableDispose : IDisposable
        {
            private readonly Action _callback;

            public NotifyableDispose(Action callback)
            {
                _callback = callback;
            }

            public void Dispose()
            {
                _callback();
            }
        }

    }
}