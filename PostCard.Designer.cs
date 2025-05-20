namespace Activity_7
{
    partial class PostCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            artworkimage = new PictureBox();
            lbltitle = new Label();
            lblauthor = new Label();
            lbllikes = new Label();
            lblcategories = new Label();
            lbltimestamp = new Label();
            ((System.ComponentModel.ISupportInitialize)artworkimage).BeginInit();
            SuspendLayout();
            // 
            // artworkimage
            // 
            artworkimage.Location = new Point(10, 10);
            artworkimage.Name = "artworkimage";
            artworkimage.Size = new Size(280, 200);
            artworkimage.SizeMode = PictureBoxSizeMode.StretchImage;
            artworkimage.TabIndex = 0;
            artworkimage.TabStop = false;
            artworkimage.BorderStyle = BorderStyle.FixedSingle;
            // 
            // lbltitle
            // 
            lbltitle.AutoSize = false;
            lbltitle.Location = new Point(10, 220);
            lbltitle.Name = "lbltitle";
            lbltitle.Size = new Size(280, 25);
            lbltitle.TabIndex = 1;
            lbltitle.Text = "artwork title";
            lbltitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lbltitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblauthor
            // 
            lblauthor.AutoSize = false;
            lblauthor.Location = new Point(10, 250);
            lblauthor.Name = "lblauthor";
            lblauthor.Size = new Size(280, 20);
            lblauthor.TabIndex = 2;
            lblauthor.Text = "username";
            lblauthor.Font = new Font("Segoe UI", 9F, FontStyle.Underline);
            lblauthor.ForeColor = Color.Blue;
            lblauthor.Cursor = Cursors.Hand;
            lblauthor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbllikes
            // 
            lbllikes.AutoSize = false;
            lbllikes.Location = new Point(10, 300);
            lbllikes.Name = "lbllikes";
            lbllikes.Size = new Size(280, 20);
            lbllikes.TabIndex = 3;
            lbllikes.Text = "likes";
            lbllikes.Font = new Font("Segoe UI", 9F);
            lbllikes.ForeColor = Color.DarkRed;
            lbllikes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblcategories
            // 
            lblcategories.AutoSize = false;
            lblcategories.Location = new Point(10, 275);
            lblcategories.Name = "lblcategories";
            lblcategories.Size = new Size(280, 20);
            lblcategories.TabIndex = 4;
            lblcategories.Text = "category";
            lblcategories.Font = new Font("Segoe UI", 9F);
            lblcategories.ForeColor = Color.DimGray;
            lblcategories.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbltimestamp
            // 
            lbltimestamp.AutoSize = false;
            lbltimestamp.Location = new Point(10, 325);
            lbltimestamp.Name = "lbltimestamp";
            lbltimestamp.Size = new Size(280, 20);
            lbltimestamp.TabIndex = 5;
            lbltimestamp.Text = "created at";
            lbltimestamp.Font = new Font("Segoe UI", 8F);
            lbltimestamp.ForeColor = Color.Gray;
            lbltimestamp.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // PostCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(lbltimestamp);
            Controls.Add(lblcategories);
            Controls.Add(lbllikes);
            Controls.Add(lblauthor);
            Controls.Add(lbltitle);
            Controls.Add(artworkimage);
            Margin = new Padding(10);
            Name = "PostCard";
            Size = new Size(300, 355);
            Load += PostCard_Load;
            ((System.ComponentModel.ISupportInitialize)artworkimage).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox artworkimage;
        private Label lbltitle;
        private Label lblauthor;
        private Label lbllikes;
        private Label lblcategories;
        private Label lbltimestamp;
    }
}
