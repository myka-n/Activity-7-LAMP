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
            panel1 = new Panel();
            linkLabel5 = new LinkLabel();
            linkLabel11 = new LinkLabel();
            profilepic = new PictureBox();
            username = new Label();
            bio = new TextBox();
            btnEditProfile = new Button();
            btnCancel = new Button();
            btnChangeProfilePic = new Button();
            btnLogout = new Button();
            openFileDialog1 = new OpenFileDialog();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)profilepic).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(linkLabel5);
            panel1.Controls.Add(linkLabel11);
            panel1.Controls.Add(profilepic);
            panel1.Controls.Add(username);
            panel1.Controls.Add(bio);
            panel1.Controls.Add(btnEditProfile);
            panel1.Controls.Add(btnCancel);
            panel1.Controls.Add(btnChangeProfilePic);
            panel1.Controls.Add(btnLogout);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10);
            panel1.Size = new Size(542, 749);
            panel1.TabIndex = 0;
            // 
            // linkLabel5
            // 
            linkLabel5.AutoSize = true;
            linkLabel5.Location = new Point(14, 10);
            linkLabel5.Name = "linkLabel5";
            linkLabel5.Size = new Size(41, 15);
            linkLabel5.TabIndex = 13;
            linkLabel5.TabStop = true;
            linkLabel5.Text = "Profile";
            linkLabel5.LinkClicked += linkLabel5_LinkClicked;
            // 
            // linkLabel11
            // 
            linkLabel11.AutoSize = true;
            linkLabel11.Location = new Point(84, 10);
            linkLabel11.Name = "linkLabel11";
            linkLabel11.Size = new Size(45, 15);
            linkLabel11.TabIndex = 14;
            linkLabel11.TabStop = true;
            linkLabel11.Text = "Logout";
            linkLabel11.LinkClicked += linkLabel11_LinkClicked;
            // 
            // profilepic
            // 
            profilepic.Location = new Point(14, 95);
            profilepic.Name = "profilepic";
            profilepic.Size = new Size(113, 113);
            profilepic.SizeMode = PictureBoxSizeMode.Zoom;
            profilepic.TabIndex = 9;
            profilepic.TabStop = false;
            // 
            // username
            // 
            username.AutoSize = true;
            username.Location = new Point(14, 211);
            username.Name = "username";
            username.Size = new Size(60, 15);
            username.TabIndex = 12;
            username.Text = "Username";
            // 
            // bio
            // 
            bio.BorderStyle = BorderStyle.FixedSingle;
            bio.Font = new Font("Segoe UI", 10F);
            bio.Location = new Point(14, 235);
            bio.Multiline = true;
            bio.Name = "bio";
            bio.ReadOnly = true;
            bio.ScrollBars = ScrollBars.Vertical;
            bio.Size = new Size(514, 50);
            bio.TabIndex = 15;
            // 
            // btnEditProfile
            // 
            btnEditProfile.BackColor = Color.FromArgb(0, 122, 204);
            btnEditProfile.Cursor = Cursors.Hand;
            btnEditProfile.FlatAppearance.BorderSize = 0;
            btnEditProfile.FlatStyle = FlatStyle.Flat;
            btnEditProfile.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEditProfile.ForeColor = Color.White;
            btnEditProfile.Location = new Point(16, 294);
            btnEditProfile.Name = "btnEditProfile";
            btnEditProfile.Size = new Size(120, 35);
            btnEditProfile.TabIndex = 16;
            btnEditProfile.Text = "Edit Profile";
            btnEditProfile.UseVisualStyleBackColor = false;
            btnEditProfile.Click += btnEditProfile_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(220, 53, 69);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(146, 294);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 35);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Visible = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnChangeProfilePic
            // 
            btnChangeProfilePic.BackColor = Color.FromArgb(0, 122, 204);
            btnChangeProfilePic.Cursor = Cursors.Hand;
            btnChangeProfilePic.FlatAppearance.BorderSize = 0;
            btnChangeProfilePic.FlatStyle = FlatStyle.Flat;
            btnChangeProfilePic.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnChangeProfilePic.ForeColor = Color.White;
            btnChangeProfilePic.Location = new Point(14, 95);
            btnChangeProfilePic.Name = "btnChangeProfilePic";
            btnChangeProfilePic.Size = new Size(113, 30);
            btnChangeProfilePic.TabIndex = 18;
            btnChangeProfilePic.Text = "Change Picture";
            btnChangeProfilePic.UseVisualStyleBackColor = false;
            btnChangeProfilePic.Click += btnChangeProfilePic_Click;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.FromArgb(108, 117, 125);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(437, 294);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(80, 35);
            btnLogout.TabIndex = 19;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.Title = "Select a Profile Picture";
            // 
            // MyAccount
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(542, 749);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(542, 749);
            Name = "MyAccount";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "My Account";
            Load += myAccount_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)profilepic).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private PictureBox profilepic;
        private Label username;
        private TextBox bio;
        private Button btnEditProfile;
        private Button btnCancel;
        private Button btnChangeProfilePic;
        private Button btnLogout;
        private OpenFileDialog openFileDialog1;
        private LinkLabel linkLabel5;
        private LinkLabel linkLabel11;
    }
}