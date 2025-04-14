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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Homepage));
            button1 = new Button();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            statusStrip1 = new StatusStrip();
            imageList1 = new ImageList(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(338, 117);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(34, 68);
            panel1.Name = "panel1";
            panel1.Size = new Size(468, 187);
            panel1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.HIRAYA__ready_to_print_;
            pictureBox1.ErrorImage = (Image)resources.GetObject("pictureBox1.ErrorImage");
            pictureBox1.Image = Properties.Resources.HIRAYA__ready_to_print_;
            pictureBox1.Location = new Point(43, 33);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(198, 107);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(0, 523);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(537, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "copy of jah.png");
            imageList1.Images.SetKeyName(1, "copyofsurreal.png");
            imageList1.Images.SetKeyName(2, "eyy_20240211103712.png");
            imageList1.Images.SetKeyName(3, "HIRAYA (ready to print).png");
            imageList1.Images.SetKeyName(4, "copy of jah.png");
            // 
            // Homepage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(537, 545);
            Controls.Add(statusStrip1);
            Controls.Add(panel1);
            Name = "Homepage";
            Text = "Homepage";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Panel panel1;
        private PictureBox pictureBox1;
        private StatusStrip statusStrip1;
        private ImageList imageList1;
    }
}