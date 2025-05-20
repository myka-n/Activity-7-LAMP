using System;
using System.Drawing;
using System.Windows.Forms;

namespace Activity_7
{
    public partial class WelcomePopup : Form
    {
        public WelcomePopup(string username)
        {
            InitializeComponent();
            lblMessage.Text = "Welcome, " + username + "!";
        }

        private void InitializeComponent()
        {
            this.lblMessage = new Label();
            this.btnOK = new Button();

            // 
            // lblMessage
            // 
            this.lblMessage.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblMessage.Location = new Point(20, 20);
            this.lblMessage.Size = new Size(260, 30);
            this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // btnOK
            // 
            this.btnOK.Text = "Continue";
            this.btnOK.Location = new Point(100, 60);
            this.btnOK.Click += new EventHandler(this.BtnOK_Click);

            // 
            // WelcomePopup
            // 
            this.ClientSize = new Size(300, 120);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Hello!";
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Label lblMessage;
        private Button btnOK;
    }
}
