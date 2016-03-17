using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;
using Remotus.Plugins.Sound;

namespace Remotus.API
{
    [Obsolete]
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

        [Obsolete]
        public IProcessAPI Process { get; }

        [Obsolete]
        public IWinServiceAPI WinService { get; }

        [Obsolete]
        public IPowershellAPI Powershell { get; }

        [Obsolete]
        public IMouseAPI Mouse { get; }

        [Obsolete]
        public IKeyboardAPI Keyboard { get; }

        [Obsolete]
        public IAudioControllerAPI AudioController { get; }

        [Obsolete]
        public IDeviceControllerAPI DeviceController { get; }


        [Obsolete]
        public async Task<IEnumerable<IPlugin>> GetPlugins(string clientID)
        {
            //var client =      // todo: get client from db
            // todo get clients for that client

            var plugins = new List<IPlugin>();
            plugins.Add(new SoundFunctionPlugin());
            return plugins;
        }

        [Obsolete]
        public async Task<IFunctionResult> ExecuteFunction(string clientID, string pluginName, string functionName, IFunctionArguments arguments)
        {
            throw new NotSupportedException();
            try
            {
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
                    //var result = await function.Execute(arguments);
                    //return result;
                    return null;
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
