using System;

namespace Activity_7
{
    public class Artwork
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
} 