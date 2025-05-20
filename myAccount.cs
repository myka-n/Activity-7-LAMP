using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Activity_7
{
    public partial class MyAccount : Form
    {
        private string profilePicsDirectory = Path.Combine(Application.StartupPath, "ProfilePics");

        public MyAccount()
        {
            InitializeComponent();
            Directory.CreateDirectory(profilePicsDirectory);
            InitializeProfileData();
            LoadUserArtworks();
            btnEditProfile.Click += BtnEdit_Click;

            // Set up navigation
            linkLabel1.LinkClicked += Home_LinkClicked;
            linkLabel3.LinkClicked += Categories_LinkClicked;
            linkLabel4.LinkClicked += Post_LinkClicked;
            profile.LinkClicked += Profile_LinkClicked;
            linkLabel7.LinkClicked += Search_LinkClicked;

            // Set up menu items
            myAccountToolStripMenuItem.Click += myAccountToolStripMenuItem_Click;
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            logoutToolStripMenuItem.Click += logoutToolStripMenuItem_Click;
        }

        private void InitializeProfileData()
        {
            try
            {
                using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT profile_pic, bio, username FROM users WHERE user_id = @user_id", connection);
                    command.Parameters.AddWithValue("@user_id", Session.UserId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Load profile picture
                            if (!reader.IsDBNull(reader.GetOrdinal("profile_pic")))
                            {
                                string profilePath = reader.GetString("profile_pic");
                                if (File.Exists(profilePath))
                                {
                                    try
                                    {
                                        using (var image = Image.FromFile(profilePath))
                                        {
                                            profilepic.Image = new Bitmap(image);
                                        }
                                        Session.ProfilePicPath = profilePath;
                                    }
                                    catch
                                    {
                                        // Silently handle image loading errors
                                        profilepic.Image = null;
                                    }
                                }
                            }

                            // Load bio
                            bio.Text = reader.IsDBNull(reader.GetOrdinal("bio")) ? "" : reader.GetString("bio");

                            // Update username if changed
                            if (!reader.IsDBNull(reader.GetOrdinal("username")))
                            {
                                string dbUsername = reader.GetString("username");
                                if (dbUsername != Session.Username)
                                {
                                    Session.Username = dbUsername;
                                }
                            }
                        }
                    }
                }

                profile.Text = "Profile";
                labelusername.Text = Session.Username ?? "User";
                bio.ReadOnly = true;
                btnEditProfile.Visible = true;
            }
            catch (Exception ex)
            {
                // Only show error for unexpected database errors
                if (!ex.Message.Contains("null"))
                {
                    MessageBox.Show($"Error loading profile data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Home_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var homepage = new Homepage();
            homepage.FormClosed += (s, args) => this.Close();
            homepage.Show();
        }

        private void Explore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var explore = new explore();
            explore.FormClosed += (s, args) => this.Close();
            explore.Show();
        }

        private void Categories_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var categories = new Categories();
            categories.FormClosed += (s, args) => this.Close();
            categories.Show();
        }

        private void Post_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var uploadForm = new Upload();
            uploadForm.Owner = this;
            this.Hide();
            uploadForm.ShowDialog();
            this.Show();
        }

        private void Profile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProfileMenuStrip.Show(profile, new Point(0, profile.Height));
        }

        private void Search_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Implement search functionality if needed
        }

        private void logo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Select a Profile Picture";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string newFileName = $"profile_{Session.UserId}_{DateTime.Now.Ticks}{Path.GetExtension(openFileDialog.FileName)}";
                        string newPath = Path.Combine(profilePicsDirectory, newFileName);

                        // Delete old profile picture if it exists
                        if (!string.IsNullOrEmpty(Session.ProfilePicPath) && File.Exists(Session.ProfilePicPath))
                        {
                            try
                            {
                                File.Delete(Session.ProfilePicPath);
                            }
                            catch { /* Ignore deletion errors */ }
                        }

                        File.Copy(openFileDialog.FileName, newPath, true);

                        using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                        {
                            connection.Open();
                            var command = new MySqlCommand("UPDATE users SET profile_pic = @path WHERE user_id = @user_id", connection);
                            command.Parameters.AddWithValue("@path", newPath);
                            command.Parameters.AddWithValue("@user_id", Session.UserId);
                            command.ExecuteNonQuery();
                        }

                        Session.ProfilePicPath = newPath;
                        using (var image = Image.FromFile(newPath))
                        {
                            profilepic.Image = new Bitmap(image);
                        }
                        MessageBox.Show("Profile picture updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating profile picture: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            using (var editForm = new EditProfileForm())
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh the profile data after successful edit
                    InitializeProfileData();
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string newBio = bio.Text.Trim();

                using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("UPDATE users SET bio = @bio WHERE user_id = @user_id", connection);
                    command.Parameters.AddWithValue("@bio", newBio);
                    command.Parameters.AddWithValue("@user_id", Session.UserId);
                    command.ExecuteNonQuery();
                }

                Session.Bio = newBio;
                MessageBox.Show("Bio updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                bio.ReadOnly = true;
                btnEditProfile.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving bio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void myAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Already on MyAccount
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Optional settings logic
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Session.Clear();
            this.Hide();
            new Login().ShowDialog();
        }

        private void LoadUserArtworks()
        {
            try
            {
                if (Session.UserId <= 0)
                {
                    MessageBox.Show("User session is invalid. Please log in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand(@"
                        SELECT a.art_id, a.art_title, a.art_description, a.image_path, a.created_at 
                        FROM artworks a 
                        WHERE a.user_id = @user_id 
                        ORDER BY a.created_at DESC", connection);
                    command.Parameters.AddWithValue("@user_id", Session.UserId);

                    using (var reader = command.ExecuteReader())
                    {
                        flowLayoutArtworks.Controls.Clear();

                        while (reader.Read())
                        {
                            try
                            {
                                int artId = reader.GetInt32("art_id");
                                string title = reader.IsDBNull(reader.GetOrdinal("art_title")) ? "Untitled" : reader.GetString("art_title");
                                string description = reader.IsDBNull(reader.GetOrdinal("art_description")) ? "" : reader.GetString("art_description");
                                string imagePath = reader.IsDBNull(reader.GetOrdinal("image_path")) ? "" : reader.GetString("image_path");
                                DateTime createdAt = reader.IsDBNull(reader.GetOrdinal("created_at")) ? DateTime.Now : reader.GetDateTime("created_at");

                                Panel panel = new Panel
                                {
                                    Width = 250,
                                    Height = 300,
                                    Margin = new Padding(10),
                                    BorderStyle = BorderStyle.FixedSingle,
                                    BackColor = Color.White
                                };

                                PictureBox pictureBox = new PictureBox
                                {
                                    Width = 230,
                                    Height = 200,
                                    Location = new Point(10, 10),
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    BorderStyle = BorderStyle.FixedSingle
                                };

                                try
                                {
                                    if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                                    {
                                        using (var image = Image.FromFile(imagePath))
                                        {
                                            pictureBox.Image = new Bitmap(image);
                                        }
                                    }
                                }
                                catch
                                {
                                    // Silently handle image loading errors
                                    pictureBox.Image = null;
                                }

                                Label titleLabel = new Label
                                {
                                    Text = title,
                                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                                    Location = new Point(10, 220),
                                    Width = 230,
                                    Height = 20
                                };

                                Label dateLabel = new Label
                                {
                                    Text = createdAt.ToString("MMMM dd, yyyy"),
                                    Font = new Font("Segoe UI", 8),
                                    Location = new Point(10, 240),
                                    Width = 230,
                                    Height = 20
                                };

                                Button editButton = new Button
                                {
                                    Text = "Edit",
                                    Location = new Point(10, 265),
                                    Width = 110,
                                    Height = 25
                                };
                                editButton.Click += (s, e) => EditArtwork(artId);

                                Button deleteButton = new Button
                                {
                                    Text = "Delete",
                                    Location = new Point(130, 265),
                                    Width = 110,
                                    Height = 25
                                };
                                deleteButton.Click += (s, e) => DeleteArtwork(artId);

                                panel.Controls.Add(pictureBox);
                                panel.Controls.Add(titleLabel);
                                panel.Controls.Add(dateLabel);
                                panel.Controls.Add(editButton);
                                panel.Controls.Add(deleteButton);

                                flowLayoutArtworks.Controls.Add(panel);
                            }
                            catch (Exception ex)
                            {
                                // Log the error but continue loading other artworks
                                Console.WriteLine($"Error loading artwork: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading artworks: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditArtwork(int artId)
        {
            using (var editForm = new EditArtworkForm(artId))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUserArtworks(); // Refresh the artworks after edit
                }
            }
        }

        private void DeleteArtwork(int artId)
        {
            var result = MessageBox.Show("Are you sure you want to delete this artwork?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString))
                    {
                        connection.Open();
                        var command = new MySqlCommand("DELETE FROM artworks WHERE art_id = @art_id AND user_id = @user_id", connection);
                        command.Parameters.AddWithValue("@art_id", artId);
                        command.Parameters.AddWithValue("@user_id", Session.UserId);
                        command.ExecuteNonQuery();
                    }
                    LoadUserArtworks(); // Refresh the artworks after deletion
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting artwork: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
