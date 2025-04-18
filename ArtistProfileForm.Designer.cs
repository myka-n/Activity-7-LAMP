namespace Activity_7
{
    partial class ArtistProfileForm
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
            lblArtistName = new Label();
            txtBio = new TextBox();
            pictureBoxProfile = new PictureBox();
            flowLayoutArtworks = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)pictureBoxProfile).BeginInit();
            SuspendLayout();
            // 
            // lblArtistName
            // 
            lblArtistName.AutoSize = true;
            lblArtistName.Location = new Point(263, 100);
            lblArtistName.Name = "lblArtistName";
            lblArtistName.Size = new Size(0, 15);
            lblArtistName.TabIndex = 0;
            // 
            // txtBio
            // 
            txtBio.Location = new Point(232, 135);
            txtBio.Multiline = true;
            txtBio.Name = "txtBio";
            txtBio.ReadOnly = true;
            txtBio.Size = new Size(100, 23);
            txtBio.TabIndex = 1;
            // 
            // pictureBoxProfile
            // 
            pictureBoxProfile.Location = new Point(232, 37);
            pictureBoxProfile.Name = "pictureBoxProfile";
            pictureBoxProfile.Size = new Size(100, 60);
            pictureBoxProfile.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxProfile.TabIndex = 2;
            pictureBoxProfile.TabStop = false;
            // 
            // flowLayoutArtworks
            // 
            flowLayoutArtworks.Location = new Point(45, 189);
            flowLayoutArtworks.Name = "flowLayoutArtworks";
            flowLayoutArtworks.Size = new Size(483, 390);
            flowLayoutArtworks.TabIndex = 3;
            // 
            // ArtistProfileForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(602, 749);
            Controls.Add(flowLayoutArtworks);
            Controls.Add(pictureBoxProfile);
            Controls.Add(txtBio);
            Controls.Add(lblArtistName);
            Name = "ArtistProfileForm";
            Text = "ArtistProfileForm";
            ((System.ComponentModel.ISupportInitialize)pictureBoxProfile).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblArtistName;
        private TextBox txtBio;
        private PictureBox pictureBoxProfile;
        private FlowLayoutPanel flowLayoutArtworks;
    }
}