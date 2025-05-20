using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Activity_7
{
    public partial class ArtistCard : UserControl
    {
        private User artist;

        public ArtistCard(User artist)
        {
            InitializeComponent();
            this.artist = artist;
            LoadArtistData();
        }

        private void LoadArtistData()
        {
            lblUsername.Text = $"@{artist.Username}";
            lblName.Text = artist.FullName;
            
            // Load profile picture if exists
            if (!string.IsNullOrEmpty(artist.ProfilePic))
            {
                try
                {
                    profilePic.Image = Image.FromFile(artist.ProfilePic);
                }
                catch
                {
                    // Create a simple placeholder image if loading fails
                    CreatePlaceholderProfilePic();
                }
            }
            else
            {
                CreatePlaceholderProfilePic();
            }

            // Add click event to view artist profile
            this.Click += ArtistCard_Click;
            profilePic.Click += ArtistCard_Click;
            lblUsername.Click += ArtistCard_Click;
            lblName.Click += ArtistCard_Click;
        }

        private void CreatePlaceholderProfilePic()
        {
            // Create a new bitmap for the placeholder
            Bitmap placeholder = new Bitmap(60, 60);
            using (Graphics g = Graphics.FromImage(placeholder))
            {
                // Fill with light gray background
                g.Clear(Color.LightGray);
                
                // Draw a circle
                using (Pen pen = new Pen(Color.Gray, 2))
                {
                    g.DrawEllipse(pen, 5, 5, 50, 50);
                }

                // Draw user icon (simple person shape)
                using (Pen pen = new Pen(Color.Gray, 2))
                {
                    // Head
                    g.DrawEllipse(pen, 20, 10, 20, 20);
                    // Body
                    g.DrawLine(pen, 30, 30, 30, 45);
                    // Arms
                    g.DrawLine(pen, 30, 35, 15, 40);
                    g.DrawLine(pen, 30, 35, 45, 40);
                    // Legs
                    g.DrawLine(pen, 30, 45, 20, 55);
                    g.DrawLine(pen, 30, 45, 40, 55);
                }
            }
            profilePic.Image = placeholder;
        }

        private void ArtistCard_Click(object sender, EventArgs e)
        {
            var profileForm = new ArtistProfileForm(artist.UserId.ToString());
            profileForm.Show();
        }
    }
} 