using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Activity_7
{
    public partial class EditArtworkForm : Form
    {
        private int artworkId;
        private string currentImagePath;
        private string connectionString;
        private string artworkDirectory;

        public EditArtworkForm(int artworkId)
        {
            InitializeComponent();
            this.artworkId = artworkId;
            this.artworkDirectory = Path.Combine(Application.StartupPath, "Artworks");
            this.connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString;
            
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Database connection string is not configured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            LoadCategories();
            LoadArtworkData();
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
                    cmbCategory.Items.Clear();
                    while (reader.Read())
                    {
                        cmbCategory.Items.Add(new ArtworkCategoryItem
                        {
                            CategoryId = reader.GetInt32("cat_id"),
                            CategoryName = reader.GetString("cat_name")
                        });
                    }
                }
                cmbCategory.DisplayMember = "CategoryName";
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

        private void LoadArtworkData()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                // Get artwork details
                var artworkCommand = new MySqlCommand(
                    @"SELECT a.art_title, a.art_description, a.image_path, ac.cat_id 
                      FROM artworks a 
                      LEFT JOIN artwork_categories ac ON a.art_id = ac.art_id 
                      WHERE a.art_id = @artworkId", 
                    connection);

                artworkCommand.Parameters.AddWithValue("@artworkId", artworkId);

                using (var reader = artworkCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtTitle.Text = reader.GetString("art_title");
                        txtDescription.Text = reader.IsDBNull(reader.GetOrdinal("art_description")) ? "" : reader.GetString("art_description");
                        currentImagePath = reader.GetString("image_path");

                        // Load and display the image
                        if (File.Exists(currentImagePath))
                        {
                            using (var image = Image.FromFile(currentImagePath))
                            {
                                pictureBox.Image = new Bitmap(image);
                            }
                        }

                        // Set the category if it exists
                        if (!reader.IsDBNull(reader.GetOrdinal("cat_id")))
                        {
                            int categoryId = reader.GetInt32("cat_id");
                            foreach (ArtworkCategoryItem item in cmbCategory.Items)
                            {
                                if (item.CategoryId == categoryId)
                                {
                                    cmbCategory.SelectedItem = item;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading artwork data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void ChangeImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Select New Artwork Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string newImagePath = openFileDialog.FileName;
                        string newFileName = $"artwork_{artworkId}_{DateTime.Now.Ticks}{Path.GetExtension(newImagePath)}";
                        string destPath = Path.Combine(artworkDirectory, newFileName);

                        // Copy the new image
                        File.Copy(newImagePath, destPath, true);

                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(currentImagePath) && File.Exists(currentImagePath))
                        {
                            File.Delete(currentImagePath);
                        }

                        // Update the current image path and display
                        currentImagePath = destPath;
                        using (var image = Image.FromFile(currentImagePath))
                        {
                            pictureBox.Image = new Bitmap(image);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error changing image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title for your artwork.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                // Update artwork details
                var artworkCommand = new MySqlCommand(
                    @"UPDATE artworks 
                      SET art_title = @title, 
                          art_description = @description, 
                          image_path = @image_path,
                          updated_at = @updated_at
                      WHERE art_id = @artworkId",
                    connection, transaction);

                artworkCommand.Parameters.AddWithValue("@title", txtTitle.Text);
                artworkCommand.Parameters.AddWithValue("@description", txtDescription.Text);
                artworkCommand.Parameters.AddWithValue("@image_path", currentImagePath);
                artworkCommand.Parameters.AddWithValue("@updated_at", DateTime.Now);
                artworkCommand.Parameters.AddWithValue("@artworkId", artworkId);

                artworkCommand.ExecuteNonQuery();

                // Update category
                var categoryCommand = new MySqlCommand(
                    @"DELETE FROM artwork_categories WHERE art_id = @artworkId;
                      INSERT INTO artwork_categories (art_id, cat_id) VALUES (@artworkId, @categoryId);",
                    connection, transaction);

                if (cmbCategory.SelectedItem != null)
                {
                    var selectedCategory = (ArtworkCategoryItem)cmbCategory.SelectedItem;
                    categoryCommand.Parameters.AddWithValue("@categoryId", selectedCategory.CategoryId);
                    categoryCommand.Parameters.AddWithValue("@artworkId", artworkId);
                    categoryCommand.ExecuteNonQuery();
                }

                transaction.Commit();
                MessageBox.Show("Artwork updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                MessageBox.Show($"Error saving artwork: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 