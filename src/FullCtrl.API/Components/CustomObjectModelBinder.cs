using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Lux;
using Lux.Interfaces;

namespace FullCtrl.API
{
    public class CustomObjectModelBinder : IModelBinder
    {
        public CustomObjectModelBinder()
        {
            Converter = new Converter();
        }

        public IConverter Converter { get; set; }


        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var model = bindingContext.Model;
            if (model == null)
            {
                model = bindingContext.Model = Activator.CreateInstance(bindingContext.ModelType);
            }
            
            foreach (var routeValue in actionContext.RequestContext.RouteData.Values)
            {
                var propertyName = routeValue.Key;
                var propertyInfo = bindingContext.ModelType.GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    var value = Converter.Convert(routeValue.Value, propertyInfo.PropertyType);
                    propertyInfo.SetValue(model, value);
                }
            }
            return true;
        }
    }
}