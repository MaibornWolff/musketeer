using System.Security;

namespace Musketeer.Config
{
    public class TestUser
    {
        public string Username { get; set; }
        public SecureString Password { get; set; }
    }
}