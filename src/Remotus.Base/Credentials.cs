using System.Linq;
using System.Security;

namespace FullCtrl.Base
{
    public partial class Credentials
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public SecureString SecurePassword
        {
            get
            {
                if (Password == null)
                    return null;
                var secure = new SecureString();
                Password.ToList().ForEach(secure.AppendChar);
                return secure;
            }
        }

        public string Domain { get; private set; }


        public bool IsEmpty
        {
            get { return Username == null && Password == null && Domain == null; }
        }

    }
}
