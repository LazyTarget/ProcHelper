using System.Net;
using System.Security;

namespace Remotus.Base.Payloads
{
    public class AuthCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public SecureString SecurePassword { get; set; }
        public string Domain { get; set; }


        public static AuthCredentials Create(NetworkCredential credential)
        {
            if (credential == null)
                return null;
            var result = new AuthCredentials();
            result.UserName = credential.UserName;
            result.Password = credential.Password;
            result.SecurePassword = credential.SecurePassword;
            result.Domain = credential.Domain;
            return result;
        }
    }
}
