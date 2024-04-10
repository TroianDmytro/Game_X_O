using Cours;
using Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Player.View
{
    public partial class EnterLogin : Form
    {
        Players mainForm;
        public EnterLogin(Players f)
        {
            InitializeComponent();
            mainForm = f;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Btn_Enter_Click(object sender, EventArgs e)
        {
            mainForm.playerCours.PlayerLogin = Tb_login.Text;
            Task.Run(() =>
            {
                Players.Conn.SendMessenge(mainForm.playerCours.PlayerLogin);
            });
            Task.WaitAll();
            Close();
        }

        private void EnterLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
