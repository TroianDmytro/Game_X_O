using System.Net.Sockets;
using System.Text;
using System.Net;
using Game.Model;
using Game.Controler;
using System.Windows.Forms;
using Cours;
using Player.View;
using Microsoft.VisualBasic.Logging;
using MyConnected;


namespace Game
{
    public partial class Players : Form
    {
        public static Socket? SocketPlayer {  get; private set ; }
        public static ConnectingToServer? Conn { get; private set; }

        PlayingField playingField;

        public PlayerCours playerCours;
        public static int P { get; private set; }
        EnterLogin login;

        public Players()
        {
            InitializeComponent();
            this.splitContainer1.Panel1.Enabled = false;
            playerCours = new PlayerCours();

        }
        private void Player_Load(object sender, EventArgs e)
        {
            playingField = new PlayingField(splitContainer1.Panel1);
            playingField.Btms(3, 3);

        }


        //Підключення до сервера
        private void Btn_connected_Click(object sender, EventArgs e)
        {
            newGameToolStripMenuItem.Enabled = false;
            Btn_connected.Enabled = false;
            label1.Text = "Wait connecting...";

            if (Conn == null)
            {
                Conn = new ConnectingToServer();

                login = new EnterLogin(this);
                login.ShowDialog();

                if ((SocketPlayer = Conn.ConnectingToServ())!=null)
                {
                    if(Players.Conn.SendMessenge(SocketPlayer, playerCours.PlayerLogin))
                    {
                        label1.Text = "You connecting.";
                    }
                    else
                    {
                        label1.Text = "Not connecting";
                        Conn.DisconnectingFromServer(SocketPlayer);
                        Conn = null;
                        Btn_connected.Enabled = true;
                        return;
                    }
                }
            }

            Task.Run(() =>
            {
                string command = string.Empty;
                while (true)
                {
                    try
                    {
                        command = Players.Conn.GetMessenge(Players.SocketPlayer);

                        playerCours = playerCours.ReadWithJSON(command);

                        command = playerCours.ServerCommandLine;
                        playingField.WriteCoursOnField(playerCours.Index_Y, playerCours.Index_X, playerCours.PlayerSimbol);
                    }
                    catch
                    { }

                    if (command.Equals("Game Over") || command.Equals("Win") || command.Equals("Loss"))
                    {
                        if (!command.Equals("Game Over"))
                        {
                            command = $"You {command}";
                        }

                        DialogResult result = MessageBox.Show(command, login.Tb_login.Text);
                        if (result == DialogResult.OK)
                        {
                            //Conn.SendMessenge(Players.SocketPlayer, "Clear");------
                            playingField.ClearField();
                            if (Players.P == 1)
                            {
                                splitContainer1.Panel1.Enabled = true;
                            }
                        }
                    }
                    else if (command.Equals("true"))
                    {
                        Action action = () =>
                        {
                            splitContainer1.Panel1.Enabled = true;
                            label1.Text = "Your course.";
                        };
                        Invoke(action);
                    }
                    else if (command.Equals("false"))
                    {
                        Action action = () =>
                        {
                            label1.Text = "Opponent`s move.";
                        };
                        Invoke(action);
                    }

                    if (command.Equals("Connect Player 1"))
                    {
                        Invoke(() =>
                        {
                            P = 1;
                            playerCours.PlayerSimbol = 'X';
                            this.Text = playerCours.PlayerLogin + " your X";
                            playingField.currentPlayer = playerCours;
                            label1.Text += "Wait Player 2";
                        });
                    }
                    else if (command.Equals("Connect Player 2") && Players.P == 0)
                    {
                        Action action = () =>
                        {
                            P = 2;
                            playerCours.PlayerSimbol = 'O';
                            this.Text = playerCours.PlayerLogin + " your O";
                            playingField.currentPlayer = playerCours;

                        };
                        Invoke(action);
                    }
                };
            });
        }

        private void Player_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Conn != null)
            {
                Conn.DisconnectingFromServer(Players.SocketPlayer);
            }
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Players.Conn.SendMessenge(Players.SocketPlayer, "Clear");
            playingField.ClearField();
        }

        private void Btn_connected_MouseEnter(object sender, EventArgs e)
        {
            playingField.ButtonStyleEnter(sender, e);
        }

        private void Btn_connected_MouseLeave(object sender, EventArgs e)
        {
            playingField.ButtonStyleLeave(sender, e);
        }
    }
}
