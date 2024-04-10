// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text;
using Cours;
using MyUsers;

const int PORT_ADDR = 4000;
IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, PORT_ADDR);
List<Room> rooms = new List<Room>();


using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
{
    ConsoleColor colorServer = ConsoleColor.DarkYellow;

    serverSocket.Bind(endPoint);
    serverSocket.Listen();

    ColorMessage("Server Start", colorServer);
    rooms.Add(new Room());

    while (true)
    {
        Thread thread = new Thread(() => Method(serverSocket));
        thread.Start();

        while (rooms.Last().UsersList.Count < 2) { Thread.Sleep(10); }

        ColorMessage("Create Room", colorServer);
        rooms.Add(new Room());
    }

    void Method(Socket servSocket)
    {
        ConsoleColor colorUser_1 = ConsoleColor.Green;
        ConsoleColor colorUser_2 = ConsoleColor.Blue;

        FieldGame fieldGame = new FieldGame();

        Socket client_A = servSocket.Accept();
        /////////////////////////////////////////////////////////////////
        byte[] bufferUser_1 = new byte[1024];
        int size = client_A.Receive(bufferUser_1);
        //////////////////////////////// Player_1
        MyUsers.User user_1 = new User(Encoding.Unicode.GetString(bufferUser_1, 0, size), client_A);//////
        rooms.Last().AddToRoom(user_1);

        PlayerCours user_1Cours = new PlayerCours();
        user_1Cours.PlayerLogin = user_1.Login;
        user_1Cours.Index_X = user_1Cours.Index_Y = -1;

        string strConnectPlayer = "Connect Player";
        user_1Cours.ServerCommandLine = strConnectPlayer + " 1";
        ColorMessage(user_1Cours.ToString(), colorUser_1);

        user_1.Socket.Send(Encoding.Unicode.GetBytes(user_1Cours.WriteToJSON()));


        Socket client_B = servSocket.Accept();
        //////////////////////////////////////////////
        byte[] bufferUser_2 = new byte[1024];
        size = client_B.Receive(bufferUser_2);
        //MyUsers.User user_2 = new User("Player_2", client_B);
        MyUsers.User user_2 = new User(Encoding.Unicode.GetString(bufferUser_2, 0, size), client_B);

        rooms.Last().AddToRoom(user_2);

        PlayerCours user_2Cours = new PlayerCours();
        user_2Cours.PlayerLogin = user_2.Login;
        user_2Cours.Index_X = user_2Cours.Index_Y = -1;

        user_2Cours.ServerCommandLine = strConnectPlayer + " 2";
        ColorMessage(user_2Cours.ToString(), colorUser_2);
        user_2.Socket.Send(Encoding.Unicode.GetBytes(user_2Cours.WriteToJSON()));


        ColorMessage($"Player_1 <- {user_2Cours.WriteToJSON()}", colorUser_1);
        user_1.Socket.Send(Encoding.Unicode.GetBytes(user_2Cours.WriteToJSON()));

        string messageFromUser_1 = string.Empty;
        string messageFromUser_2 = string.Empty;

        while (true)
        {
            try
            {
                ColorMessage("waiting messange from Player_1", colorServer);
                ///////////////
                bufferUser_1 = new byte[1024];
                size = user_1.Socket.Receive(bufferUser_1);
                /////////////
                messageFromUser_1 = Encoding.Unicode.GetString(bufferUser_1, 0, size);
                ColorMessage($"Player_1 -> {messageFromUser_1}", colorUser_1);

                user_1Cours = user_1Cours.ReadWithJSON(messageFromUser_1);
                fieldGame.RecordInField(user_1Cours.Index_Y, user_1Cours.Index_X, user_1Cours.PlayerSimbol);
                // якщо IsWin = true відправляе повідомлення про кінець гри
                if (fieldGame.IsWin(user_1Cours.PlayerSimbol))
                {
                    messageFromUser_1 = "Win";

                    user_1Cours.ServerCommandLine = "Loss";
                    messageFromUser_2 = user_1Cours.WriteToJSON();
                }
                else if (fieldGame.IsFull())//перевіряе чи не заповнене поле
                {
                    messageFromUser_1 = user_1Cours.ServerCommandLine = "Game Over";
                    messageFromUser_2 = user_1Cours.WriteToJSON();
                }
                else
                {
                    messageFromUser_1 = "false";

                    user_1Cours.ServerCommandLine = "true";
                    messageFromUser_2 = user_1Cours.WriteToJSON();
                }

                user_1.Socket.Send(Encoding.Unicode.GetBytes(messageFromUser_1));
                ColorMessage($"Player_1 <- {messageFromUser_1}", colorUser_1);

                user_2.Socket.Send(Encoding.Unicode.GetBytes(messageFromUser_2));
                ColorMessage($"Player_2 <- {messageFromUser_2}", colorUser_2);


                ///////////////////////////////////////////////

                ColorMessage("waiting messange from Player_2", colorServer);

                //byte[] bufferUser_2 = new byte[1024];
                //int size = user_2.Socket.Receive(bufferUser_2);
                bufferUser_2 = new byte[1024];
                size = user_2.Socket.Receive(bufferUser_2);


                messageFromUser_2 = Encoding.Unicode.GetString(bufferUser_2, 0, size);
                ColorMessage($"Player_2 -> {messageFromUser_2}", colorUser_2);


                user_2Cours = user_2Cours.ReadWithJSON(messageFromUser_2);
                fieldGame.RecordInField(user_2Cours.Index_Y, user_2Cours.Index_X, user_2Cours.PlayerSimbol);
                // якщо IsWin = true відправляе повідомлення про кінець гри
                if (fieldGame.IsWin(user_2Cours.PlayerSimbol))
                {
                    user_2Cours.ServerCommandLine = "Loss";
                    messageFromUser_1 = user_2Cours.WriteToJSON();

                    messageFromUser_2 = "Win";
                }
                else if (fieldGame.IsFull())//перевіряе чи не заповнене поле
                {
                    user_2Cours.ServerCommandLine = messageFromUser_2 = "Game Over";
                    messageFromUser_1 = user_2Cours.WriteToJSON();
                }
                else
                {
                    user_2Cours.ServerCommandLine = "true";
                    messageFromUser_1 = user_2Cours.WriteToJSON();
                    messageFromUser_2 = "false";
                }

                user_2.Socket.Send(Encoding.Unicode.GetBytes(messageFromUser_2));
                ColorMessage($"Player_2 <- {messageFromUser_2}", colorUser_2);

                user_1.Socket.Send(Encoding.Unicode.GetBytes(messageFromUser_1));
                ColorMessage($"Player_1 <- {messageFromUser_1}", colorUser_1);

            }
            catch (Exception ex)
            {
                if (messageFromUser_1.Equals("Clear") || messageFromUser_2.Equals("Clear"))
                {
                    ColorMessage("Catch if Clear", colorServer);
                    //user_1.Socket.Send(Encoding.Unicode.GetBytes("Clear"));
                    ColorMessage("Player_1 <- Clear", colorUser_1);
                    Task.Delay(10);
                    //user_2.Socket.Send(Encoding.Unicode.GetBytes("Clear"));
                    ColorMessage("Player_2 <- Clear", colorUser_2);

                    fieldGame.ClearField();

                    //user_1.Socket.Send(Encoding.Unicode.GetBytes("true"));
                    //ColorMessage("Player_1 <- true", colorUser_1);

                    //user_2.Socket.Send(Encoding.Unicode.GetBytes("false"));
                    //ColorMessage("Player_2 <- false", colorUser_2);

                    continue;
                }

                ColorMessage(ex.Message, ConsoleColor.Red);
                Console.ReadLine();
                continue;
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




