using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Remotus.Base;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace Remotus
{
    /// <summary>
	/// Default JSON serializer for request bodies
	/// Doesn't currently use the SerializeAs attribute, defers to Newtonsoft's attributes
	/// </summary>
	public class CustomJsonSerializer : ISerializer, IDeserializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;
        private IContainer _container;

        /// <summary>
        /// Default serializer
        /// </summary>
        public CustomJsonSerializer()
        {
            ContentType = "application/json";
            
            var settings = new CustomJsonSerializerSettings();
            _container = settings.Container;
            _serializer = Newtonsoft.Json.JsonSerializer.Create(settings.Settings);
        }

        /// <summary>
        /// Default serializer with overload for allowing custom Json.NET settings
        /// </summary>
        public CustomJsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
            : this()
        {
            ContentType = "application/json";
            _serializer = serializer;
        }

        public JsonSerializerSettings GetSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters = _serializer.Converters;
            settings.MissingMemberHandling = _serializer.MissingMemberHandling;
            settings.NullValueHandling = _serializer.NullValueHandling;
            settings.DefaultValueHandling = _serializer.DefaultValueHandling;
            settings.ContractResolver = _serializer.ContractResolver;
            return settings;
        }

        public IContainer Container
        {
            get { return _container; }
            set
            {
                _container = value;

                var converter = _serializer.Converters.OfType<IoCJsonConverter>().ToList();
                converter.ForEach(x => x.Container = value);
            }
        }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string RootElement { get; set; }
        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// Content type for serialized content
        /// </summary>
        public string ContentType { get; set; }


        /// <summary>
        /// Serialize the object as JSON
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>JSON as String</returns>
        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                jsonTextWriter.Formatting = Formatting.Indented;
                jsonTextWriter.QuoteChar = '"';

                _serializer.Serialize(jsonTextWriter, obj);

                var result = stringWriter.ToString();
                return result;
            }
        }

        public T Deserialize<T>(IRestResponse response)
        {
            var json = response.Content;
            using (var stringReader = new StringReader(json))
            using (var jsonTextReader = new JsonTextReader(stringReader))
            {
                var result = _serializer.Deserialize<T>(jsonTextReader);
                return result;
            }
        }
    }


    public class CustomJsonSerializerSettings
    {
        public IContainer Container { get; private set; }

        public JsonSerializerSettings Settings { get; private set; }


        public CustomJsonSerializerSettings()
        {
            Container = new UnityAdaptorContainer();
            Container.Bind(typeof(IResponseBase), typeof(DefaultResponseBase<object>));
            Container.Bind(typeof(IResponseBase<>), typeof(DefaultResponseBase<>));
            Container.Bind(typeof(IError), typeof(DefaultError));
            Container.Bind(typeof(ILink), typeof(DefaultLink));
            Container.Bind(typeof(IProcessDto), typeof(ProcessDto));
            Container.Bind(typeof(IParameter), typeof(Base.Parameter));
            Container.Bind(typeof(IParameterCollection), typeof(ParameterCollection));
            Container.Bind(typeof(IFunctionResult), typeof(FunctionResult));
            Container.Bind(typeof(IFunctionArguments), typeof(FunctionArguments));
            Container.Bind(typeof(IFunctionDescriptor), typeof(API.v1.FunctionDescriptor));
            Container.Bind(typeof(IPlugin), typeof(API.v1.FunctionPluginDescriptor));
            Container.Bind(typeof(IFunctionPlugin), typeof(API.v1.FunctionPluginDescriptor));
            Container.Bind(typeof(IServicePlugin), typeof(API.v1.ServicePluginDescriptor));


            Settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
                Formatting = Formatting.Indented,
            };
            //Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            Settings.Converters.Add(new API.v1.TypeInfoJsonConverter());
            Settings.Converters.Add(new API.v1.ParameterConverter());
            Settings.Converters.Add(new API.v1.ParameterCollectionConverter());
            Settings.Converters.Add(new API.v1.FunctionDescriptorConverter());
            Settings.Converters.Add(new API.v1.FunctionPluginConverter());
            Settings.Converters.Add(new API.v1.ProcessToNullConverter());
            Settings.Converters.Add(new API.v1.ResponseBaseConverter());
            Settings.Converters.Add(new StringEnumConverter { AllowIntegerValues = true });
            Settings.Converters.Add(new IoCJsonConverter(Container));
            Settings.Converters.Add(new BitmapConverter());
        }


        public static implicit operator JsonSerializerSettings(CustomJsonSerializerSettings self)
        {
            return self?.Settings;
        }

    }
}