using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Spotify
{
    public class GetProfileFunction : IFunction<SpotifyAPI.Web.Models.PublicProfile>
    {
        private readonly ModelConverter _modelConverter = new ModelConverter();

        public GetProfileFunction()
        {
            
        }

        public IFunctionDescriptor GetDescriptor()
        {
            return new Descriptor();
        }

        async Task<IFunctionResult> IFunction.Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            var result = await Execute(context, arguments);
            return result;
        }

        public async Task<IFunctionResult<SpotifyAPI.Web.Models.PublicProfile>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                string userID = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.UserID)?.Value;
                var publicProfile = Worker.WebApi.GetPublicProfile(userID);
                //var res = _modelConverter.FromStatusResponse(publicProfile);
                var res = publicProfile;

                var result = new FunctionResult<SpotifyAPI.Web.Models.PublicProfile>();
                result.Arguments = arguments;
                result.Result = res;
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<SpotifyAPI.Web.Models.PublicProfile>();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public class Descriptor : IFunctionDescriptor
        {
            public string ID => "9B902341-2081-403E-B449-3CD1CE0A6B95";
            public string Name => "Get profile";
            public string Version => "1.0.0.0";

            public IParameterCollection GetParameters()
            {
                var res = new Parameters();
                return res;
            }

            IFunction IComponentInstantiator<IFunction>.Instantiate()
            {
                return Instantiate();
            }

            public IFunction<SpotifyAPI.Web.Models.PublicProfile> Instantiate()
            {
                return new GetProfileFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                UserID = new Parameter<string>
                {
                    Name = ParameterKeys.UserID,
                    Required = true,
                    Type = typeof(string),
                    Value = null,
                };
            }

            public IParameter<string> UserID
            {
                get { return this.GetOrDefault<string>(ParameterKeys.UserID); }
                private set { this[ParameterKeys.UserID] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string UserID = "UserID";
        }

        public void Dispose()
        {
            
        }
    }
}
