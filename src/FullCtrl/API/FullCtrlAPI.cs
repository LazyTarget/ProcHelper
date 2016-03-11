using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FullCtrl.Base;
using FullCtrl.Plugins.Sound;

namespace FullCtrl
{
    public class FullCtrlAPI : IFullCtrlAPI
    {
        public FullCtrlAPI()
        {
            Process = new ProcessAPI();
            //WinService = new WinServiceAPI();
            //Powershell = new PowershellAPI();
            //Mouse = new MouseAPI();
            //Keyboard = new KeyboardAPI();
            AudioController = new AudioControllerAPI();
            DeviceController = new DeviceControllerAPI();
        }

        public IProcessAPI Process { get; }
        public IWinServiceAPI WinService { get; }
        public IPowershellAPI Powershell { get; }
        public IMouseAPI Mouse { get; }
        public IKeyboardAPI Keyboard { get; }
        public IAudioControllerAPI AudioController { get; }
        public IDeviceControllerAPI DeviceController { get; }


        public async Task<IEnumerable<IPlugin>> GetPlugins(string clientID)
        {
            var plugins = new List<IPlugin>();
            plugins.Add(new SoundFunctionPlugin());
            return plugins;
        }

        public async Task<IFunctionResult> ExecuteFunction(string clientID, string pluginName, string functionName, IFunctionArguments arguments)
        {
            try
            {
                //var client =      // todo: get client from db

                var plugins = await GetPlugins(clientID);
                var plugin = plugins.FirstOrDefault(x => x.Name == pluginName);
                if (plugin == null)
                {
                    throw new Exception($"Plugin '{pluginName}' not found");
                }

                if (plugin is IFunctionPlugin)
                {
                    var functionPlugin = (IFunctionPlugin)plugin;
                    var functions = functionPlugin.GetFunctions();
                    var functionDescriptor = functions.FirstOrDefault(x => x.Name == functionName);
                    if (functionDescriptor == null)
                    {
                        throw new Exception($"Function '{functionName}' not found");
                    }

                    var function = functionDescriptor.Instantiate();
                    var result = await function.Execute(arguments);
                    return result;
                }
                else
                {
                    throw new Exception($"Plugin '{functionName}' is not a function plugin");
                }
            }
            catch (Exception ex)
            {
                var result = new FunctionResult();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }
    }
}
