using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Activity_7
{
    public partial class Categories : Form
    {
        public Categories()
        {
            InitializeComponent();
            this.Load += CategoriesPage_Load;
        }

        private void CategoriesPage_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            string query = "SELECT cat_id, cat_name FROM categories ORDER BY cat_name";
            
            DatabaseManager.ExecuteQuery(query, null, reader =>
            {
                flowlayoutcategories.Controls.Clear();
                while (reader.Read())
                {
                    var btn = CreateCategoryButton(
                        reader.GetString("cat_name"),
                        reader.GetInt32("cat_id")
                    );
                    flowlayoutcategories.Controls.Add(btn);
                }
            });
        }

        private Button CreateCategoryButton(string categoryName, int categoryId)
        {
            var btn = new Button
            {
                Width = 100,
                Height = 40,
                Text = categoryName,
                Tag = categoryId
            };
            btn.Click += CategoryButton_Click;
            return btn;
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is int categoryId)
            {
                LoadArtworksByCategory(categoryId);
            }
        }

        private void LoadArtworksByCategory(int categoryId)
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
                JOIN artwork_categories ac ON a.art_id = ac.art_id
                JOIN categories c ON ac.cat_id = c.cat_id
                WHERE ac.cat_id = @categoryId
                ORDER BY a.created_at DESC";

            bool hasData = false;

            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@categoryId", categoryId);
            }, reader =>
            {
                flowlayoutartworks.Controls.Clear();
                while (reader.Read())
                {
                    hasData = true;
                    var artwork = new Artwork
                    {
                        ArtworkId = reader.GetInt32("art_id"),
                        Title = reader.GetString("Title"),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                        ImagePath = reader.GetString("ImagePath"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        ArtistId = reader.GetInt32("ArtistId"),
                        ArtistName = reader.GetString("ArtistName"),
                        CategoryId = reader.GetInt32("CategoryId"),
                        CategoryName = reader.GetString("CategoryName")
                    };
                    flowlayoutartworks.Controls.Add(CreateArtworkPanel(artwork));
                }
            });

            if (!hasData)
            {
                MessageBox.Show("No artworks found under this category.", "No Artworks", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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

            var lblArtist = new Label
            {
                Text = "by " + artwork.ArtistName,
                Font = new Font("Segoe UI", 10, FontStyle.Underline),
                Dock = DockStyle.Top,
                Height = 25,
                Padding = new Padding(10, 0, 10, 0),
                ForeColor = Color.Blue,
                Cursor = Cursors.Hand,
                Tag = artwork.ArtistId
            };
            lblArtist.Click += ArtistLabel_Click;

            var lblDate = new Label
            {
                Text = artwork.CreatedAt.ToString("MMMM dd, yyyy"),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 20,
                Padding = new Padding(10, 0, 10, 10),
                ForeColor = Color.DarkGray
            };

            artPanel.Controls.Add(lblDate);
            artPanel.Controls.Add(lblArtist);
            artPanel.Controls.Add(lblTitle);
            artPanel.Controls.Add(pictureBox);

            return artPanel;
        }

        private void ArtistLabel_Click(object sender, EventArgs e)
        {
            if (sender is Label artistLabel && artistLabel.Tag is int artistId)
            {
                var artistProfileForm = new ArtistProfileForm(artistId);
                artistProfileForm.Show();
            }
        }

        private void SearchLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string keyword = searchtextbox.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                SearchAllArtworks(keyword);
            }
            else
            {
                MessageBox.Show("Please enter a keyword to search.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SearchAllArtworks(string keyword)
        {
            string query = @"
                SELECT 
                    a.art_id,
                    a.art_title AS Title,
                    a.art_description AS Description,
                    a.image_path AS ImagePath,
                    a.created_at AS CreatedAt,
                    u.user_id AS ArtistId,
                    u.username AS ArtistName
                FROM artworks a
                JOIN users u ON a.user_id = u.user_id
                WHERE a.art_title LIKE @keyword
                ORDER BY a.created_at DESC";

            bool hasData = false;

            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
            }, reader =>
            {
                flowlayoutartworks.Controls.Clear();
                while (reader.Read())
                {
                    hasData = true;
                    var artwork = new Artwork
                    {
                        ArtworkId = reader.GetInt32("art_id"),
                        Title = reader.GetString("Title"),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                        ImagePath = reader.GetString("ImagePath"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        ArtistId = reader.GetInt32("ArtistId"),
                        ArtistName = reader.GetString("ArtistName")
                    };
                    flowlayoutartworks.Controls.Add(CreateArtworkPanel(artwork));
                }
            });

            if (!hasData)
            {
                MessageBox.Show("No artworks matched your search.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
