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
            panel2 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            pictureBox1 = new PictureBox();
            linkLabel1 = new LinkLabel();
            linkLabel2 = new LinkLabel();
            linkLabel3 = new LinkLabel();
            linkLabel4 = new LinkLabel();
            flowLayoutPanel2 = new FlowLayoutPanel();
            linkLabel5 = new LinkLabel();
            searchlink = new LinkLabel();
            searchtextbox = new TextBox();
            bindingSource1 = new BindingSource(components);
            flowlayoutcategories = new FlowLayoutPanel();
            mySqlCommand1 = new MySql.Data.MySqlClient.MySqlCommand();
            label1 = new Label();
            flowlayoutartworks = new FlowLayoutPanel();
            panel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ControlLight;
            panel2.Controls.Add(flowLayoutPanel1);
            panel2.Controls.Add(flowLayoutPanel2);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(601, 28);
            panel2.TabIndex = 8;
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
            flowLayoutPanel1.Location = new Point(12, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(282, 27);
            flowLayoutPanel1.TabIndex = 5;
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
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel2.BackColor = Color.Transparent;
            flowLayoutPanel2.Controls.Add(linkLabel5);
            flowLayoutPanel2.Controls.Add(searchlink);
            flowLayoutPanel2.Controls.Add(searchtextbox);
            flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel2.Location = new Point(372, 0);
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
            flowlayoutcategories.Size = new Size(571, 193);
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
            flowlayoutartworks.Size = new Size(571, 459);
            flowlayoutartworks.TabIndex = 11;
            // 
            // Categories
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(599, 749);
            Controls.Add(flowlayoutartworks);
            Controls.Add(label1);
            Controls.Add(flowlayoutcategories);
            Controls.Add(panel2);
            Name = "Categories";
            Text = "Categories";
            panel2.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel2;
        private FlowLayoutPanel flowLayoutPanel1;
        private PictureBox pictureBox1;
        private LinkLabel linkLabel1;
        private LinkLabel linkLabel2;
        private LinkLabel linkLabel3;
        private LinkLabel linkLabel4;
        private FlowLayoutPanel flowLayoutPanel2;
        private LinkLabel linkLabel5;
        private LinkLabel searchlink;
        private TextBox searchtextbox;
        private BindingSource bindingSource1;
        private FlowLayoutPanel flowlayoutcategories;
        private MySql.Data.MySqlClient.MySqlCommand mySqlCommand1;
        private Label label1;
        private FlowLayoutPanel flowlayoutartworks;
    }
}