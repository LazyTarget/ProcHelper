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
                var api = new SpotifyAPI.Local.SpotifyLocalAPI();
                api.ListenForEvents = false;        // todo: Implement as IServicePlugin, and expose event hooks
                var connected = api.Connect();
                if (!connected)
                {
                    throw new Exception("Unable to connect to Spotify");
                }
                api.Pause();
                var status = api.GetStatus();


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