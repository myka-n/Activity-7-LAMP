using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Activity_7
{
    public partial class ArtworkPage : Form
    {
        public ArtworkPage()
        {
            InitializeComponent();
            this.Load += ArtworkPage_Load;       // Attach the Load event
            panel7.Resize += panel7_Resize;      // Handle resize for dynamic rounding
            panel7.Paint += panel7_Paint;
        }

        private void ArtworkPage_Load(object sender, EventArgs e)
        {
            MakePanelRounded(panel7, 20); // Initial rounding
        }

        private void panel7_Resize(object sender, EventArgs e)
        {
            MakePanelRounded(panel7, 20); // Reapply when resized
        }

        private void MakePanelRounded(Panel panel, int radius)
        {
            if (panel.Width > 0 && panel.Height > 0)
            {
                Rectangle bounds = new Rectangle(0, 0, panel.Width, panel.Height);
                int diameter = radius * 2;
                GraphicsPath path = new GraphicsPath();

                path.StartFigure();
                path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
                path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();

                panel.Region = new Region(path);
            }
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            int radius = 20;
            int borderThickness = 2;
            Color borderColor = Color.Gray;

            Rectangle bounds = new Rectangle(0, 0, panel7.Width, panel7.Height);
            GraphicsPath path = new GraphicsPath();

            int diameter = radius * 2;
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter - 1, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter - 1, bounds.Bottom - diameter - 1, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter - 1, diameter, diameter, 90, 90);
            path.CloseFigure();

            using (Pen pen = new Pen(borderColor, borderThickness))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void panel7_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Homepage homepageForm = new Homepage();
            homepageForm.ShowDialog();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            this.Hide();
            Homepage homepageForm = new Homepage();
            homepageForm.ShowDialog();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProfileMenuStrip.Show(linkLabel5, new Point(0, linkLabel5.Height));
        }

        private void myAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            MyAccount my_AccountForm = new MyAccount();
            my_AccountForm.ShowDialog();
        }

        private void likesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void commentsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.ShowDialog();
        }
    }
}
