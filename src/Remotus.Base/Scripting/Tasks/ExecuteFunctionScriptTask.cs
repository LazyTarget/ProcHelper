using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Remotus.Base.Scripting
{
    [Serializable]
    public class ExecuteFunctionScriptTask : ScriptTaskBase, IXmlSerializable
    {
        public override string Name => "ExecuteFunctionTask";

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

                var response = !string.IsNullOrWhiteSpace(ClientID)
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


        #region IXmlSerializable

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == Name)
            {
                var elem = (XElement)XNode.ReadFrom(reader);
                
                ClientID = elem.Element(nameof(ClientID))?.Value;
                PluginID = elem.Element(nameof(PluginID))?.Value;
                FunctionID = elem.Element(nameof(FunctionID))?.Value;

                var paramsElem = elem.Element("Parameters");
                if (paramsElem != null)
                {
                    var paramsElements = paramsElem.Elements("Parameter");
                    foreach (var e in paramsElements)
                    {
                        var parameter = new Parameter();
                        parameter.Name = e.Element(nameof(parameter.Name))?.Value;
                        parameter.Value = e.Element(nameof(parameter.Value))?.Value;
                        var typeStr = e.Element(nameof(parameter.Type))?.Value;
                        parameter.Type = !string.IsNullOrEmpty(typeStr)
                            ? Type.GetType(typeStr, throwOnError: true)
                            : null;

                        var bstr = e.Element("Required")?.Value;
                        bool b;
                        bool.TryParse(bstr, out b);
                        parameter.Required = b;

                        Arguments = Arguments ?? new FunctionArguments();
                        Arguments.Parameters = Arguments.Parameters ?? new ParameterCollection();
                        Arguments.Parameters.Add(parameter.Name, parameter);
                    }

                }
            }
        }
        
        public void WriteXml(XmlWriter writer)
        {
            //writer.WriteElementString(nameof(Name), Name);
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

        #endregion

    }
}