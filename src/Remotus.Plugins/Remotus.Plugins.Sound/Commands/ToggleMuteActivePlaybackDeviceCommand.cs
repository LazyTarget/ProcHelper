using System;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Sound
{
    public class ToggleMuteActivePlaybackDeviceCommand : ICommand
    {
        public ToggleMuteActivePlaybackDeviceCommand()
        {
            
        }

        public ICommandDescriptor GetDescriptor()
        {
            return new Descriptor();
        }

        public async Task<ICommandResult> Execute(IExecutionContext context)
        {

            try
            {
                var descriptor1 = new GetAudioDevicesFunction.Descriptor();
                var function1 = descriptor1.Instantiate();

                var parameters1 = descriptor1.GetParameters();
                parameters1.DeviceState.Value = AudioDeviceState.Active;
                parameters1.DeviceType.Value = AudioDeviceType.Playback;

                IFunctionArguments arguments = new FunctionArguments();
                arguments.Parameters = parameters1;
                var functionResult1 = await function1.Execute(context, arguments);
                functionResult1.EnsureSuccess("Could not get audio device");

                var device = functionResult1?.Result?.FirstOrDefault(x => x.DefaultDevice && x.DeviceType == AudioDeviceType.Playback);
                if (device == null)
                {
                    throw new Exception("Could not find default device");
                }

                var descriptor2 = new ToggleMuteAudioDeviceFunction.Descriptor();
                var function2 = descriptor2.Instantiate();

                var parameters2 = descriptor2.GetParameters();
                parameters2.DeviceID.Value = device.ID.ToString();

                arguments = new FunctionArguments();
                arguments.Parameters = parameters2;
                var functionResult = await function2.Execute(context, arguments);

                var result = new CommandResult();
                result.Result = functionResult;
                return result;
            }
            catch (Exception ex)
            {
                var result = new CommandResult();
                //result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public class Descriptor : ICommandDescriptor
        {
            public string Name => nameof(ToggleMuteActivePlaybackDeviceCommand);
            public string Version => "1.0.0.0";

            public ICommand Instantiate()
            {
                return new ToggleMuteActivePlaybackDeviceCommand();
            }
        }

        public void Dispose()
        {
            
        }

    }
}