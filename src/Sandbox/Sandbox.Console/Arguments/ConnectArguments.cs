namespace Sandbox.Console
{
    public class ConnectArguments
    {
        public ConnectArguments()
        {
            Host = "localhost";
            Port = 9000;
        }

        public string Host { get; set; }
        public int Port { get; set; }
    }
}