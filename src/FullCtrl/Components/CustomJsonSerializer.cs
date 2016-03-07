using System.IO;
using System.Linq;
using FullCtrl.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace FullCtrl
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

            //_container = new Container();
            _container = new UnityAdaptorContainer();

            _serializer = new Newtonsoft.Json.JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
            };
            _serializer.Converters.Add(new StringEnumConverter());
            _serializer.Converters.Add(new IoCJsonConverter(_container));
            _serializer.Converters.Add(new BitmapConverter());
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
}