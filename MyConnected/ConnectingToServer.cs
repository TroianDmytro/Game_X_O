using System.Net.Sockets;
using System.Text;

namespace MyConnected
{
    public class ConnectingToServer
    {
        const string IP_SERVER_ADDR = "127.0.0.1";
        const int PORT_SERVER_ADDR = 4000;
        Socket socket;

        public ConnectingToServer() 
        { 
        }

        public Socket ConnectingToServ()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.ConnectAsync(IP_SERVER_ADDR, PORT_SERVER_ADDR);
            Task.WaitAll();
            Task.Delay(10);

            return socket;
        }

        public bool IsConnecting(Socket s)
        {
            if (s != null && s.Connected)
            {
                return true;
            }
            return false;
        }


        public bool SendMessenge(Socket s, string msg)
        {
            if (s != null && s.Connected)
            {
                s.Send(Encoding.Unicode.GetBytes(msg));
                return true;
            }

            return false;
        }

        public string GetMessenge(Socket s)
        {
            if (s != null && s.Connected)
            {
                byte[] buffer = new byte[1024];
                var size = s.Receive(buffer, SocketFlags.None);
                string temp = Encoding.Unicode.GetString(buffer, 0, size);

                return temp;
            }

            return string.Empty;
        }

        public bool DisconnectingFromServer(Socket s)
        {
            bool res = false;

            try
            {
                if (s!= null && s.Connected)
                {
                    s.Shutdown(SocketShutdown.Both);
                    s.Close();
                }
                res = true;
            }
            catch (Exception)
            { }

            return res;
        }
    }
}
