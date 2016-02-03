namespace FullCtrl.Base
{
    #if DEBUG
    public partial class Credentials
    {
		// Update the account credentials when debugging
		
        public static Credentials Debug = new Credentials
        {
            Username = "Peter",
            Password = "814cKGH0$t",
            Domain = null,
        };
    }
#endif
}
