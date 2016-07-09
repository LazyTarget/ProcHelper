namespace Sandbox.Console
{
    public class DisconnectArguments
    {
        public DisconnectArguments()
        {
            Force = true;
        }

        public bool Force { get; set; }
        public string ForceMessage { get; set; }
    }
}