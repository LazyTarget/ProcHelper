using System;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Spotify
{
    public class PausePlaybackCommand : ICommand
    {
        public PausePlaybackCommand()
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
                var connected = Worker.ConnectIfNotConnected();
                if (!connected)
                {
                    throw new Exception("Unable to connect to Spotify");
                }
                Worker.Api.Pause();
                var status = Worker.Api.GetStatus();

                var result = new CommandResult();
                result.Result = status;
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
            public string ID => "B525CAD9-27E6-4709-B29B-96A32438C7D0";
            public string Name => nameof(PausePlaybackCommand);
            public string Version => "1.0.0.0";

            public ICommand Instantiate()
            {
                return new PausePlaybackCommand();
            }
        }

        public void Dispose()
        {
            
        }

    }
}