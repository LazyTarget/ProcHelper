using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Remotus.Web.Rendering
{
    public class ObjectRendererCollection : IObjectRenderer
    {
        public ObjectRendererCollection()
        {
            Children = new List<IObjectRenderer>();
        }

        public IList<IObjectRenderer> Children { get; private set; }

        
        public virtual bool CanRender(object value)
        {
            var canRender = false;
            if (Children != null && Children.Any())
            {
                foreach (var objectRenderer in Children)
                {
                    if (objectRenderer == null)
                        continue;
                    canRender = objectRenderer.CanRender(value);
                    if (canRender)
                        break;
                }
            }
            return canRender;
        }

        public virtual void Render(TextWriter textWriter, object value)
        {
            try
            {
                if (Children != null && Children.Any())
                {
                    foreach (var objectRenderer in Children)
                    {
                        if (objectRenderer == null)
                            continue;
                        var canRender = objectRenderer.CanRender(value);
                        if (!canRender)
                            continue;

                        objectRenderer.Render(textWriter, value);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}