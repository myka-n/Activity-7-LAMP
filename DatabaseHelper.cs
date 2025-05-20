using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Activity_7
{
    public class Post
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Image Thumbnail { get; set; }
        public DateTime Timestamp { get; set; }
        public int LikeCount { get; set; }
        public string Category { get; set; }
    }

    public class DatabaseHelper
    {
        private string connectionString = "server=localhost;uid=root;pwd=mykz;database=zeereal_artspace";

        // ✅ Static method used by ArtistProfile and other forms
        public static MySqlConnection GetConnection()
        {
            string connStr = "server=localhost;uid=root;pwd=mykz;database=zeereal_artspace";
            return new MySqlConnection(connStr);
        }

        // Fetch posts for a specific user
        public List<Post> GetUserPosts(int userId)
        {
            List<Post> posts = new List<Post>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

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
                    WHERE user_id = @userId 
                    ORDER BY created_at DESC";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string imagePath = reader["image"].ToString();
                            Image img = null;
                            if (File.Exists(imagePath))
                            {
                                img = Image.FromFile(imagePath);
                            }

                            Post post = new Post
                            {
                                ArtworkId = Convert.ToInt32(reader["art_id"]),
                                Title = reader["art_title"].ToString(),
                                Author = reader["username"].ToString(),
                                Timestamp = Convert.ToDateTime(reader["created_at"]),
                                Category = reader["category_name"].ToString(),
                                LikeCount = Convert.ToInt32(reader["like_count"]),
                                Thumbnail = img
                            };

                            posts.Add(post);
                        }
                    }
                }
            }

            return posts;
        }

        public List<User> GetSuggestedArtists(int userId)
        {
            List<User> suggestedArtists = new List<User>();
            string query = @"
                SELECT DISTINCT u.user_id, u.username, u.profile_pic, u.bio
                FROM users u
                JOIN user_artworks ua ON u.user_id = ua.user_id
                JOIN categories c ON ua.cat_id = c.cat_id
                WHERE u.user_id != @userId
                AND u.user_id IN (
                    SELECT ua2.user_id
                    FROM user_artworks ua2
                    JOIN categories c2 ON ua2.cat_id = c2.cat_id
                    WHERE c2.cat_id IN (
                        SELECT cat_id 
                        FROM user_artworks 
                        WHERE user_id = @userId
                    )
                )
                ORDER BY RAND()
                LIMIT 5";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User artist = new User
                                {
                                    UserId = reader.GetInt32("user_id"),
                                    Username = reader.GetString("username"),
                                    FullName = reader.GetString("username"),
                                    ProfilePic = reader.IsDBNull(reader.GetOrdinal("profile_pic")) ? 
                                        null : reader.GetString("profile_pic"),
                                    Bio = reader.IsDBNull(reader.GetOrdinal("bio")) ? 
                                        null : reader.GetString("bio")
                                };
                                suggestedArtists.Add(artist);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error getting suggested artists: " + ex.Message);
                }
            }
            return suggestedArtists;
        }
    }
}
