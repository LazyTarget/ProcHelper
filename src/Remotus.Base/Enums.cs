namespace Remotus.Base
{
    public enum ServiceStatus
    {
        None,
        Initializing,
        Initialized,
        Starting,
        Running,
        Stopping,
        Stopped,
    }

    public enum ServiceInstallState
    {
        None,
        Installing,
        Installed,
        Updating,
        Uninstalling,
        Uninstalled,
    }
}
