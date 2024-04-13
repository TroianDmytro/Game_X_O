namespace Player.View
{
    partial class EnterLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            Btn_Enter = new Button();
            Tb_login = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 9);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(64, 28);
            label1.TabIndex = 0;
            label1.Text = "Login";
            label1.Click += label1_Click;
            // 
            // Btn_Enter
            // 
            Btn_Enter.Location = new Point(195, 48);
            Btn_Enter.Margin = new Padding(4);
            Btn_Enter.Name = "Btn_Enter";
            Btn_Enter.Size = new Size(114, 41);
            Btn_Enter.TabIndex = 2;
            Btn_Enter.Text = "Enter";
            Btn_Enter.UseVisualStyleBackColor = true;
            Btn_Enter.Click += Btn_Enter_Click;
            // 
            // Tb_login
            // 
            Tb_login.Location = new Point(84, 9);
            Tb_login.MaxLength = 16;
            Tb_login.Name = "Tb_login";
            Tb_login.Size = new Size(225, 34);
            Tb_login.TabIndex = 3;
            Tb_login.TextChanged += Tb_login_TextChanged;
            // 
            // EnterLogin
            // 
            AutoScaleDimensions = new SizeF(12F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(320, 102);
            Controls.Add(Tb_login);
            Controls.Add(Btn_Enter);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EnterLogin";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EnterLogin";
            Load += EnterLogin_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button Btn_Enter;
        public TextBox Tb_login;
    }
}