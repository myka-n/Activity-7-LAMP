using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Activity_7
{
    public partial class Upload : Form
    {
        private string artworkDirectory = Path.Combine(Application.StartupPath, "Artworks");
        private string selectedImagePath;
        private string connectionString;

        public Upload()
        {
            InitializeComponent();
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Database connection string is not configured.");
                }
                Directory.CreateDirectory(artworkDirectory);
                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void LoadCategories()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                
                var command = new MySqlCommand("SELECT cat_id, cat_name FROM categories ORDER BY cat_name", connection);
                using (var reader = command.ExecuteReader())
                {
                    cmbCategory.Items.Clear(); // Clear existing items
                    while (reader.Read())
                    {
                        cmbCategory.Items.Add(new CategoryItem
                        {
                            CategoryId = reader.GetInt32("cat_id"),
                            CategoryName = reader.GetString("cat_name")
                        });
                    }
                }
                cmbCategory.DisplayMember = "CategoryName";
            }
            catch (MySqlException ex)
            {
                string errorMessage = "Database error: ";
                switch (ex.Number)
                {
                    case 1042: // Cannot connect to server
                        errorMessage += "Cannot connect to database server.";
                        break;
                    case 1045: // Access denied
                        errorMessage += "Invalid database credentials.";
                        break;
                    case 1146: // Table doesn't exist
                        errorMessage += "Required database tables are missing.";
                        break;
                    default:
                        errorMessage += ex.Message;
                        break;
                }
                MessageBox.Show(errorMessage, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void BtnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Select Artwork";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        selectedImagePath = openFileDialog.FileName;
                        using (var image = Image.FromFile(selectedImagePath))
                        {
                            previewBox.Image = new Bitmap(image);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title for your artwork.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(selectedImagePath))
            {
                MessageBox.Show("Please select an image to upload.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                string newFileName = $"artwork_{Session.UserId}_{DateTime.Now.Ticks}{Path.GetExtension(selectedImagePath)}";
                string destPath = Path.Combine(artworkDirectory, newFileName);
                File.Copy(selectedImagePath, destPath, true);

                connection = new MySqlConnection(connectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                var currentTime = DateTime.Now;
                
                // Insert artwork
                var artworkCommand = new MySqlCommand(
                    @"INSERT INTO artworks (user_id, art_title, art_description, image_path, created_at, updated_at) 
                      VALUES (@user_id, @title, @description, @image_path, @created_at, @updated_at);
                      SELECT LAST_INSERT_ID();", 
                    connection, transaction);

                artworkCommand.Parameters.AddWithValue("@user_id", Session.UserId);
                artworkCommand.Parameters.AddWithValue("@title", txtTitle.Text);
                artworkCommand.Parameters.AddWithValue("@description", txtDescription.Text);
                artworkCommand.Parameters.AddWithValue("@image_path", destPath);
                artworkCommand.Parameters.AddWithValue("@created_at", currentTime);
                artworkCommand.Parameters.AddWithValue("@updated_at", currentTime);

                // Get the new artwork ID
                int artworkId = Convert.ToInt32(artworkCommand.ExecuteScalar());

                // Get the selected category ID
                var selectedCategory = (CategoryItem)cmbCategory.SelectedItem;

                // Insert the artwork-category relationship
                var categoryCommand = new MySqlCommand(
                    @"INSERT INTO artwork_categories (art_id, cat_id) 
                      VALUES (@art_id, @cat_id)",
                    connection, transaction);

                categoryCommand.Parameters.AddWithValue("@art_id", artworkId);
                categoryCommand.Parameters.AddWithValue("@cat_id", selectedCategory.CategoryId);
                categoryCommand.ExecuteNonQuery();

                transaction.Commit();
                MessageBox.Show("Artwork uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Hide this form and show the previous form
                this.Hide();
                if (this.Owner != null)
                {
                    this.Owner.Show();
                }
                this.Close();
            }
            catch (MySqlException ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                string errorMessage = "Database error: ";
                switch (ex.Number)
                {
                    case 1042: // Cannot connect to server
                        errorMessage += "Cannot connect to database server.";
                        break;
                    case 1045: // Access denied
                        errorMessage += "Invalid database credentials.";
                        break;
                    case 1146: // Table doesn't exist
                        errorMessage += "Required database tables are missing.";
                        break;
                    case 1452: // Foreign key constraint fails
                        errorMessage += "Invalid category selected.";
                        break;
                    default:
                        errorMessage += ex.Message;
                        break;
                }
                MessageBox.Show(errorMessage, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                MessageBox.Show($"Error uploading artwork: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    public class CategoryItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
