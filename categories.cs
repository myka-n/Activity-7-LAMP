using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            bool hasData = false;
            flowlayoutartworks.Controls.Clear();

            string query = @"
                SELECT 
                    art_id,
                    art_title,
                    username,
                    image,
                    created_at,
                    category_name,
                    like_count
                FROM user_artworks 
                WHERE category_name = (
                    SELECT cat_name 
                    FROM categories 
                    WHERE cat_id = @categoryId
                )
                ORDER BY created_at DESC";

            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@categoryId", categoryId);
            }, reader =>
            {
                while (reader.Read())
                {
                    hasData = true;
                    string imagePath = reader["image"].ToString();
                    Image img = null;
                    if (File.Exists(imagePath))
                    {
                        img = Image.FromFile(imagePath);
                    }

                    var post = new Post
                    {
                        ArtworkId = reader.GetInt32("art_id"),
                        Title = reader.GetString("art_title"),
                        Author = reader.GetString("username"),
                        Timestamp = reader.GetDateTime("created_at"),
                        Category = reader.GetString("category_name"),
                        LikeCount = reader.GetInt32("like_count"),
                        Thumbnail = img
                    };

                    var card = new PostCard(post);
                    flowlayoutartworks.Controls.Add(card);
                }
            });

            if (!hasData)
            {
                MessageBox.Show("No artworks found under this category.", "No Artworks", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SearchArtworks(string keyword)
        {
            bool hasData = false;
            flowlayoutartworks.Controls.Clear();

            string query = @"
                SELECT 
                    art_id,
                    art_title,
                    username,
                    image,
                    created_at,
                    category_name,
                    like_count
                FROM user_artworks 
                WHERE art_title LIKE @keyword 
                   OR art_description LIKE @keyword 
                   OR username LIKE @keyword
                ORDER BY created_at DESC";

            DatabaseManager.ExecuteQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
            }, reader =>
            {
                while (reader.Read())
                {
                    hasData = true;
                    string imagePath = reader["image"].ToString();
                    Image img = null;
                    if (File.Exists(imagePath))
                    {
                        img = Image.FromFile(imagePath);
                    }

                    var post = new Post
                    {
                        ArtworkId = reader.GetInt32("art_id"),
                        Title = reader.GetString("art_title"),
                        Author = reader.GetString("username"),
                        Timestamp = reader.GetDateTime("created_at"),
                        Category = reader.GetString("category_name"),
                        LikeCount = reader.GetInt32("like_count"),
                        Thumbnail = img
                    };

                    var card = new PostCard(post);
                    flowlayoutartworks.Controls.Add(card);
                }
            });

            if (!hasData)
            {
                MessageBox.Show("No artworks matched your search.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SearchLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string keyword = searchtextbox.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                SearchArtworks(keyword);
            }
            else
            {
                MessageBox.Show("Please enter a keyword to search.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Home
            this.Hide();
            var homepage = new Homepage();
            homepage.FormClosed += (s, args) => this.Close();
            homepage.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Categories
            this.Hide();
            var categories = new Categories();
            categories.FormClosed += (s, args) => this.Close();
            categories.Show();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Upload
            var uploadForm = new Upload();
            uploadForm.Owner = this;
            this.Hide();
            uploadForm.ShowDialog();
            this.Show();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Profile -> attach profilemenustrip
            ProfileMenuStrip.Show(linkLabel5, new Point(0, linkLabel5.Height));
        }

        private void myAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myaccount = new MyAccount();
            myaccount.FormClosed += (s, args) => this.Close();
            myaccount.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Session.Clear();
            this.Hide();
            new Login().ShowDialog();
        }
    }
}
