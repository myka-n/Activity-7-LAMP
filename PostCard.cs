using System;
using System.Drawing;
using System.Windows.Forms;

namespace Activity_7
{
    public partial class PostCard : UserControl
    {
        private Post post;

        public PostCard(Post post)
        {
            InitializeComponent();
            this.post = post;
            LoadPostData(post);
            AddClickHandlers();
        }

        private void LoadPostData(Post post)
        {
            artworkimage.Image = post.Thumbnail;
            lbltitle.Text = post.Title;
            lblauthor.Text = $"by {post.Author}";
            lbllikes.Text = $"{post.LikeCount} ❤️";
            lbltimestamp.Text = post.Timestamp.ToString("g");
            lblcategories.Text = post.Category;
        }

        private void AddClickHandlers()
        {
            // Make the entire card clickable
            this.Click += PostCard_Click;
            artworkimage.Click += PostCard_Click;
            lbltitle.Click += PostCard_Click;
            lblcategories.Click += PostCard_Click;
            lbllikes.Click += PostCard_Click;
            lbltimestamp.Click += PostCard_Click;

            // Author name opens artist profile
            lblauthor.Click += (s, e) => {
                var profileForm = new ArtistProfileForm(post.Author);
                profileForm.Show();
                // Prevent the parent click event by stopping event propagation
                ((Control)s).Parent?.Controls.Remove((Control)s);
                ((Control)s).Parent?.Controls.Add((Control)s);
            };
        }

        private void PostCard_Click(object sender, EventArgs e)
        {
            if (post.ArtworkId > 0)
            {
                var artworkPage = new ArtworkPage(post.ArtworkId, Session.UserId);
                artworkPage.Show();
            }
        }

        private void PostCard_Load(object sender, EventArgs e)
        {
        }
    }
}
