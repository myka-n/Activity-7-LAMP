namespace Activity_7
{
    partial class Login
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
            label1 = new Label();
            textBox1 = new TextBox();
            label3 = new Label();
            linkLabel1 = new LinkLabel();
            label4 = new Label();
            button1 = new Button();
            textBox2 = new TextBox();
            label2 = new Label();
            label5 = new Label();
            forgotPasswordLink = new LinkLabel();
            button2 = new Button();
            togglePasswordButton = new Button();
            checkBox1 = new CheckBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(50, 85);
            label1.Name = "label1";
            label1.Size = new Size(106, 15);
            label1.TabIndex = 0;
            label1.Text = "Username or Email";
            label1.Click += label1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(50, 103);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(240, 23);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft JhengHei", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(106, 40);
            label3.Name = "label3";
            label3.Size = new Size(133, 16);
            label3.TabIndex = 9;
            label3.Text = "Log In to our website";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(145, 274);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(76, 15);
            linkLabel1.TabIndex = 8;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Sign Up Now";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(90, 259);
            label4.Name = "label4";
            label4.Size = new Size(153, 15);
            label4.TabIndex = 7;
            label4.Text = "Don't have an account yet? ";
            label4.Click += label4_Click;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ActiveCaption;
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.Cursor = Cursors.Hand;
            button1.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(132, 307);
            button1.Name = "button1";
            button1.Size = new Size(75, 30);
            button1.TabIndex = 6;
            button1.Text = "Log In";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(50, 156);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(240, 23);
            textBox2.TabIndex = 3;
            textBox2.UseSystemPasswordChar = true;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(50, 138);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 2;
            label2.Text = "Password";
            label2.Click += label2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(108, 274);
            label5.Name = "label5";
            label5.Size = new Size(40, 15);
            label5.TabIndex = 10;
            label5.Text = "Please";
            // 
            // forgotPasswordLink
            // 
            forgotPasswordLink.AutoSize = true;
            forgotPasswordLink.Location = new Point(50, 230);
            forgotPasswordLink.Name = "forgotPasswordLink";
            forgotPasswordLink.Size = new Size(100, 15);
            forgotPasswordLink.TabIndex = 11;
            forgotPasswordLink.TabStop = true;
            forgotPasswordLink.Text = "Forgot Password?";
            // 
            // button2
            // 
            button2.Cursor = Cursors.Hand;
            button2.Font = new Font("Segoe UI Emoji", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.Location = new Point(315, 12);
            button2.Name = "button2";
            button2.Size = new Size(24, 24);
            button2.TabIndex = 12;
            button2.Text = "x";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // togglePasswordButton
            // 
            togglePasswordButton.BackColor = SystemColors.Window;
            togglePasswordButton.FlatAppearance.BorderSize = 0;
            togglePasswordButton.FlatStyle = FlatStyle.Flat;
            togglePasswordButton.Font = new Font("Webdings", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 2);
            togglePasswordButton.Location = new Point(270, 156);
            togglePasswordButton.Name = "togglePasswordButton";
            togglePasswordButton.Size = new Size(20, 23);
            togglePasswordButton.TabIndex = 13;
            togglePasswordButton.Text = "v";
            togglePasswordButton.UseVisualStyleBackColor = false;
            togglePasswordButton.Click += togglePasswordButton_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(52, 185);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(104, 19);
            checkBox1.TabIndex = 14;
            checkBox1.Text = "Remember me";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(351, 365);
            Controls.Add(checkBox1);
            Controls.Add(togglePasswordButton);
            Controls.Add(button2);
            Controls.Add(forgotPasswordLink);
            Controls.Add(linkLabel1);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(textBox2);
            Controls.Add(label2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Login";
            Text = "Login";
            Load += Login_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Label label4;
        private Button button1;
        private TextBox textBox2;
        private Label label2;
        private LinkLabel linkLabel1;
        private Label label3;
        private Label label5;
        private LinkLabel forgotPasswordLink;
        private Button button2;
        private Button togglePasswordButton;
        private CheckBox checkBox1;
    }
}