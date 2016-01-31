namespace ProcHelper
{
    #if DEBUG
    public partial class Credentials
    {
		// Update the account credentials when debugging
		
        public static Credentials Debug = new Credentials
        {
            Username = null,
            Password = null,
            Domain = null,
        };
    }
#endif
}
