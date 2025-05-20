namespace Activity_7
{
    partial class Categories
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
            flowLayoutPanel2 = new FlowLayoutPanel();
            linkLabel5 = new LinkLabel();
            searchlink = new LinkLabel();
            searchtextbox = new TextBox();
            bindingSource1 = new BindingSource(components);
            flowlayoutcategories = new FlowLayoutPanel();
            mySqlCommand1 = new MySql.Data.MySqlClient.MySqlCommand();
            label1 = new Label();
            flowlayoutartworks = new FlowLayoutPanel();
            ProfileMenuStrip = new ContextMenuStrip(components);
            myAccountToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            logoutToolStripMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            linkLabel4 = new LinkLabel();
            linkLabel3 = new LinkLabel();
            linkLabel2 = new LinkLabel();
            linkLabel1 = new LinkLabel();
            pictureBox1 = new PictureBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            ProfileMenuStrip.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel2.BackColor = Color.Transparent;
            flowLayoutPanel2.Controls.Add(linkLabel5);
            flowLayoutPanel2.Controls.Add(searchlink);
            flowLayoutPanel2.Controls.Add(searchtextbox);
            flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel2.Location = new Point(559, 10);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(217, 27);
            flowLayoutPanel2.TabIndex = 6;
            // 
            // linkLabel5
            // 
            linkLabel5.AutoSize = true;
            linkLabel5.Dock = DockStyle.Top;
            linkLabel5.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel5.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel5.LinkColor = Color.Black;
            linkLabel5.Location = new Point(169, 0);
            linkLabel5.Name = "linkLabel5";
            linkLabel5.Size = new Size(45, 17);
            linkLabel5.TabIndex = 5;
            linkLabel5.TabStop = true;
            linkLabel5.Text = "Profile";
            linkLabel5.LinkClicked += linkLabel5_LinkClicked;
            // 
            // searchlink
            // 
            searchlink.AutoSize = true;
            searchlink.Dock = DockStyle.Top;
            searchlink.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            searchlink.LinkBehavior = LinkBehavior.NeverUnderline;
            searchlink.LinkColor = Color.Black;
            searchlink.Location = new Point(116, 0);
            searchlink.Name = "searchlink";
            searchlink.Size = new Size(47, 17);
            searchlink.TabIndex = 8;
            searchlink.TabStop = true;
            searchlink.Text = "Search";
            searchlink.LinkClicked += SearchLinkLabel_LinkClicked;
            // 
            // searchtextbox
            // 
            searchtextbox.Location = new Point(11, 3);
            searchtextbox.Name = "searchtextbox";
            searchtextbox.Size = new Size(99, 23);
            searchtextbox.TabIndex = 7;
            // 
            // flowlayoutcategories
            // 
            flowlayoutcategories.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowlayoutcategories.Cursor = Cursors.Hand;
            flowlayoutcategories.Location = new Point(12, 79);
            flowlayoutcategories.MinimumSize = new Size(200, 100);
            flowlayoutcategories.Name = "flowlayoutcategories";
            flowlayoutcategories.Size = new Size(760, 193);
            flowlayoutcategories.TabIndex = 9;
            // 
            // mySqlCommand1
            // 
            mySqlCommand1.CacheAge = 0;
            mySqlCommand1.Connection = null;
            mySqlCommand1.EnableCaching = false;
            mySqlCommand1.Transaction = null;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 51);
            label1.Name = "label1";
            label1.Size = new Size(119, 15);
            label1.TabIndex = 10;
            label1.Text = "Browse artwork posts";
            // 
            // flowlayoutartworks
            // 
            flowlayoutartworks.AutoScroll = true;
            flowlayoutartworks.Location = new Point(12, 278);
            flowlayoutartworks.Name = "flowlayoutartworks";
            flowlayoutartworks.Size = new Size(760, 459);
            flowlayoutartworks.TabIndex = 11;
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
            myAccountToolStripMenuItem.Click += myAccountToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(139, 22);
            settingsToolStripMenuItem.Text = "Settings";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // logoutToolStripMenuItem
            // 
            logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            logoutToolStripMenuItem.Size = new Size(139, 22);
            logoutToolStripMenuItem.Text = "Logout";
            logoutToolStripMenuItem.Click += logoutToolStripMenuItem_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(flowLayoutPanel2);
            panel1.Location = new Point(-4, -1);
            panel1.Name = "panel1";
            panel1.Size = new Size(795, 49);
            panel1.TabIndex = 12;
            // 
            // linkLabel4
            // 
            linkLabel4.AutoSize = true;
            linkLabel4.Dock = DockStyle.Top;
            linkLabel4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel4.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel4.LinkColor = Color.Black;
            linkLabel4.Location = new Point(216, 0);
            linkLabel4.Name = "linkLabel4";
            linkLabel4.Size = new Size(33, 17);
            linkLabel4.TabIndex = 4;
            linkLabel4.TabStop = true;
            linkLabel4.Text = "Post";
            linkLabel4.LinkClicked += linkLabel4_LinkClicked;
            // 
            // linkLabel3
            // 
            linkLabel3.AutoSize = true;
            linkLabel3.Dock = DockStyle.Top;
            linkLabel3.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel3.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel3.LinkColor = Color.Black;
            linkLabel3.Location = new Point(139, 0);
            linkLabel3.Name = "linkLabel3";
            linkLabel3.Size = new Size(71, 17);
            linkLabel3.TabIndex = 3;
            linkLabel3.TabStop = true;
            linkLabel3.Text = "Categories";
            linkLabel3.LinkClicked += linkLabel3_LinkClicked;
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.Dock = DockStyle.Top;
            linkLabel2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel2.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel2.LinkColor = Color.Black;
            linkLabel2.Location = new Point(81, 0);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new Size(52, 17);
            linkLabel2.TabIndex = 2;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "Explore";
            // 
            // linkLabel1
            // 
            linkLabel1.Dock = DockStyle.Top;
            linkLabel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel1.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel1.LinkColor = Color.Black;
            linkLabel1.Location = new Point(32, 0);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(43, 22);
            linkLabel1.TabIndex = 1;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Home";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources._487992110_1648939996013765_4619232922560852750_n;
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(23, 19);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.BackColor = Color.Transparent;
            flowLayoutPanel1.Controls.Add(pictureBox1);
            flowLayoutPanel1.Controls.Add(linkLabel1);
            flowLayoutPanel1.Controls.Add(linkLabel2);
            flowLayoutPanel1.Controls.Add(linkLabel3);
            flowLayoutPanel1.Controls.Add(linkLabel4);
            flowLayoutPanel1.Location = new Point(16, 10);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(502, 27);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // Categories
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 700);
            Controls.Add(panel1);
            Controls.Add(flowlayoutartworks);
            Controls.Add(label1);
            Controls.Add(flowlayoutcategories);
            Name = "Categories";
            Text = "Categories";
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ProfileMenuStrip.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private FlowLayoutPanel flowLayoutPanel2;
        private LinkLabel linkLabel5;
        private LinkLabel searchlink;
        private TextBox searchtextbox;
        private BindingSource bindingSource1;
        private FlowLayoutPanel flowlayoutcategories;
        private MySql.Data.MySqlClient.MySqlCommand mySqlCommand1;
        private Label label1;
        private FlowLayoutPanel flowlayoutartworks;
        private ContextMenuStrip ProfileMenuStrip;
        private ToolStripMenuItem myAccountToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem logoutToolStripMenuItem;
        private Panel panel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private PictureBox pictureBox1;
        private LinkLabel linkLabel1;
        private LinkLabel linkLabel2;
        private LinkLabel linkLabel3;
        private LinkLabel linkLabel4;
    }
}