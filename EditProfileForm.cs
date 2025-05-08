using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace Activity_7
{
    public partial class EditProfileForm : Form
    {
        private TextBox txtUsername;
        private TextBox bio;
        private PictureBox profilepic;
        private Button btnSave;
        private Button btnCancel;
        private Button btnChangeProfilePic;
        private string profilePicsDirectory = Path.Combine(Application.StartupPath, "ProfilePics");

        public EditProfileForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit Profile";
            this.Size = new System.Drawing.Size(400, 400);

            Directory.CreateDirectory(profilePicsDirectory);
            InitializeControls();
            LoadUserData();
        }

        private void InitializeControls()
        {
            // Username TextBox
            txtUsername = new TextBox
            {
                Font = new System.Drawing.Font("Segoe UI", 12),
                Width = 250,
                Location = new System.Drawing.Point(20, 20),
                PlaceholderText = "Username"
            };
            this.Controls.Add(txtUsername);

            // Bio TextBox
            bio = new TextBox
            {
                Font = new System.Drawing.Font("Segoe UI", 12),
                Width = 250,
                Height = 100,
                Location = new System.Drawing.Point(20, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "Bio"
            };
            this.Controls.Add(bio);

            // Profile Picture PictureBox
            profilepic = new PictureBox
            {
                Width = 100,
                Height = 100,
                Location = new System.Drawing.Point(20, 170),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(profilepic);

            // Change Profile Picture Button
            btnChangeProfilePic = new Button
            {
                Text = "Change Picture",
                Width = 120,
                Location = new System.Drawing.Point(20, 280),
                BackColor = System.Drawing.Color.FromArgb(0, 122, 204),
                ForeColor = System.Drawing.Color.White
            };
            btnChangeProfilePic.Click += btnChangeProfilePic_Click;
            this.Controls.Add(btnChangeProfilePic);

            // Save Button
            btnSave = new Button
            {
                Text = "Save",
                Width = 100,
                Location = new System.Drawing.Point(20, 320),
                BackColor = System.Drawing.Color.FromArgb(0, 122, 204),
                ForeColor = System.Drawing.Color.White
            };
            btnSave.Click += btnSave_Click;
            this.Controls.Add(btnSave);

            // Cancel Button
            btnCancel = new Button
            {
                Text = "Cancel",
                Width = 100,
                Location = new System.Drawing.Point(130, 320),
                BackColor = System.Drawing.Color.Gray,
                ForeColor = System.Drawing.Color.White
            };
            btnCancel.Click += btnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        private void LoadUserData()
        {
            try
            {
                using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT username, bio, profile_pic FROM users WHERE user_id = @user_id", connection);
                    command.Parameters.AddWithValue("@user_id", Session.UserId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtUsername.Text = !reader.IsDBNull(reader.GetOrdinal("username")) ? 
                                reader.GetString("username") : Session.Username;
                            
                            bio.Text = !reader.IsDBNull(reader.GetOrdinal("bio")) ? 
                                reader.GetString("bio") : "";

                            if (!reader.IsDBNull(reader.GetOrdinal("profile_pic")))
                            {
                                string profilePath = reader.GetString("profile_pic");
                                if (File.Exists(profilePath))
                                {
                                    try
                                    {
                                        profilepic.Image = Image.FromFile(profilePath);
                                        Session.ProfilePicPath = profilePath;
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Error loading profile picture: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Username cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("UPDATE users SET bio = @Bio, username = @Username, profile_pic = @ProfilePic WHERE user_id = @UserId", connection);
                    command.Parameters.AddWithValue("@Bio", bio.Text);
                    command.Parameters.AddWithValue("@Username", txtUsername.Text);
                    command.Parameters.AddWithValue("@ProfilePic", Session.ProfilePicPath);
                    command.Parameters.AddWithValue("@UserId", Session.UserId);
                    command.ExecuteNonQuery();
                }

                Session.Bio = bio.Text;
                Session.Username = txtUsername.Text;
                MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnChangeProfilePic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                dlg.Title = "Select a Profile Picture";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string newFileName = $"profile_{Session.UserId}_{DateTime.Now.Ticks}{Path.GetExtension(dlg.FileName)}";
                        string destPath = Path.Combine(profilePicsDirectory, newFileName);

                        // Delete old profile picture if it exists
                        if (!string.IsNullOrEmpty(Session.ProfilePicPath) && File.Exists(Session.ProfilePicPath))
                        {
                            try
                            {
                                File.Delete(Session.ProfilePicPath);
                            }
                            catch { /* Ignore deletion errors */ }
                        }

                        File.Copy(dlg.FileName, destPath, true);
                        profilepic.Image = Image.FromFile(destPath);
                        Session.ProfilePicPath = destPath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating profile picture: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
