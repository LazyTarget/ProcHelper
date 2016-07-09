namespace Sandbox.Console
{
    public class ZeroConfResolveArguments
    {
        public ZeroConfResolveArguments()
        {
            Host = "localhost";
            Type = "_workstation._tcp";
            Domain = "local";
        }

        public string Host { get; set; }
        public string Type { get; set; }
        public string Domain { get; set; }
    }


    public class ZeroConfPublishArguments
    {
        public ZeroConfPublishArguments()
        {
            Type = "_workstation._tcp";
            Name = "Foobar";
            Port = 9000;
            Domain = "local";
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public int Port { get; set; }
        public string Domain { get; set; }
    }
}
