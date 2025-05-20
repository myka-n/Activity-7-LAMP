namespace Activity_7
{
    partial class ArtistCard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            profilePic = new PictureBox();
            lblUsername = new Label();
            lblName = new Label();
            ((System.ComponentModel.ISupportInitialize)profilePic).BeginInit();
            SuspendLayout();
            // 
            // profilePic
            // 
            profilePic.Location = new Point(10, 10);
            profilePic.Name = "profilePic";
            profilePic.Size = new Size(60, 60);
            profilePic.SizeMode = PictureBoxSizeMode.StretchImage;
            profilePic.TabIndex = 0;
            profilePic.TabStop = false;
            profilePic.BorderStyle = BorderStyle.FixedSingle;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = false;
            lblUsername.Location = new Point(80, 15);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(200, 20);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "@username";
            lblUsername.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUsername.ForeColor = Color.Blue;
            lblUsername.Cursor = Cursors.Hand;
            // 
            // lblName
            // 
            lblName.AutoSize = false;
            lblName.Location = new Point(80, 40);
            lblName.Name = "lblName";
            lblName.Size = new Size(200, 20);
            lblName.TabIndex = 2;
            lblName.Text = "Full Name";
            lblName.Font = new Font("Segoe UI", 9F);
            lblName.ForeColor = Color.DimGray;
            lblName.Cursor = Cursors.Hand;
            // 
            // ArtistCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(lblName);
            Controls.Add(lblUsername);
            Controls.Add(profilePic);
            Margin = new Padding(5);
            Name = "ArtistCard";
            Size = new Size(290, 80);
            Cursor = Cursors.Hand;
            ((System.ComponentModel.ISupportInitialize)profilePic).EndInit();
            ResumeLayout(false);
        }

        private PictureBox profilePic;
        private Label lblUsername;
        private Label lblName;
    }
} 