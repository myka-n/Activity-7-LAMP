namespace Activity_7
{
    partial class ResetPasswordForm
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
            newPasswordTextBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            confirmPasswordTextBox = new TextBox();
            resetButton = new Button();
            SuspendLayout();
            // 
            // newPasswordTextBox
            // 
            newPasswordTextBox.Location = new Point(39, 86);
            newPasswordTextBox.Name = "newPasswordTextBox";
            newPasswordTextBox.Size = new Size(275, 23);
            newPasswordTextBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 68);
            label1.Name = "label1";
            label1.Size = new Size(115, 15);
            label1.TabIndex = 1;
            label1.Text = "Enter new password:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(39, 121);
            label2.Name = "label2";
            label2.Size = new Size(132, 15);
            label2.TabIndex = 2;
            label2.Text = "Confirm new password:";
            // 
            // confirmPasswordTextBox
            // 
            confirmPasswordTextBox.Location = new Point(39, 139);
            confirmPasswordTextBox.Name = "confirmPasswordTextBox";
            confirmPasswordTextBox.Size = new Size(275, 23);
            confirmPasswordTextBox.TabIndex = 3;
            confirmPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // resetButton
            // 
            resetButton.Location = new Point(125, 204);
            resetButton.Name = "resetButton";
            resetButton.Size = new Size(90, 31);
            resetButton.TabIndex = 4;
            resetButton.Text = "Reset";
            resetButton.UseVisualStyleBackColor = true;
            // 
            // ResetPasswordForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(351, 365);
            Controls.Add(resetButton);
            Controls.Add(confirmPasswordTextBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(newPasswordTextBox);
            Name = "ResetPasswordForm";
            Text = "ResetPasswordForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox newPasswordTextBox;
        private Label label1;
        private Label label2;
        private TextBox confirmPasswordTextBox;
        private Button resetButton;
    }
}