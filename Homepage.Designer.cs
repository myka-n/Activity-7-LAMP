namespace Activity_7
{
    partial class Homepage
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
            components = new System.ComponentModel.Container();
            panel2 = new Panel();
            flowLayoutPanel2 = new FlowLayoutPanel();
            profile = new LinkLabel();
            ProfileMenuStrip = new ContextMenuStrip(components);
            myAccountToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            logoutToolStripMenuItem = new ToolStripMenuItem();
            linkLabel7 = new LinkLabel();
            textBox1 = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            pictureBox1 = new PictureBox();
            home = new LinkLabel();
            categories = new LinkLabel();
            post = new LinkLabel();
            search = new TextBox();
            welcomeMessage = new Label();
            label1 = new Label();
            flowLayoutPanel3 = new FlowLayoutPanel();
            flowLayoutPanel4 = new FlowLayoutPanel();
            mainContent = new Panel();
            label4 = new Label();
            label2 = new Label();
            panel3 = new Panel();
            label3 = new Label();
            flowLayoutPanel5 = new FlowLayoutPanel();
            panel2.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            ProfileMenuStrip.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            mainContent.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(240, 240, 240);
            panel2.Controls.Add(flowLayoutPanel2);
            panel2.Controls.Add(flowLayoutPanel1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(10);
            panel2.Size = new Size(784, 45);
            panel2.TabIndex = 2;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel2.BackColor = Color.Transparent;
            flowLayoutPanel2.Controls.Add(profile);
            flowLayoutPanel2.Controls.Add(linkLabel7);
            flowLayoutPanel2.Controls.Add(textBox1);
            flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel2.Location = new Point(516, 8);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(268, 27);
            flowLayoutPanel2.TabIndex = 7;
            // 
            // profile
            // 
            profile.ContextMenuStrip = ProfileMenuStrip;
            profile.Font = new Font("Segoe UI", 10F);
            profile.LinkBehavior = LinkBehavior.NeverUnderline;
            profile.LinkColor = Color.Black;
            profile.Location = new Point(183, 0);
            profile.Margin = new Padding(0, 0, 20, 0);
            profile.Name = "profile";
            profile.Size = new Size(65, 25);
            profile.TabIndex = 0;
            profile.TabStop = true;
            profile.Text = "Profile";
            profile.LinkClicked += profile_LinkClicked;
            // 
            // ProfileMenuStrip
            // 
            ProfileMenuStrip.Items.AddRange(new ToolStripItem[] { myAccountToolStripMenuItem, settingsToolStripMenuItem, logoutToolStripMenuItem });
            ProfileMenuStrip.Name = "ProfileMenuStrip";
            ProfileMenuStrip.Size = new Size(181, 92);
            // 
            // myAccountToolStripMenuItem
            // 
            myAccountToolStripMenuItem.Name = "myAccountToolStripMenuItem";
            myAccountToolStripMenuItem.Size = new Size(139, 22);
            myAccountToolStripMenuItem.Text = "My Account";
            myAccountToolStripMenuItem.Click += myAccountToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(139, 22);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // logoutToolStripMenuItem
            // 
            logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            logoutToolStripMenuItem.Size = new Size(180, 22);
            logoutToolStripMenuItem.Text = "Logout";
            logoutToolStripMenuItem.Click += logoutToolStripMenuItem_Click_1;
            // 
            // linkLabel7
            // 
            linkLabel7.AutoSize = true;
            linkLabel7.Dock = DockStyle.Top;
            linkLabel7.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel7.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel7.LinkColor = Color.Black;
            linkLabel7.Location = new Point(133, 0);
            linkLabel7.Name = "linkLabel7";
            linkLabel7.Size = new Size(47, 17);
            linkLabel7.TabIndex = 8;
            linkLabel7.TabStop = true;
            linkLabel7.Text = "Search";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(28, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(99, 23);
            textBox1.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(pictureBox1);
            flowLayoutPanel1.Controls.Add(home);
            flowLayoutPanel1.Controls.Add(categories);
            flowLayoutPanel1.Controls.Add(post);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(10, 10);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(390, 25);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources._487992110_1648939996013765_4619232922560852750_n;
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(23, 19);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // home
            // 
            home.Font = new Font("Segoe UI", 10F);
            home.LinkBehavior = LinkBehavior.NeverUnderline;
            home.LinkColor = Color.Black;
            home.Location = new Point(29, 0);
            home.Margin = new Padding(0, 0, 20, 0);
            home.Name = "home";
            home.Size = new Size(57, 23);
            home.TabIndex = 1;
            home.TabStop = true;
            home.Text = "Home";
            home.LinkClicked += home_LinkClicked;
            // 
            // categories
            // 
            categories.Font = new Font("Segoe UI", 10F);
            categories.LinkBehavior = LinkBehavior.NeverUnderline;
            categories.LinkColor = Color.Black;
            categories.Location = new Point(106, 0);
            categories.Margin = new Padding(0, 0, 20, 0);
            categories.Name = "categories";
            categories.Size = new Size(88, 23);
            categories.TabIndex = 3;
            categories.TabStop = true;
            categories.Text = "Categories";
            categories.LinkClicked += categories_LinkClicked;
            // 
            // post
            // 
            post.Font = new Font("Segoe UI", 10F);
            post.LinkBehavior = LinkBehavior.NeverUnderline;
            post.LinkColor = Color.Black;
            post.Location = new Point(214, 0);
            post.Margin = new Padding(0, 0, 20, 0);
            post.Name = "post";
            post.Size = new Size(43, 23);
            post.TabIndex = 4;
            post.TabStop = true;
            post.Text = "Post";
            post.LinkClicked += post_LinkClicked;
            // 
            // search
            // 
            search.Location = new Point(0, 0);
            search.Name = "search";
            search.Size = new Size(100, 23);
            search.TabIndex = 0;
            // 
            // welcomeMessage
            // 
            welcomeMessage.Location = new Point(0, 0);
            welcomeMessage.Name = "welcomeMessage";
            welcomeMessage.Size = new Size(100, 23);
            welcomeMessage.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label1.Location = new Point(20, 20);
            label1.Name = "label1";
            label1.Size = new Size(0, 30);
            label1.TabIndex = 0;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.Location = new Point(0, 0);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(200, 100);
            flowLayoutPanel3.TabIndex = 0;
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.AutoScroll = true;
            flowLayoutPanel4.BackColor = Color.White;
            flowLayoutPanel4.Location = new Point(20, 90);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(741, 365);
            flowLayoutPanel4.TabIndex = 2;
            // 
            // mainContent
            // 
            mainContent.Controls.Add(label4);
            mainContent.Controls.Add(label1);
            mainContent.Controls.Add(label2);
            mainContent.Controls.Add(flowLayoutPanel4);
            mainContent.Controls.Add(panel3);
            mainContent.Controls.Add(label3);
            mainContent.Controls.Add(flowLayoutPanel5);
            mainContent.Dock = DockStyle.Fill;
            mainContent.Location = new Point(0, 45);
            mainContent.Name = "mainContent";
            mainContent.Padding = new Padding(20);
            mainContent.Size = new Size(784, 655);
            mainContent.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(20, 458);
            label4.Name = "label4";
            label4.Size = new Size(167, 21);
            label4.TabIndex = 6;
            label4.Text = "Artist you may know";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label2.Location = new Point(20, 66);
            label2.Name = "label2";
            label2.Size = new Size(117, 21);
            label2.TabIndex = 1;
            label2.Text = "Your Artworks";
            // 
            // panel3
            // 
            panel3.BackColor = Color.LightGray;
            panel3.Location = new Point(20, 350);
            panel3.Name = "panel3";
            panel3.Size = new Size(200, 1);
            panel3.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label3.Location = new Point(20, 370);
            label3.Name = "label3";
            label3.Size = new Size(0, 21);
            label3.TabIndex = 4;
            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.AutoScroll = true;
            flowLayoutPanel5.BackColor = Color.White;
            flowLayoutPanel5.Location = new Point(20, 491);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Size = new Size(741, 200);
            flowLayoutPanel5.TabIndex = 5;
            flowLayoutPanel5.Paint += flowLayoutPanel5_Paint;
            // 
            // Homepage
            // 
            BackColor = Color.White;
            ClientSize = new Size(784, 700);
            Controls.Add(mainContent);
            Controls.Add(panel2);
            MinimumSize = new Size(800, 600);
            Name = "Homepage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Art Gallery - Homepage";
            panel2.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ProfileMenuStrip.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            mainContent.ResumeLayout(false);
            mainContent.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.LinkLabel home;
        private System.Windows.Forms.LinkLabel categories;
        private System.Windows.Forms.LinkLabel post;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.LinkLabel profile;
        private System.Windows.Forms.LinkLabel linkLabel7;
        private System.Windows.Forms.TextBox search;
        private System.Windows.Forms.ContextMenuStrip ProfileMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem myAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.Label welcomeMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.Panel panel3;
        private Panel mainContent;
        private Label label4;
        private TextBox textBox1;
        private PictureBox pictureBox1;
    }
}
