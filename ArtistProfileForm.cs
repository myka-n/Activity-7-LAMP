using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Activity_7
{
    public partial class ArtistProfileForm : Form
    {
        private int artistId;
        private MySqlConnection conn;

        public ArtistProfileForm(int artistId)
        {
            InitializeComponent();
            this.artistId = artistId;
            conn = new MySqlConnection("server=localhost;user id=root;password=mykz;database=zeereal_artspace;");
            this.Load += ArtistProfile_Load;
        }

        private void ArtistProfile_Load(object sender, EventArgs e)
        {
            LoadArtistProfile();
            LoadArtistArtworks();
        }

        private void LoadArtistProfile()
        {
            try
            {
                conn.Open();
                string query = "SELECT username, bio, profile_pic FROM users WHERE user_id = @artistId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@artistId", artistId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblArtistName.Text = reader.GetString("username");
                        txtBio.Text = reader.IsDBNull(reader.GetOrdinal("bio")) ? "No bio available." : reader.GetString("bio");

                        if (!reader.IsDBNull(reader.GetOrdinal("profile_pic")))
                        {
                            byte[] imgData = (byte[])reader["profile_pic"];
                            using (var ms = new System.IO.MemoryStream(imgData))
                            {
                                pictureBoxProfile.Image = Image.FromStream(ms);
                            }
                        }
                        else
                        {
                            pictureBoxProfile.Image = Properties.Resources.Untitled58_20221225121714; // fallback picture
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading artist profile: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadArtistArtworks()
        {
            try
            {
                conn.Open();
                string query = @"
                    SELECT art_title, created_at
                    FROM artworks
                    WHERE user_id = @artistId
                    ORDER BY created_at DESC;
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@artistId", artistId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    flowLayoutArtworks.Controls.Clear();

                    while (reader.Read())
                    {
                        Panel artworkPanel = new Panel
                        {
                            Width = 250,
                            Height = 100,
                            BorderStyle = BorderStyle.FixedSingle,
                            BackColor = Color.White,
                            Margin = new Padding(5)
                        };

                        Label lblTitle = new Label
                        {
                            Text = reader.GetString("art_title"),
                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                            Dock = DockStyle.Top,
                            Height = 30,
                            Padding = new Padding(5)
                        };

                        Label lblDate = new Label
                        {
                            Text = reader.GetDateTime("created_at").ToString("MMMM dd, yyyy"),
                            Font = new Font("Segoe UI", 9),
                            Dock = DockStyle.Bottom,
                            Height = 20,
                            Padding = new Padding(5)
                        };

                        artworkPanel.Controls.Add(lblDate);
                        artworkPanel.Controls.Add(lblTitle);
                        flowLayoutArtworks.Controls.Add(artworkPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading artist artworks: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

       
    }
}
