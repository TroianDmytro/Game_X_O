using Cours;

namespace Game.Controler
{
    public class PlayingField
    {
        //масив кнопок з яких складается поле
        List<List<Button>> buttons = new List<List<Button>>();
        // панель на якій буде поле для гри
        Panel _panel;
        //поточний гравець
        public PlayerCours currentPlayer;

        public PlayingField( Panel p)
        {
            _panel = p;
        }

        //створюе масив кнопок 
        public void Btms(int width, int height)
        {
            int positionLeft = 0;
            int positionTop = 0;

            for (int i = 0; i < height; i++)
            {
                buttons.Add(new List<Button>());
                for (int j = 0; j < width; j++)
                {
                    buttons[i].Add(new Button()
                    {
                        Location = new Point(positionLeft, positionTop)

                    });
                    buttons[i][j].Width = 100;
                    buttons[i][j].Height = 100;
                    buttons[i][j].BackColor = Color.Green;
                    //buttons[i][j].Enabled = false;
                    buttons[i][j].Font = new Font(buttons[i][j].Font.Name.ToString(), 20, FontStyle.Bold);
                    buttons[i][j].Name = $"{i} {j}";

                    _panel.Controls.Add(buttons[i][j]);
                    //додае обробник подиї натискання на кнопку
                    buttons[i][j].Click += TicTocToe;
                    buttons[i][j].MouseEnter += ButtonStyleEnter;
                    buttons[i][j].MouseLeave += ButtonStyleLeave;

                    positionLeft += buttons[i][j].Size.Width;
                }
                positionTop += buttons[0][0].Size.Height;
                positionLeft = 0;
            }
        }

        public void ButtonStyleEnter(object? sender, EventArgs e)
        {
            Button b = sender as Button;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 2;
            b.FlatAppearance.BorderColor = System.Drawing.Color.Red;
        }

        public void ButtonStyleLeave(object? sender, EventArgs e)
        {
            Button b = sender as Button;
            b.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
        }

        // подія хід гравця
        private void TicTocToe(object? sender, EventArgs e)
        {
            Button? b = sender as Button;
            if (b is Button) 
            {
                b.Text = currentPlayer.PlayerSimbol.ToString();

                currentPlayer.Index_Y = int.Parse(b.Name.Split(' ').First());
                currentPlayer.Index_X = int.Parse(b.Name.Split(' ').Last());
                b.Enabled = false;
                currentPlayer.PlayerIndex = Players.P;

                string strMsg = currentPlayer.WriteToJSON();
                Players.Conn.SendMessenge(Players.SocketPlayer, strMsg);

                _panel.Enabled = false;
            }

            Task.WaitAll();
        }

        // записує в buttons хід противника
        public void WriteCoursOnField(int y, int x, char ch)
        {
            if (y >= 0 && x >= 0)
            {
                buttons[y][x].Text = ch.ToString();
                buttons[y][x].Enabled = false;
            }
        }

        // очищає поле
        public void ClearField()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                for (int j = 0; j < buttons[i].Count; j++)
                {
                    buttons[i][j].Text = string.Empty;
                    buttons[i][j].Enabled = true;
                }
            }
        }
        
        public void AcessToButton(bool b)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                for (int j = 0; j < buttons[i].Count; j++)
                {
                    buttons[i][j].Enabled = b;
                }
            }
            
        }
        
        private void Enabl(List<List<Button>> b)
        {
            for (int i = 0; i < b.Count; i++)
            {
                for (int j = 0; j < b[i].Count; j++)
                {
                    (b[i][j] as Button).Enabled = false;
                }
            }
        }
    }
}
