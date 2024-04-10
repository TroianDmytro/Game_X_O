
namespace MyUsers
{
    public class FieldGame
    {
        List<List<char>> field;
        public int Width { get; set; }
        public int Height { get; set; }

        public FieldGame()
        {
            this.Height = 3;
            this.Width = 3;
            
            this.CreateField();
        }

        //створення поля
        public void CreateField()
        {
            this.field = new List<List<char>>();

            for (int i = 0; i < Height; i++) 
            {
                field.Add(new List<char>());

                for(int j = 0; j < Width; j++)
                {
                    field[i].Add('_');
                }
            }
        }

        // додавання символу на поле
        public void RecordInField(int y, int x, char symbol)
        {
            if (!IsFull())
            {
                if (this.field[y][x] != 'X' || this.field[y][x]!='O')
                {
                    this.field[y][x] = symbol;
                }
            }

        }

        #region RecordInField(MyUsers.User player)
        //public void RecordInField(MyUsers.User player)
        //{
        //    if (!IsFull())
        //    {
        //        if (!char.IsSymbol(field[player.Index_Y, player.Index_X]))
        //        {
        //            field[player.Index_Y, player.Index_X] = player.PlayerSimbol;
        //        }
        //    }

        //}
# endregion RecordInField(MyUsers.User player)

        // перевіряе хід на виграш
        public bool IsWin(char ch)
        {
            bool winDiagonal = true;
            bool rightDiagonal = true;

            for (int q = 0; q < this.field.Count; q++)
            {
                bool winGorizontal = true;

                //перевіряе по горизонталі
                for (int k = 0; k < this.field[q].Count; k++)
                {
                    if (this.field[k][ q] != ch)
                    {
                        winGorizontal = false;
                        break;
                    }
                }

                if (winGorizontal)
                {
                    return winGorizontal;
                }
            }

            //перевіряе по вертикали
            for (int i = 0; i < this.field.Count; i++)
            {
                bool winVertical = true;
                for (int j = 0; j < this.field[i].Count; j++)
                {
                    if (this.field[i][j] != ch)
                    {
                        winVertical = false;
                        break;
                    }
                }

                if (winVertical)
                {
                    return winVertical;
                }

                if (this.field[i][this.field.Count - 1 - i] != ch)
                {
                    rightDiagonal = false;
                }

                if (this.field[i][i] != ch)
                {
                    winDiagonal = false;
                }
            }
            if (winDiagonal)
            {
                //MessageBox.Show(b.Text + "- WIN", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //_form.Text = b.Text + "- WIN";
                //Enabl(buttons);
                return winDiagonal;
            }
            if (rightDiagonal)
            {
                //MessageBox.Show(b.Text + "- WIN", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //_form.Text = b.Text + "- WIN";
                //Enabl(buttons);
                return rightDiagonal;
            }
            return false;
        }


        // перевіряе чи заповнене поле
        public bool IsFull()
        {
            for (int i = 0; i < this.field.Count; i++)
            {
                for (int j = 0; j < this.field[i].Count; j++)
                {
                    if (this.field[i][j] != 'X' && this.field[i][j] != 'O')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void ClearField()
        {
            this.field.Clear();
            CreateField();
        }
    }
}
