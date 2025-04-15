namespace Activity_7
{
    partial class SignUp
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
            label3 = new Label();
            backToLogIn = new LinkLabel();
            label4 = new Label();
            signupbutton = new Button();
            password = new TextBox();
            label2 = new Label();
            full_name = new TextBox();
            label1 = new Label();
            label5 = new Label();
            username = new TextBox();
            label6 = new Label();
            email = new TextBox();
            button2 = new Button();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft JhengHei", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(135, 35);
            label3.Name = "label3";
            label3.Size = new Size(90, 16);
            label3.TabIndex = 9;
            label3.Text = "Sign Up Now!";
            label3.Click += label3_Click;
            // 
            // backToLogIn
            // 
            backToLogIn.AutoSize = true;
            backToLogIn.Location = new Point(143, 304);
            backToLogIn.Name = "backToLogIn";
            backToLogIn.Size = new Size(82, 15);
            backToLogIn.TabIndex = 8;
            backToLogIn.TabStop = true;
            backToLogIn.Text = "Back to Log In";
            backToLogIn.LinkClicked += linkLabel1_LinkClicked;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(105, 289);
            label4.Name = "label4";
            label4.Size = new Size(142, 15);
            label4.TabIndex = 7;
            label4.Text = "Already have an account?";
            // 
            // signupbutton
            // 
            signupbutton.BackColor = SystemColors.ActiveCaption;
            signupbutton.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            signupbutton.Location = new Point(136, 339);
            signupbutton.Name = "signupbutton";
            signupbutton.Size = new Size(82, 30);
            signupbutton.TabIndex = 6;
            signupbutton.Text = "Sign Up";
            signupbutton.UseVisualStyleBackColor = false;
            signupbutton.Click += button1_Click;
            // 
            // password
            // 
            password.Location = new Point(72, 236);
            password.Name = "password";
            password.Size = new Size(206, 23);
            password.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(72, 218);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 2;
            label2.Text = "Password";
            // 
            // full_name
            // 
            full_name.Location = new Point(72, 94);
            full_name.Name = "full_name";
            full_name.Size = new Size(206, 23);
            full_name.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(72, 76);
            label1.Name = "label1";
            label1.Size = new Size(61, 15);
            label1.TabIndex = 0;
            label1.Text = "Full Name";
            label1.Click += label1_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(72, 125);
            label5.Name = "label5";
            label5.Size = new Size(60, 15);
            label5.TabIndex = 10;
            label5.Text = "Username";
            // 
            // username
            // 
            username.Location = new Point(72, 143);
            username.Name = "username";
            username.Size = new Size(206, 23);
            username.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(72, 174);
            label6.Name = "label6";
            label6.Size = new Size(36, 15);
            label6.TabIndex = 12;
            label6.Text = "Email";
            // 
            // email
            // 
            email.Location = new Point(72, 192);
            email.Name = "email";
            email.Size = new Size(206, 23);
            email.TabIndex = 13;
            // 
            // button2
            // 
            button2.Cursor = Cursors.Hand;
            button2.Font = new Font("Segoe UI Emoji", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.Location = new Point(318, 12);
            button2.Name = "button2";
            button2.Size = new Size(24, 24);
            button2.TabIndex = 14;
            button2.Text = "x";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(354, 401);
            Controls.Add(button2);
            Controls.Add(label6);
            Controls.Add(email);
            Controls.Add(label5);
            Controls.Add(username);
            Controls.Add(label3);
            Controls.Add(backToLogIn);
            Controls.Add(label4);
            Controls.Add(label1);
            Controls.Add(signupbutton);
            Controls.Add(full_name);
            Controls.Add(password);
            Controls.Add(label2);
            Cursor = Cursors.Hand;
            ForeColor = SystemColors.ActiveCaptionText;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SignUp";
            Text = "SignUp";
            Load += SignUp_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label3;
        private LinkLabel backToLogIn;
        private Label label4;
        private Button signupbutton;
        private TextBox password;
        private Label label2;
        private TextBox full_name;
        private Label label1;
        private Label label5;
        private TextBox username;
        private Label label6;
        private TextBox email;
        private Button button2;
    }
}