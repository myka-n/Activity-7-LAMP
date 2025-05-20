namespace Activity_7
{
    partial class MyAccount
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
            flowLayoutPanel1 = new FlowLayoutPanel();
            logo = new PictureBox();
            linkLabel1 = new LinkLabel();
            linkLabel3 = new LinkLabel();
            linkLabel4 = new LinkLabel();
            flowLayoutPanel2 = new FlowLayoutPanel();
            profile = new LinkLabel();
            linkLabel7 = new LinkLabel();
            search = new TextBox();
            ProfileMenuStrip = new ContextMenuStrip(components);
            myAccountToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            logoutToolStripMenuItem = new ToolStripMenuItem();
            profilepic = new PictureBox();
            bio = new TextBox();
            btnEditProfile = new Button();
            labelusername = new Label();
            flowLayoutArtworks = new FlowLayoutPanel();
            lblArtworks = new Label();
            panel1 = new Panel();
            panel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)logo).BeginInit();
            flowLayoutPanel2.SuspendLayout();
            ProfileMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)profilepic).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Control;
            panel2.Controls.Add(flowLayoutPanel1);
            panel2.Location = new Point(13, 13);
            panel2.Name = "panel2";
            panel2.Size = new Size(314, 28);
            panel2.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.BackColor = Color.Transparent;
            flowLayoutPanel1.Controls.Add(logo);
            flowLayoutPanel1.Controls.Add(linkLabel1);
            flowLayoutPanel1.Controls.Add(linkLabel3);
            flowLayoutPanel1.Controls.Add(linkLabel4);
            flowLayoutPanel1.Location = new Point(12, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(282, 27);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // logo
            // 
            logo.Image = Properties.Resources._487992110_1648939996013765_4619232922560852750_n;
            logo.Location = new Point(3, 3);
            logo.Name = "logo";
            logo.Size = new Size(23, 19);
            logo.SizeMode = PictureBoxSizeMode.StretchImage;
            logo.TabIndex = 0;
            logo.TabStop = false;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Dock = DockStyle.Top;
            linkLabel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel1.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel1.LinkColor = Color.Black;
            linkLabel1.Location = new Point(32, 0);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(43, 17);
            linkLabel1.TabIndex = 1;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Home";
            // 
            // linkLabel3
            // 
            linkLabel3.AutoSize = true;
            linkLabel3.Dock = DockStyle.Top;
            linkLabel3.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel3.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel3.LinkColor = Color.Black;
            linkLabel3.Location = new Point(81, 0);
            linkLabel3.Name = "linkLabel3";
            linkLabel3.Size = new Size(71, 17);
            linkLabel3.TabIndex = 3;
            linkLabel3.TabStop = true;
            linkLabel3.Text = "Categories";
            // 
            // linkLabel4
            // 
            linkLabel4.AutoSize = true;
            linkLabel4.Dock = DockStyle.Top;
            linkLabel4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel4.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel4.LinkColor = Color.Black;
            linkLabel4.Location = new Point(158, 0);
            linkLabel4.Name = "linkLabel4";
            linkLabel4.Size = new Size(33, 17);
            linkLabel4.TabIndex = 4;
            linkLabel4.TabStop = true;
            linkLabel4.Text = "Post";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel2.BackColor = Color.Transparent;
            flowLayoutPanel2.Controls.Add(profile);
            flowLayoutPanel2.Controls.Add(linkLabel7);
            flowLayoutPanel2.Controls.Add(search);
            flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel2.Location = new Point(544, 16);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(217, 27);
            flowLayoutPanel2.TabIndex = 6;
            // 
            // profile
            // 
            profile.AutoSize = true;
            profile.Dock = DockStyle.Top;
            profile.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            profile.LinkBehavior = LinkBehavior.NeverUnderline;
            profile.LinkColor = Color.Black;
            profile.Location = new Point(169, 0);
            profile.Name = "profile";
            profile.Size = new Size(45, 17);
            profile.TabIndex = 5;
            profile.TabStop = true;
            profile.Text = "Profile";
            // 
            // linkLabel7
            // 
            linkLabel7.AutoSize = true;
            linkLabel7.Dock = DockStyle.Top;
            linkLabel7.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel7.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel7.LinkColor = Color.Black;
            linkLabel7.Location = new Point(116, 0);
            linkLabel7.Name = "linkLabel7";
            linkLabel7.Size = new Size(47, 17);
            linkLabel7.TabIndex = 8;
            linkLabel7.TabStop = true;
            linkLabel7.Text = "Search";
            // 
            // search
            // 
            search.Location = new Point(11, 3);
            search.Name = "search";
            search.Size = new Size(99, 23);
            search.TabIndex = 7;
            // 
            // ProfileMenuStrip
            // 
            ProfileMenuStrip.Items.AddRange(new ToolStripItem[] { myAccountToolStripMenuItem, settingsToolStripMenuItem, logoutToolStripMenuItem });
            ProfileMenuStrip.Name = "contextMenuStrip1";
            ProfileMenuStrip.Size = new Size(140, 70);
            // 
            // myAccountToolStripMenuItem
            // 
            myAccountToolStripMenuItem.Name = "myAccountToolStripMenuItem";
            myAccountToolStripMenuItem.Size = new Size(139, 22);
            myAccountToolStripMenuItem.Text = "My Account";
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
            logoutToolStripMenuItem.Size = new Size(139, 22);
            logoutToolStripMenuItem.Text = "Logout";
            // 
            // profilepic
            // 
            profilepic.Location = new Point(24, 90);
            profilepic.Name = "profilepic";
            profilepic.Size = new Size(113, 101);
            profilepic.SizeMode = PictureBoxSizeMode.StretchImage;
            profilepic.TabIndex = 8;
            profilepic.TabStop = false;
            // 
            // bio
            // 
            bio.Location = new Point(24, 229);
            bio.Name = "bio";
            bio.Size = new Size(216, 23);
            bio.TabIndex = 9;
            // 
            // btnEditProfile
            // 
            btnEditProfile.Location = new Point(24, 276);
            btnEditProfile.Name = "btnEditProfile";
            btnEditProfile.Size = new Size(75, 23);
            btnEditProfile.TabIndex = 10;
            btnEditProfile.Text = "Edit";
            btnEditProfile.UseVisualStyleBackColor = true;
            // 
            // labelusername
            // 
            labelusername.AutoSize = true;
            labelusername.Location = new Point(24, 211);
            labelusername.Name = "labelusername";
            labelusername.Size = new Size(60, 15);
            labelusername.TabIndex = 13;
            labelusername.Text = "Username";
            // 
            // flowLayoutArtworks
            // 
            flowLayoutArtworks.AutoScroll = true;
            flowLayoutArtworks.BackColor = Color.White;
            flowLayoutArtworks.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutArtworks.Location = new Point(24, 344);
            flowLayoutArtworks.Name = "flowLayoutArtworks";
            flowLayoutArtworks.Size = new Size(571, 400);
            flowLayoutArtworks.TabIndex = 2;
            // 
            // lblArtworks
            // 
            lblArtworks.AutoSize = true;
            lblArtworks.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblArtworks.Location = new Point(24, 314);
            lblArtworks.Name = "lblArtworks";
            lblArtworks.Size = new Size(106, 21);
            lblArtworks.TabIndex = 1;
            lblArtworks.Text = "My Artworks";
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(flowLayoutPanel2);
            panel1.Location = new Point(-1, -1);
            panel1.Name = "panel1";
            panel1.Size = new Size(786, 54);
            panel1.TabIndex = 14;
            // 
            // MyAccount
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(784, 700);
            Controls.Add(panel1);
            Controls.Add(lblArtworks);
            Controls.Add(flowLayoutArtworks);
            Controls.Add(labelusername);
            Controls.Add(btnEditProfile);
            Controls.Add(bio);
            Controls.Add(profilepic);
            Name = "MyAccount";
            Text = "MyAccount";
            panel2.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)logo).EndInit();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ProfileMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)profilepic).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel2;
        private FlowLayoutPanel flowLayoutPanel1;
        private PictureBox logo;
        private LinkLabel linkLabel1;
        private LinkLabel linkLabel3;
        private LinkLabel linkLabel4;
        private FlowLayoutPanel flowLayoutPanel2;
        private LinkLabel profile;
        private LinkLabel linkLabel7;
        private TextBox search;
        private ContextMenuStrip ProfileMenuStrip;
        private ToolStripMenuItem myAccountToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem logoutToolStripMenuItem;
        private PictureBox profilepic;
        private TextBox bio;
        private Button btnEditProfile;
        private Label labelusername;
        private FlowLayoutPanel flowLayoutArtworks;
        private Label lblArtworks;
        private Panel panel1;
    }
}