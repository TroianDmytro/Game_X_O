using System.Net.Sockets;
using System.Text;
using System.Net;
using Game.Model;
using Game.Controler;
using System.Windows.Forms;
using Cours;
using Player.View;
using Microsoft.VisualBasic.Logging;


namespace Game
{
    public partial class Players : Form
    {
        //const string IP_SERVER_ADDR = "127.0.0.1";
        //const int PORT_SERVER_ADDR = 4000;
        public Socket socket;
        public static ConnectingToServer Conn { get; private set; }
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

                if (Conn.Connecting())
                {
                    label1.Text = "You connecting.";
                    login = new EnterLogin(this);
                    login.ShowDialog();
                }
                else
                {
                    label1.Text = "Not connecting";
                    return;
                }

            }
            Task.Run(() =>
            {
                string command = string.Empty;
                while (true)
                {
                    try
                    {
                        command = Players.Conn.GetMessenge();
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

                        DialogResult result = MessageBox.Show(command, this.Text);
                        if (result == DialogResult.OK)
                        {
                            Conn.SendMessenge("Clear");
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
                        });
                    }

                    if (command.Equals("Connect Player 2") && Players.P == 0)
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
                    else if (command.Equals("Connect Player 2"))
                    {
                        Action action = () =>
                        {
                            this.splitContainer1.Panel1.Enabled = true;
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
                Conn.DisconnectingFromServer();
            }
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Players.Conn.SendMessenge("Clear");
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
