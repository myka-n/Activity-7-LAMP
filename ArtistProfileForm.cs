using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Activity_7
{
    public partial class ArtistProfileForm : Form
    {
        private string artistUsername;
        private Panel profilePanel;
        private PictureBox picProfile;
        private Label lblArtistName;
        private Label lblBio;
        private FlowLayoutPanel flowLayoutArtworks;

        public ArtistProfileForm(string username)
        {
            artistUsername = username;
            InitializeComponent();
            LoadArtistProfile();
            LoadArtworks();
        }

        private void InitializeComponent()
        {
            profilePanel = new Panel();
            picProfile = new PictureBox();
            flowLayoutArtworks = new FlowLayoutPanel();
            lblArtistName = new Label();
            lblBio = new Label();
            this.label1 = new Label();
            profilePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picProfile).BeginInit();
            SuspendLayout();
            // 
            // profilePanel
            // 
            profilePanel.Controls.Add(this.label1);
            profilePanel.Controls.Add(picProfile);
            profilePanel.Controls.Add(flowLayoutArtworks);
            profilePanel.Controls.Add(lblArtistName);
            profilePanel.Controls.Add(lblBio);
            profilePanel.Location = new Point(-1, 0);
            profilePanel.Name = "profilePanel";
            profilePanel.Size = new Size(634, 615);
            profilePanel.TabIndex = 0;
            // 
            // picProfile
            // 
            picProfile.Location = new Point(34, 50);
            picProfile.Name = "picProfile";
            picProfile.Size = new Size(108, 97);
            picProfile.SizeMode = PictureBoxSizeMode.StretchImage;
            picProfile.TabIndex = 0;
            picProfile.TabStop = false;
            // 
            // flowLayoutArtworks
            // 
            flowLayoutArtworks.AutoScroll = true;
            flowLayoutArtworks.Location = new Point(34, 203);
            flowLayoutArtworks.Name = "flowLayoutArtworks";
            flowLayoutArtworks.Size = new Size(580, 428);
            flowLayoutArtworks.TabIndex = 1;
            // 
            // lblArtistName
            // 
            lblArtistName.Font = new Font("Segoe UI Emoji", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblArtistName.Location = new Point(162, 50);
            lblArtistName.Name = "lblArtistName";
            lblArtistName.Size = new Size(172, 29);
            lblArtistName.TabIndex = 1;
            // 
            // lblBio
            // 
            lblBio.Location = new Point(162, 88);
            lblBio.Name = "lblBio";
            lblBio.Size = new Size(172, 32);
            lblBio.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Yu Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(37, 171);
            this.label1.Name = "label1";
            this.label1.Size = new Size(79, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Artworks";
            // 
            // ArtistProfileForm
            // 
            BackColor = Color.White;
            ClientSize = new Size(634, 711);
            Controls.Add(profilePanel);
            Name = "ArtistProfileForm";
            Text = "Artist Profile";
            profilePanel.ResumeLayout(false);
            profilePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picProfile).EndInit();
            ResumeLayout(false);
        }

        private void LoadArtistProfile()
        {
            using (MySqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT username, bio, profile_pic FROM users WHERE username = @username";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", artistUsername);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblArtistName.Text = reader.GetString("username");
                            lblBio.Text = reader.IsDBNull(reader.GetOrdinal("bio")) ? "" : reader.GetString("bio");

                            if (!reader.IsDBNull(reader.GetOrdinal("profile_pic")))
                            {
                                string imagePath = reader.GetString("profile_pic");
                                try
                                {
                                    if (File.Exists(imagePath))
                                    {
                                        picProfile.Image = Image.FromFile(imagePath);
                                    }
                                    else
                                    {
                                        picProfile.Image = SystemIcons.Warning.ToBitmap(); // fallback image
                                    }
                                }
                                catch
                                {
                                    picProfile.Image = SystemIcons.Error.ToBitmap(); // error fallback
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Artist not found.");
                            this.Close();
                        }
                    }
                }
            }
        }

        private void LoadArtworks()
        {
            using (MySqlConnection connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT art_title, category_name, image, created_at, like_count, art_description FROM user_artworks WHERE username = @username ORDER BY created_at DESC";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", artistUsername);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Artwork artwork = new Artwork
                            {
                                Title = reader.GetString("art_title"),
                                Category = reader.GetString("category_name"),
                                ImagePath = reader.GetString("image"),
                                CreatedAt = reader.GetDateTime("created_at"),
                                LikeCount = reader.GetInt32("like_count"),
                                Description = reader.IsDBNull(reader.GetOrdinal("art_description")) ? "" : reader.GetString("art_description")
                            };
                            flowLayoutArtworks.Controls.Add(CreateArtworkPanel(artwork));
                        }
                    }
                }
            }
        }

        private Panel CreateArtworkPanel(Artwork artwork)
        {
            Panel panel = new Panel
            {
                Width = 200,
                Height = 320,
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            PictureBox pictureBox = new PictureBox
            {
                Width = 180,
                Height = 140,
                SizeMode = PictureBoxSizeMode.Zoom,
                Top = 10,
                Left = 10
            };

            try
            {
                if (File.Exists(artwork.ImagePath))
                {
                    pictureBox.Image = Image.FromFile(artwork.ImagePath);
                }
                else
                {
                    pictureBox.Image = SystemIcons.Warning.ToBitmap(); // fallback image
                }
            }
            catch
            {
                pictureBox.Image = SystemIcons.Error.ToBitmap(); // error fallback
            }

            Label lblTitle = new Label
            {
                Text = artwork.Title,
                AutoSize = false,
                Width = 180,
                Top = pictureBox.Bottom + 5,
                Left = 10,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblCategory = new Label
            {
                Text = "Category: " + artwork.Category,
                AutoSize = false,
                Width = 180,
                Top = lblTitle.Bottom + 2,
                Left = 10,
                Font = new Font("Segoe UI", 9)
            };

            Label lblDescription = new Label
            {
                Text = artwork.Description,
                AutoSize = false,
                Width = 180,
                Height = 40,
                Top = lblCategory.Bottom + 2,
                Left = 10,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.DimGray
            };

            Label lblLikes = new Label
            {
                Text = "❤️ " + artwork.LikeCount + " likes",
                AutoSize = false,
                Width = 180,
                Top = lblDescription.Bottom + 2,
                Left = 10,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.DarkRed
            };

            Label lblCreatedAt = new Label
            {
                Text = "Uploaded: " + artwork.CreatedAt.ToString("MMMM dd, yyyy"),
                AutoSize = false,
                Width = 180,
                Top = lblLikes.Bottom + 2,
                Left = 10,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };

            panel.Controls.Add(pictureBox);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblCategory);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblLikes);
            panel.Controls.Add(lblCreatedAt);

            return panel;
        }

        private class Artwork
        {
            public string Title { get; set; }
            public string Category { get; set; }
            public string ImagePath { get; set; }
            public DateTime CreatedAt { get; set; }
            public int LikeCount { get; set; }
            public string Description { get; set; }
        }
    }
}
