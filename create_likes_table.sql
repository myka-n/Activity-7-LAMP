-- Create likes table
CREATE TABLE IF NOT EXISTS likes (
    like_id INT AUTO_INCREMENT PRIMARY KEY,
    art_id INT NOT NULL,
    user_id INT NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (art_id) REFERENCES artworks(art_id),
    FOREIGN KEY (user_id) REFERENCES users(user_id),
    UNIQUE KEY unique_like (art_id, user_id)
);

-- Function to get total likes for an artwork
DELIMITER //
CREATE FUNCTION IF NOT EXISTS get_total_likes(p_art_id INT)
RETURNS INT
DETERMINISTIC
BEGIN
    DECLARE total_likes INT;
    SELECT COUNT(*) INTO total_likes
    FROM likes
    WHERE art_id = p_art_id;
    RETURN total_likes;
END //
DELIMITER ; 