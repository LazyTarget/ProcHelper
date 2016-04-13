using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Remotus.Base.Scripting
{
    [Serializable]
    public class ExecuteFunctionScriptTask : ScriptTaskBase, IXmlSerializable
    {
        public override string Name => "ExecuteFunction";

        public string ClientID { get; set; }

        public string PluginID { get; set; }

        public string FunctionID { get; set; }
        
        [XmlIgnore]
        public IFunctionArguments Arguments { get; set; }


        public override async Task<IResponseBase> Execute(IExecutionContext context, IParameterCollection parameterCollection)
        {
            try
            {
                if (Arguments?.Parameters != null && parameterCollection != null)
                {
                    foreach (var parameter in parameterCollection)
                    {
                        Arguments.Parameters[parameter.Key] = parameter.Value;
                    }
                }

                var response = ClientID != null
                    ? await context.Remotus.ExecuteRemoteFunction(ClientID, PluginID, FunctionID, Arguments)
                    : await context.Remotus.ExecuteLocalFunction(PluginID, FunctionID, Arguments);
                return response;
            }
            catch (Exception ex)
            {
                var result = DefaultResponseBase.CreateError(DefaultError.FromException(ex));
                return result;
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            string elem = "ExeTask";

            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == elem)
            {
                //Name = reader[nameof(Name)];
                ClientID = reader[nameof(ClientID)];
                PluginID = reader[nameof(PluginID)];
                FunctionID = reader[nameof(FunctionID)];

                if (reader.ReadToDescendant("Parameters"))
                {
                    while (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == "Parameter")
                    {
                        var parameter = new Parameter();
                        parameter.Name = reader[nameof(parameter.Name)];
                        parameter.Value = reader[nameof(parameter.Value)];
                        var typeStr = reader[nameof(parameter.Type)];
                        parameter.Type = !string.IsNullOrEmpty(typeStr)
                            ? Type.GetType(typeStr, throwOnError: true)
                            : null;

                        var bstr = reader["Required"];
                        bool b;
                        bool.TryParse(bstr, out b);
                        parameter.Required = b;

                        Arguments = Arguments ?? new FunctionArguments();
                        Arguments.Parameters = Arguments.Parameters ?? new ParameterCollection();
                        Arguments.Parameters.Add(parameter.Name, parameter);
                        reader.Read();
                    }
                    reader.Read();
                }
            }
        }
        
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(Name), Name);
            writer.WriteElementString(nameof(ClientID), ClientID);
            writer.WriteElementString(nameof(PluginID), PluginID);
            writer.WriteElementString(nameof(FunctionID), FunctionID);

            writer.WriteStartElement(nameof(Arguments.Parameters));
            if (Arguments?.Parameters != null)
            {
                foreach (var pair in Arguments.Parameters)
                {
                    writer.WriteStartElement("Parameter");
                    writer.WriteElementString(nameof(pair.Value.Name), pair.Value.Name);
                    writer.WriteElementString(nameof(pair.Value.Value), pair.Value.Value?.ToString());
                    writer.WriteElementString(nameof(pair.Value.Type), pair.Value.Type?.ToString());
                    writer.WriteElementString(nameof(pair.Value.Required), pair.Value.Required.ToString());
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }
    }
}