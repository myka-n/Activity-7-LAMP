using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;

namespace Activity_7
{
    public partial class myAccount : Form
    {
        private bool isEditing = false;
        private string originalBio;
        private string originalUsername;
        private string originalProfilePicPath;

        public myAccount()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            SetupResponsiveDesign();
            LoadUserData();
        }

        private void SetupResponsiveDesign()
        {
            // Make the form resizable
            this.MinimumSize = new Size(800, 600);
            
            // Set up anchor points for controls
            profilepic.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            username.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            bio.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            
            // Set up the profile picture
            profilepic.SizeMode = PictureBoxSizeMode.Zoom;
            profilepic.BorderStyle = BorderStyle.FixedSingle;
            
            // Style the bio textbox
            bio.Multiline = true;
            bio.ScrollBars = ScrollBars.Vertical;
            bio.Font = new Font("Segoe UI", 10);
            
            // Style the username label
            username.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            
            // Add hover effects for buttons
            foreach (Control control in this.Controls)
            {
                if (control is Button)
                {
                    Button button = (Button)control;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.BackColor = Color.FromArgb(0, 122, 204);
                    button.ForeColor = Color.White;
                    button.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    button.Cursor = Cursors.Hand;
                    
                    // Add hover effect
                    button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(0, 102, 184);
                    button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(0, 122, 204);
                }
            }
        }

        private void LoadUserData()
        {
            username.Text = Session.Username;
            bio.Text = Session.Bio;
            originalBio = Session.Bio;
            originalUsername = Session.Username;
            originalProfilePicPath = Session.ProfilePicPath;

            if (!string.IsNullOrEmpty(Session.ProfilePicPath))
            {
                try
                {
                    profilepic.ImageLocation = Session.ProfilePicPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading profile picture: " + ex.Message);
                }
            }
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            isEditing = !isEditing;
            if (isEditing)
            {
                // Enable editing
                bio.ReadOnly = false;
                btnEditProfile.Text = "Save Changes";
                btnCancel.Visible = true;
            }
            else
            {
                // Save changes
                SaveChanges();
                btnEditProfile.Text = "Edit Profile";
                btnCancel.Visible = false;
            }
        }

        private void SaveChanges()
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"UPDATE users 
                                   SET bio = @Bio, 
                                       username = @Username,
                                       profile_pic = @ProfilePic
                                   WHERE user_id = @UserId";
                    
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Bio", bio.Text);
                    cmd.Parameters.AddWithValue("@Username", username.Text);
                    cmd.Parameters.AddWithValue("@ProfilePic", Session.ProfilePicPath);
                    cmd.Parameters.AddWithValue("@UserId", Session.UserId);
                    
                    cmd.ExecuteNonQuery();
                    
                    // Update session data
                    Session.Bio = bio.Text;
                    Session.Username = username.Text;
                    
                    MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating profile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChangeProfilePic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Select a Profile Picture";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create base uploads directory if it doesn't exist
                        string baseUploadsDir = @"C:\EDP\LAMP\uploads";
                        if (!Directory.Exists(baseUploadsDir))
                        {
                            Directory.CreateDirectory(baseUploadsDir);
                        }

                        // Create profile pictures directory
                        string profilePicsDir = Path.Combine(baseUploadsDir, "profilepics");
                        if (!Directory.Exists(profilePicsDir))
                        {
                            Directory.CreateDirectory(profilePicsDir);
                        }

                        // Create artwork images directory
                        string artworkDir = Path.Combine(baseUploadsDir, "artworkimages");
                        if (!Directory.Exists(artworkDir))
                        {
                            Directory.CreateDirectory(artworkDir);
                        }

                        // Generate a unique filename
                        string fileName = $"profile_{Session.UserId}_{DateTime.Now.Ticks}{Path.GetExtension(openFileDialog.FileName)}";
                        string destinationPath = Path.Combine(profilePicsDir, fileName);

                        // Copy the selected image to the profile pictures directory
                        File.Copy(openFileDialog.FileName, destinationPath, true);

                        // Update the profile picture
                        profilepic.ImageLocation = destinationPath;
                        Session.ProfilePicPath = destinationPath;

                        // Update the database
                        string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
                        using (MySqlConnection conn = new MySqlConnection(connStr))
                        {
                            conn.Open();
                            string query = "UPDATE users SET profile_pic = @ProfilePic WHERE user_id = @UserId";
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@ProfilePic", destinationPath);
                            cmd.Parameters.AddWithValue("@UserId", Session.UserId);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Profile picture updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating profile picture: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Restore original values
            bio.Text = originalBio;
            username.Text = originalUsername;
            if (!string.IsNullOrEmpty(originalProfilePicPath))
            {
                profilepic.ImageLocation = originalProfilePicPath;
            }
            
            isEditing = false;
            bio.ReadOnly = true;
            btnEditProfile.Text = "Edit Profile";
            btnCancel.Visible = false;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Clear session data
                Session.UserId = 0;
                Session.Username = string.Empty;
                Session.Bio = string.Empty;
                Session.ProfilePicPath = string.Empty;
                
                this.Hide();
                Login loginForm = new Login();
                loginForm.ShowDialog();
                this.Close();
            }
        }

        private void myAccount_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Profile link clicked - already on profile page, do nothing
        }

        private void myAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Already on my account page, do nothing
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnLogout_Click(sender, e);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            // Tab page clicked - refresh posts if needed
            LoadUserData();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            // Activity tab clicked - refresh activity if needed
            LoadUserData();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // My Account label clicked - do nothing
        }

        private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Adjust control sizes and positions based on form size
            if (profilepic != null)
            {
                int newSize = Math.Min(this.Width, this.Height) / 4;
                profilepic.Size = new Size(newSize, newSize);
            }
        }
    }
}
