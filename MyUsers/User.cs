using System.Net.Sockets;

namespace MyUsers
{
    public class User
    {
        public string Login { get; set; }
        public Socket Socket { get; set; }

        public User()
        {
            Login = string.Empty;
            Socket = null;
        }

        public User(string login, Socket s)
        {
            Login = login;
            Socket = s;
        }
    }
}
