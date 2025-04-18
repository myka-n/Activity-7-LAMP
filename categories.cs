using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Activity_7
{
    public partial class Categories : Form
    {
        MySqlConnection conn;

        public Categories()
        {
            InitializeComponent();
            this.Load += CategoriesPage_Load;
            conn = new MySqlConnection("server=localhost;user id=root;password=mykz;database=zeereal_artspace;");
        }

        private void CategoriesPage_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                conn.Open();
                string query = "SELECT cat_id, cat_name FROM categories ORDER BY cat_name";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    flowlayoutcategories.Controls.Clear(); // Clear previous category buttons

                    while (reader.Read())
                    {
                        Button btn = new Button();
                        btn.Width = 100;
                        btn.Height = 40;
                        btn.Text = reader.GetString("cat_name");
                        btn.Tag = reader.GetInt32("cat_id");
                        btn.Click += CategoryButton_Click;
                        flowlayoutcategories.Controls.Add(btn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is int categoryId)
            {
                LoadArtworksByCategory(categoryId);
            }
            else
            {
                MessageBox.Show("Invalid category button or missing category ID.");
            }
        }

        private void LoadArtworksByCategory(int categoryId)
        {
            bool hasData = false;

            try
            {
                conn.Open();
                string query = @"
                    SELECT 
                        a.art_title AS Title,
                        u.username AS ArtistName,
                        u.user_id AS ArtistId, 
                        a.created_at AS CreatedAt
                    FROM artworks a
                    JOIN artwork_categories ac ON a.art_id = ac.art_id
                    JOIN users u ON a.user_id = u.user_id
                    WHERE ac.cat_id = @categoryId
                    ORDER BY a.created_at DESC;
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@categoryId", categoryId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    flowlayoutartworks.Controls.Clear(); // Clear previous artworks

                    while (reader.Read())
                    {
                        hasData = true;

                        Panel artPanel = CreateArtworkPanel(reader);
                        flowlayoutartworks.Controls.Add(artPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading artworks: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();

                if (!hasData)
                {
                    MessageBox.Show("No artworks found under this category.", "No Artworks", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private Panel CreateArtworkPanel(MySqlDataReader reader)
        {
            Panel artPanel = new Panel
            {
                Width = 300,
                Height = 150,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            Label lblTitle = new Label
            {
                Text = reader.GetString("Title"),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30,
                Padding = new Padding(10, 10, 10, 0),
                ForeColor = Color.Black
            };

            Label lblArtist = new Label
            {
                Text = "by " + reader.GetString("ArtistName"),
                Font = new Font("Segoe UI", 10, FontStyle.Underline),
                Dock = DockStyle.Top,
                Height = 25,
                Padding = new Padding(10, 0, 10, 0),
                ForeColor = Color.Blue,
                Cursor = Cursors.Hand,
                Tag = reader.GetInt32("ArtistId") // store artistId now!
            };
            lblArtist.Click += ArtistLabel_Click;

            Label lblDate = new Label
            {
                Text = reader.GetDateTime("CreatedAt").ToString("MMMM dd, yyyy"),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 20,
                Padding = new Padding(10, 0, 10, 10),
                ForeColor = Color.DarkGray
            };

            artPanel.Controls.Add(lblDate);
            artPanel.Controls.Add(lblArtist);
            artPanel.Controls.Add(lblTitle);

            return artPanel;
        }

        private void ArtistLabel_Click(object sender, EventArgs e)
        {
            if (sender is Label artistLabel && artistLabel.Tag is int artistId)
            {
                ArtistProfileForm artistProfileForm = new ArtistProfileForm(artistId);
                artistProfileForm.Show();
            }
        }

        private void SearchLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string keyword = searchtextbox.Text.Trim();  // Assuming searchTextBox is the TextBox control
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
            try
            {
                conn.Open();
                string query = @"
                SELECT 
                    a.art_title AS Title,
                    u.username AS ArtistName,
                    u.user_id AS ArtistId, 
                    a.created_at AS CreatedAt
                FROM artworks a
                JOIN users u ON a.user_id = u.user_id
                WHERE a.art_title LIKE @keyword
                ORDER BY a.created_at DESC;
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    flowlayoutartworks.Controls.Clear(); // Clear previous artworks

                    bool hasData = false;

                    while (reader.Read())
                    {
                        hasData = true;

                        Panel artPanel = CreateArtworkPanel(reader);
                        flowlayoutartworks.Controls.Add(artPanel);
                    }

                    if (!hasData)
                    {
                        MessageBox.Show("No artworks matched your search.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching artworks: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
