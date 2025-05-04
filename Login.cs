using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using MySql.Data.MySqlClient;


namespace Activity_7
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            SetRoundedRegion(20);
        }

        private void SetRoundedRegion(int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(this.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(this.Width - radius, this.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, this.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();

            this.Region = new Region(path);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetRoundedRegion(30); // keeps it rounded on resize
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            SignUp signUpForm = new SignUp();
            signUpForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Database connection
            string connStr = "server=localhost;user=root;password=mykz;database=zeereal_artspace";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT user_id, username, email, password, profile_pic, bio FROM users WHERE email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", textBox1.Text); // Email textbox
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string dbPassword = reader["password"].ToString();

                        // Assuming you are storing passwords securely (hash them in real-world scenarios)
                        if (textBox2.Text == dbPassword) // Password textbox
                        {
                            // ⭐ SET THE SESSION VALUES
                            Session.UserId = Convert.ToInt32(reader["user_id"]);
                            Session.Username = reader["username"].ToString();
                            Session.Email = reader["email"].ToString();
                            Session.ProfilePicPath = reader["profile_pic"] != DBNull.Value ? reader["profile_pic"].ToString() : "";
                            Session.Bio = reader["bio"].ToString();

                            // Successful login
                            MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide(); // Hide login form
                            Homepage homepageForm = new Homepage();
                            homepageForm.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.BackColor = Color.Transparent;
            button2.ForeColor = Color.Black; // or White
            button2.Font = new Font("Arial", 10, FontStyle.Bold);


        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
