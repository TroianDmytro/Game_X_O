// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text;
using Cours;
using MyUsers;

const int PORT_ADDR = 4000;
IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, PORT_ADDR);
List<Room> rooms = new List<Room>();
MyConnected.ConnectingToServer connecting = new MyConnected.ConnectingToServer();

using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
{
    ConsoleColor colorServer = ConsoleColor.DarkYellow;

    serverSocket.Bind(endPoint);
    serverSocket.Listen();

    ColorMessage("Server Start", colorServer);
    rooms.Add(new Room());

    try
    {
        while (true)
        {
            Thread thread = new Thread(() => Method(serverSocket, rooms.Count));
            thread.Start();

            if (rooms.Count > 0)
            {
                while (rooms.Last().UsersList.Count < 2) { Thread.Sleep(10); }
            }

            ColorMessage("Create Room", colorServer);
            rooms.Add(new Room());
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("try catch #1");
        ColorMessage(ex.Message, ConsoleColor.Red);
    }
   
    // метод обробки одниєї ігрової кімнати
    void Method(Socket servSocket,int indexRoom)
    {
        ConsoleColor colorUser_1 = ConsoleColor.Green;
        ConsoleColor colorUser_2 = ConsoleColor.Blue;

        FieldGame fieldGame = new FieldGame();
        // Player 1 connecting
        Socket client_A = servSocket.Accept();

        MyUsers.User user_1 = new User(connecting.GetMessenge(client_A), client_A);
        rooms.Last().AddToRoom(user_1);

        PlayerCours user_1Cours = new PlayerCours();
        user_1Cours.PlayerLogin = user_1.Login;
        user_1Cours.Index_X = user_1Cours.Index_Y = -1;

        string strConnectPlayer = "Connect Player";
        user_1Cours.ServerCommandLine = strConnectPlayer + " 1";
        ColorMessage(user_1Cours.ToString(), colorUser_1);

        connecting.SendMessenge(user_1.Socket,user_1Cours.WriteToJSON());

        // Player 2 connecting
        Socket client_B = servSocket.Accept();
        
        MyUsers.User user_2 = new User(connecting.GetMessenge(client_B), client_B);

        rooms.Last().AddToRoom(user_2);

        PlayerCours user_2Cours = new PlayerCours();
        user_2Cours.PlayerLogin = user_2.Login;
        user_2Cours.Index_X = user_2Cours.Index_Y = -1;

        user_2Cours.ServerCommandLine = strConnectPlayer + " 2";
        ColorMessage(user_2Cours.ToString(), colorUser_2);
        connecting.SendMessenge(user_2.Socket,user_2Cours.WriteToJSON());

        user_1Cours.ServerCommandLine = "true";
        ColorMessage($"Player_1 <- {user_1Cours.WriteToJSON()}", colorUser_1);
        connecting.SendMessenge(user_1.Socket,user_1Cours.WriteToJSON());

        string messageFromUser_1;
        string messageFromUser_2;

        while (true)
        {
            messageFromUser_1 = string.Empty;
            messageFromUser_2 = string.Empty;
            bool clear = false;// визначає потрибно очищувати ігрове поле чи ні (true - так, false - ні)

            try
            {
                ColorMessage("waiting messange from Player_1", colorServer);
                
                messageFromUser_1 = connecting.GetMessenge(user_1.Socket);
                ColorMessage($"Player_1 -> {messageFromUser_1}", colorUser_1);

                user_1Cours = user_1Cours.ReadWithJSON(messageFromUser_1);
                fieldGame.RecordInField(user_1Cours.Index_Y, user_1Cours.Index_X, user_1Cours.PlayerSimbol);

                // якщо IsWin = true відправляе повідомлення про кінець гри
                if (fieldGame.IsWin(user_1Cours.PlayerSimbol))
                {
                    messageFromUser_1 = "Win";

                    user_1Cours.ServerCommandLine = "Loss";
                    messageFromUser_2 = user_1Cours.WriteToJSON();
                    clear = true;
                    
                }
                else if (fieldGame.IsFull())//перевіряе чи не заповнене поле
                {
                    messageFromUser_1 = user_1Cours.ServerCommandLine = "Game Over";
                    messageFromUser_2 = user_1Cours.WriteToJSON();
                    clear = true;
                }
                else
                {
                    messageFromUser_1 = "false";

                    user_1Cours.ServerCommandLine = "true";
                    messageFromUser_2 = user_1Cours.WriteToJSON();
                }

                connecting.SendMessenge(user_1.Socket, messageFromUser_1);
                ColorMessage($"Player_1 <- {messageFromUser_1}", colorUser_1);

                connecting.SendMessenge(user_2.Socket, messageFromUser_2);
                ColorMessage($"Player_2 <- {messageFromUser_2}", colorUser_2);

                if (clear)
                {
                    fieldGame.CreateField();
                    continue;
                }
                ///////////////////////////////////////////////

                ColorMessage("waiting messange from Player_2", colorServer);

                messageFromUser_2 = connecting.GetMessenge(user_2.Socket);
                ColorMessage($"Player_2 -> {messageFromUser_2}", colorUser_2);

                user_2Cours = user_2Cours.ReadWithJSON(messageFromUser_2);
                fieldGame.RecordInField(user_2Cours.Index_Y, user_2Cours.Index_X, user_2Cours.PlayerSimbol);
                // якщо IsWin = true відправляе повідомлення про кінець гри
                if (fieldGame.IsWin(user_2Cours.PlayerSimbol))
                {
                    user_2Cours.ServerCommandLine = "Loss";
                    messageFromUser_1 = user_2Cours.WriteToJSON();

                    messageFromUser_2 = "Win";
                    clear = true;
                }
                else if (fieldGame.IsFull())//перевіряе чи не заповнене поле
                {
                    user_2Cours.ServerCommandLine = messageFromUser_2 = "Game Over";
                    messageFromUser_1 = user_2Cours.WriteToJSON();
                    clear = true;
                }
                else
                {
                    user_2Cours.ServerCommandLine = "true";
                    messageFromUser_1 = user_2Cours.WriteToJSON();
                    messageFromUser_2 = "false";
                }

                connecting.SendMessenge(user_2.Socket, messageFromUser_2);

                ColorMessage($"Player_2 <- {messageFromUser_2}", colorUser_2);

                connecting.SendMessenge(user_1.Socket, messageFromUser_1);
                ColorMessage($"Player_1 <- {messageFromUser_1}", colorUser_1);

                if (clear)
                {
                    fieldGame.CreateField();
                }
            }
            catch (Exception ex)
            {
                ColorMessage(ex.Message, ConsoleColor.Red);
                if (user_1.Socket.Connected)
                {
                    connecting.SendMessenge(user_1.Socket, "Disconnecting");
                    user_1.Socket.Shutdown(SocketShutdown.Both);
                    user_1.Socket.Close();
                    ColorMessage("user_1 Disconect", colorUser_1);
                }
                else
                {
                    ColorMessage("user_1 Disconect", colorUser_1);
                }

                if (user_2.Socket.Connected)
                {
                    connecting.SendMessenge(user_2.Socket, "Disconnecting");
                    user_2.Socket.Shutdown(SocketShutdown.Both);
                    user_2.Socket.Close();
                    ColorMessage("user_2 Disconect", colorUser_2);
                }
                else
                {
                    ColorMessage("user_2 Disconect", colorUser_2);
                }

                rooms.RemoveAt(--indexRoom);
                ColorMessage("room delete", colorServer);
                return;
            }

        }
    }

    void ColorMessage(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}




