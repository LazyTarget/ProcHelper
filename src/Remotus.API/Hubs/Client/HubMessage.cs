namespace Remotus.API.Hubs.Client
{
    public class HubMessage
    {
        public string Hub { get; set; }

        public string Method { get; set; }

        public object[] Args { get; set; }

        public bool Queuable { get; set; }
    }
}
