
using Microsoft.VisualBasic.Logging;
using Player.View;
using System.Net.Sockets;
using System.Text;

namespace Game.Model
{
    //public class ConnectingToServer
    //{
    //    const string IP_SERVER_ADDR = "127.0.0.1";
    //    const int PORT_SERVER_ADDR = 4000;
    //    Socket socket;

    //    public ConnectingToServer()
    //    {
            
    //    }

    //    public bool ConnectingToServ()
    //    {
    //        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        socket.ConnectAsync(IP_SERVER_ADDR, PORT_SERVER_ADDR);
    //        Task.WaitAll();
    //        Task.Delay(10);
            
    //        return socket.Connected;
    //    }

    //    public bool IsConnecting()
    //    {
    //        if (socket != null && socket.Connected)
    //        {
    //            return true;
    //        }
    //        return false;
    //    }


    //    public bool SendMessenge(string msg)
    //    {
    //        if (socket != null && socket.Connected)
    //        {
    //            socket.Send(Encoding.Unicode.GetBytes(msg));
    //            return true;
    //        }

    //        return false;
    //    }

    //    public string GetMessenge()
    //    {
    //        if (socket != null && socket.Connected)
    //        {
    //            byte[] buffer = new byte[1024];
    //            var size = socket.Receive(buffer,SocketFlags.None);
    //            string temp = Encoding.Unicode.GetString(buffer, 0, size);

    //            return temp;
    //        }

    //        return string.Empty;
    //    }

    //    public bool DisconnectingFromServer()
    //    {
    //        bool res = false;

    //        try
    //        {
    //            if (socket != null && socket.Connected)
    //            {
    //                socket.Shutdown(SocketShutdown.Both);
    //                socket.Close();
    //                res = true;
    //            }
    //        }
    //        catch (Exception)
    //        {}

    //        return res;
    //    }

    //}
}
