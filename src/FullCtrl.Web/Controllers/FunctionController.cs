using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FullCtrl.Base;
using FullCtrl.Plugins.Sound;
using FullCtrl.Web.Models;
using Lux;

namespace FullCtrl.Web.Controllers
{
    public class FunctionController : Controller
    {
        public IEnumerable<IFunctionPlugin> GetFunctionPlugins()
        {
            yield return new SoundFunctionPlugin();
        }


        public async Task<ActionResult> Plugin(string pluginName)
        {
            var model = await GetPluginViewModel(pluginName);
            return View("Plugin", model);
        }


        public async Task<PluginViewModel> GetPluginViewModel(string pluginName)
        {
            var plugin = GetFunctionPlugins().FirstOrDefault(x => x.Name == pluginName);
            if (plugin == null)
                throw new Exception($"Plugin '{pluginName}' not found");

            var model = new PluginViewModel();
            model.PluginName = pluginName;
            model.Functions = new FunctionViewModel[0];
            
            var functions = plugin.GetFunctions().ToList();
            foreach (var functionDescriptor in functions)
            {
                model.Functions = model.Functions.Concat(new[] {GetFunctionViewModel(functionDescriptor)}).ToArray();
            }
            return model;
        }

        public FunctionViewModel GetFunctionViewModel(IFunctionDescriptor functionDescriptor)
        {
            var model = new FunctionViewModel();
            model.FunctionName = functionDescriptor.Name;
            model.ParameterCollection = functionDescriptor.GetParameters();
            return model;
        }


        public async Task<IFunctionResult> Execute(string pluginName, string functionName)
        {
            var plugin = GetFunctionPlugins().FirstOrDefault(x => x.Name == pluginName);
            if (plugin == null)
                throw new Exception($"Plugin '{pluginName}' not found");

            var functionDescriptor = plugin.GetFunctions().FirstOrDefault(x => x.Name == functionName);
            if (functionDescriptor == null)
                throw new Exception($"Function '{functionName}' not found");

            var converter = new Converter();
            var parameters = functionDescriptor.GetParameters();
            foreach (var param in parameters)
            {
                var str = Request.Form["Param_" + param.Key];
                if (str != null)
                {
                    var val = converter.Convert(str, param.Value.Type);
                    param.Value.Value = val;
                }
            }

            var arg = new FunctionArguments();
            arg.Parameters = parameters;
            
            var function = functionDescriptor.Instantiate();
            var result = await function.Execute(arg);
            return result;
        }

    }
}