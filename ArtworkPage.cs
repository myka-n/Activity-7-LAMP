using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;

namespace Activity_7
{
    public partial class ArtworkPage : Form
    {
        private int artworkId;
        private int currentUserId;
        private bool isLiked;

        public ArtworkPage(int artworkId, int currentUserId)
        {
            InitializeComponent();
            this.artworkId = artworkId;
            this.currentUserId = currentUserId;
            
            // Set up the form
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);
            
            // Initialize UI
            InitializeUI();
            LoadArtworkDetails();
            CheckIfLiked();
        }

        private void InitializeUI()
        {
            // Configure artwork image
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            
            // Configure artist profile picture
            pictureBox12.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox12.BorderStyle = BorderStyle.FixedSingle;
            
            // Configure comment section
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
            
            // Configure comment input
            textBox2.Multiline = true;
            textBox2.Height = 60;
            textBox2.PlaceholderText = "Write a comment...";
            
            // Configure comment sort dropdown
            comboBox1.Items.AddRange(new string[] { "Most recent comments", "Top Comments" });
            comboBox1.SelectedIndex = 0;
            
            // Make panels rounded
            MakePanelRounded(panel7, 20);
            MakePanelRounded(panel10, 20);
        }

        private void LoadArtworkDetails()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            a.*,
                            u.username as artist_name,
                            u.profile_pic as artist_pic,
                            c.cat_name,
                            (SELECT COUNT(*) FROM likes WHERE art_id = a.art_id) as total_likes,
                            a.created_at
                        FROM artworks a
                        JOIN users u ON a.user_id = u.user_id
                        LEFT JOIN artwork_categories ac ON a.art_id = ac.art_id
                        LEFT JOIN categories c ON ac.cat_id = c.cat_id
                        WHERE a.art_id = @ArtworkId";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ArtworkId", artworkId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Update form title and artwork details
                            this.Text = reader["art_title"].ToString();
                            label9.Text = reader["art_title"].ToString();
                            label6.Text = reader["art_description"].ToString();
                            label19.Text = $"Category: {reader["cat_name"]?.ToString() ?? "Uncategorized"}";
                            label22.Text = $"Posted on: {Convert.ToDateTime(reader["created_at"]).ToString("MMMM dd, yyyy")}";

                            // Load artwork image
                            string imagePath = reader["image_path"].ToString();
                            if (File.Exists(imagePath))
                            {
                                using (var img = Image.FromFile(imagePath))
                                {
                                    pictureBox1.Image = new Bitmap(img);
                                }
                            }
                            else
                            {
                                pictureBox1.Image = null;
                                label6.Text += "\n\n[Image not found]";
                            }

                            // Display artist info
                            label15.Text = reader["artist_name"].ToString();
                            string artistPicPath = reader["artist_pic"].ToString();
                            if (!string.IsNullOrEmpty(artistPicPath) && File.Exists(artistPicPath))
                            {
                                using (var img = Image.FromFile(artistPicPath))
                                {
                                    pictureBox12.Image = new Bitmap(img);
                                }
                            }
                            else
                            {
                                pictureBox12.Image = null;
                            }

                            // Update likes count
                            int totalLikes = Convert.ToInt32(reader["total_likes"]);
                            likesLabel.Text = $"{totalLikes:N0} likes";

                            // Load comments
                            LoadComments();
                        }
                        else
                        {
                            MessageBox.Show("Artwork not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading artwork: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadComments()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            c.*,
                            u.username,
                            u.profile_pic
                        FROM comments c
                        JOIN users u ON c.user_id = u.user_id
                        WHERE c.art_id = @ArtworkId
                        ORDER BY c.created_at DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ArtworkId", artworkId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        flowLayoutPanel1.Controls.Clear();
                        while (reader.Read())
                        {
                            var commentPanel = CreateCommentPanel(
                                reader["username"].ToString(),
                                reader["profile_pic"].ToString(),
                                reader["comment_text"].ToString(),
                                Convert.ToDateTime(reader["created_at"]),
                                0  // Set like count to 0 since we don't have comment likes
                            );
                            flowLayoutPanel1.Controls.Add(commentPanel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading comments: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private Panel CreateCommentPanel(string username, string profilePicPath, string commentText, DateTime timestamp, int likeCount)
        {
            var panel = new Panel
            {
                Width = flowLayoutPanel1.Width - 20,
                Height = 100,
                Margin = new Padding(5),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Profile picture
            var picBox = new PictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            if (!string.IsNullOrEmpty(profilePicPath) && File.Exists(profilePicPath))
            {
                using (var img = Image.FromFile(profilePicPath))
                {
                    picBox.Image = new Bitmap(img);
                }
            }

            // Username (clickable to view profile)
            var lblUsername = new LinkLabel
            {
                Text = username,
                AutoSize = true,
                Location = new Point(60, 10),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            lblUsername.Click += (s, e) => {
                var profileForm = new ArtistProfileForm(username);
                profileForm.Show();
            };

            // Comment text
            var lblComment = new Label
            {
                Text = commentText,
                AutoSize = false,
                Width = panel.Width - 80,
                Location = new Point(60, 30),
                Font = new Font("Segoe UI", 9)
            };

            // Timestamp
            var lblInfo = new Label
            {
                Text = timestamp.ToString("MMM dd, yyyy HH:mm"),
                AutoSize = true,
                Location = new Point(60, 70),
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray
            };

            panel.Controls.AddRange(new Control[] { picBox, lblUsername, lblComment, lblInfo });
            return panel;
        }

        private void CheckIfLiked()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM likes WHERE art_id = @ArtworkId AND user_id = @UserId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ArtworkId", artworkId);
                    cmd.Parameters.AddWithValue("@UserId", currentUserId);

                    isLiked = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    UpdateLikeButton();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking like status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateLikeButton()
        {
            btnLike.Text = isLiked ? "❤ Liked" : "❤ Like";
            btnLike.BackColor = isLiked ? Color.LightPink : SystemColors.Control;
        }

        private void BtnLike_Click(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    if (isLiked)
                    {
                        // Unlike
                        string query = "DELETE FROM likes WHERE art_id = @ArtworkId AND user_id = @UserId";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ArtworkId", artworkId);
                        cmd.Parameters.AddWithValue("@UserId", currentUserId);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // Like
                        string query = "INSERT INTO likes (art_id, user_id) VALUES (@ArtworkId, @UserId)";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ArtworkId", artworkId);
                        cmd.Parameters.AddWithValue("@UserId", currentUserId);
                        cmd.ExecuteNonQuery();
                    }

                    isLiked = !isLiked;
                    UpdateLikeButton();
                    LoadArtworkDetails(); // Refresh likes count
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating like: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MakePanelRounded(Panel panel, int radius)
        {
            if (panel.Width > 0 && panel.Height > 0)
            {
                Rectangle bounds = new Rectangle(0, 0, panel.Width, panel.Height);
                int diameter = radius * 2;
                GraphicsPath path = new GraphicsPath();

                path.StartFigure();
                path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
                path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();

                panel.Region = new Region(path);
            }
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            int radius = 20;
            int borderThickness = 2;
            Color borderColor = Color.Gray;

            Rectangle bounds = new Rectangle(0, 0, panel7.Width, panel7.Height);
            GraphicsPath path = new GraphicsPath();

            int diameter = radius * 2;
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter - 1, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter - 1, bounds.Bottom - diameter - 1, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter - 1, diameter, diameter, 90, 90);
            path.CloseFigure();

            using (Pen pen = new Pen(borderColor, borderThickness))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            // Add a subtle border to panel10
            using (Pen pen = new Pen(Color.LightGray, 1))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, panel10.Width - 1, panel10.Height - 1));
            }
        }

        // Navigation and menu handlers
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var homepage = new Homepage();
            homepage.FormClosed += (s, args) => this.Close();
            homepage.Show();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            linkLabel1_LinkClicked(sender, new LinkLabelLinkClickedEventArgs(null));
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProfileMenuStrip.Show(linkLabel5, new Point(0, linkLabel5.Height));
        }

        private void myAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myAccount = new MyAccount();
            myAccount.FormClosed += (s, args) => this.Close();
            myAccount.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Session.Clear();
            this.Hide();
            new Login().ShowDialog();
        }

        // Comment handlers
        private void button1_Click(object sender, EventArgs e)
        {
            string commentText = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(commentText))
            {
                MessageBox.Show("Please enter a comment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    // Insert comment and get its ID
                    string insertQuery = @"
                        INSERT INTO comments (art_id, user_id, comment_text, created_at)
                        VALUES (@ArtworkId, @UserId, @CommentText, NOW());
                        SELECT LAST_INSERT_ID();";

                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@ArtworkId", artworkId);
                    cmd.Parameters.AddWithValue("@UserId", currentUserId);
                    cmd.Parameters.AddWithValue("@CommentText", commentText);
                    
                    int newCommentId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Get the new comment data
                    string fetchQuery = @"
                        SELECT 
                            c.*,
                            u.username,
                            u.profile_pic,
                            0 as like_count
                        FROM comments c
                        JOIN users u ON c.user_id = u.user_id
                        WHERE c.comment_id = @CommentId";

                    cmd = new MySqlCommand(fetchQuery, conn);
                    cmd.Parameters.AddWithValue("@CommentId", newCommentId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Create new comment panel
                            var commentPanel = CreateCommentPanel(
                                reader["username"].ToString(),
                                reader["profile_pic"].ToString(),
                                reader["comment_text"].ToString(),
                                Convert.ToDateTime(reader["created_at"]),
                                0 // New comment starts with 0 likes
                            );

                            // Add to the top of the comments
                            flowLayoutPanel1.Controls.Add(commentPanel);
                            flowLayoutPanel1.Controls.SetChildIndex(commentPanel, 0);

                            // Clear input and scroll to new comment
                            textBox2.Clear();
                            flowLayoutPanel1.ScrollControlIntoView(commentPanel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error posting comment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadComments();
        }
    }
}

