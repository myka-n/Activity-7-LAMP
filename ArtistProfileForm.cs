using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Activity_7
{
    public partial class ArtistProfileForm : Form
    {
        private readonly int artistId;

        public ArtistProfileForm(int artistId)
        {
            InitializeComponent();
            this.artistId = artistId;
            this.Load += ArtistProfileForm_Load;
        }

        private void ArtistProfileForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadArtistProfile();
                LoadArtistArtworks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading artist profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadArtistProfile()
        {
            string query = @"
                SELECT 
                    u.username,
                    u.profile_pic,
                    u.bio
                FROM users u
                WHERE u.user_id = @artistId";

            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@artistId", artistId);
            }, reader =>
            {
                if (reader.Read())
                {
                    lblUsername.Text = reader.GetString("username");
                    txtBio.Text = reader.IsDBNull(reader.GetOrdinal("bio")) ? "" : reader.GetString("bio");

                    string profilePicPath = reader.IsDBNull(reader.GetOrdinal("profile_pic")) ? null : reader.GetString("profile_pic");
                    if (!string.IsNullOrEmpty(profilePicPath) && System.IO.File.Exists(profilePicPath))
                    {
                        pictureBoxProfile.Image = Image.FromFile(profilePicPath);
                    }
                }
            });
        }

        private void LoadArtistArtworks()
        {
            string query = @"
                SELECT 
                    a.art_id,
                    a.art_title AS Title,
                    a.art_description AS Description,
                    a.image_path AS ImagePath,
                    a.created_at AS CreatedAt,
                    u.user_id AS ArtistId,
                    u.username AS ArtistName,
                    c.cat_id AS CategoryId,
                    c.cat_name AS CategoryName
                FROM artworks a
                JOIN users u ON a.user_id = u.user_id
                LEFT JOIN artwork_categories ac ON a.art_id = ac.art_id
                LEFT JOIN categories c ON ac.cat_id = c.cat_id
                WHERE a.user_id = @artistId
                ORDER BY a.created_at DESC";

            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@artistId", artistId);
            }, reader =>
            {
                flowLayoutArtworks.Controls.Clear();
                while (reader.Read())
                {
                    var artwork = new Artwork
                    {
                        ArtworkId = reader.GetInt32("art_id"),
                        Title = reader.GetString("Title"),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                        ImagePath = reader.GetString("ImagePath"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        ArtistId = reader.GetInt32("ArtistId"),
                        ArtistName = reader.GetString("ArtistName"),
                        CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? null : (int?)reader.GetInt32("CategoryId"),
                        CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader.GetString("CategoryName")
                    };
                    flowLayoutArtworks.Controls.Add(CreateArtworkPanel(artwork));
                }
            });
        }

        private Panel CreateArtworkPanel(Artwork artwork)
        {
            var artPanel = new Panel
            {
                Width = 300,
                Height = 250,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            var pictureBox = new PictureBox
            {
                Width = 280,
                Height = 150,
                Dock = DockStyle.Top,
                SizeMode = PictureBoxSizeMode.Zoom,
                Padding = new Padding(10)
            };

            try
            {
                if (System.IO.File.Exists(artwork.ImagePath))
                {
                    pictureBox.Image = Image.FromFile(artwork.ImagePath);
                }
            }
            catch
            {
                pictureBox.Image = null;
            }

            var lblTitle = new Label
            {
                Text = artwork.Title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30,
                Padding = new Padding(10, 10, 10, 0),
                ForeColor = Color.Black
            };

            var lblCategory = new Label
            {
                Text = artwork.CategoryName ?? "Uncategorized",
                Font = new Font("Segoe UI", 10),
                Dock = DockStyle.Top,
                Height = 25,
                Padding = new Padding(10, 0, 10, 0),
                ForeColor = Color.DarkGray
            };

            var lblDate = new Label
            {
                Text = artwork.CreatedAt.ToString("MMMM dd, yyyy"),
                Font = new Font("Segoe UI", 9),
                Dock = DockStyle.Top,
                Height = 20,
                Padding = new Padding(10, 0, 10, 10),
                ForeColor = Color.DarkGray
            };

            artPanel.Controls.Add(lblDate);
            artPanel.Controls.Add(lblCategory);
            artPanel.Controls.Add(lblTitle);
            artPanel.Controls.Add(pictureBox);

            return artPanel;
        }
    }
}
