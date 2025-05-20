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
using System.Configuration;
using System.Drawing.Drawing2D;
using System.IO;

namespace Activity_7
{
    public partial class Homepage : Form
    {
        public Homepage()
        {
            InitializeComponent();
            LoadUserPosts();

            int userId = Session.UserId;
            string username = Session.Username;

            Console.WriteLine("User ID: " + userId);
            Console.WriteLine("Username: " + username);

            new WelcomePopup(Session.Username).ShowDialog();
            label1.Text = $"Welcome, {Session.Username}!";
        }

        private void home_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var homepage = new Homepage();
            homepage.FormClosed += (s, args) => this.Close();
            homepage.Show();
        }

        private void explore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var explore = new explore();
            explore.FormClosed += (s, args) => this.Close();
            explore.Show();
        }

        private void categories_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            var categories = new Categories();
            categories.FormClosed += (s, args) => this.Close();
            categories.Show();
        }

        private void post_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var uploadForm = new Upload();
            uploadForm.Owner = this;
            this.Hide();
            uploadForm.ShowDialog();
            this.Show();
        }

        private void profile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProfileMenuStrip.Show(profile, new Point(0, profile.Height));
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Welcome user message click handler
        }

        private void LoadUserPosts()
        {
            int userId = Session.UserId;
            DatabaseHelper db = new DatabaseHelper();
            List<Post> posts = db.GetUserPosts(userId);
            List<User> suggestedArtists = db.GetSuggestedArtists(userId);

            // Load artworks
            flowLayoutPanel4.Controls.Clear();
            foreach (var post in posts)
            {
                PostCard card = new PostCard(post);
                flowLayoutPanel4.Controls.Add(card);
            }

            // Load suggested artists
            flowLayoutPanel5.Controls.Clear();
            foreach (var artist in suggestedArtists)
            {
                ArtistCard card = new ArtistCard(artist);
                flowLayoutPanel5.Controls.Add(card);
            }
        }

        private void flowLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

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
            
        }

        private void logoutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Session.Clear();
            this.Hide();
            new Login().ShowDialog();
        }
    }
}
