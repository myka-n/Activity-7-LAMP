-- Table for artwork logs
CREATE TABLE IF NOT EXISTS artwork_logs (
    log_id INT AUTO_INCREMENT PRIMARY KEY,
    art_id INT NOT NULL,
    action VARCHAR(50) NOT NULL,
    timestamp DATETIME NOT NULL,
    FOREIGN KEY (art_id) REFERENCES artworks(art_id)
);

-- Table for user activity logs
CREATE TABLE IF NOT EXISTS user_activity_logs (
    log_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    activity_type VARCHAR(50) NOT NULL,
    timestamp DATETIME NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(user_id)
);

-- Add artwork_count column to users table if it doesn't exist
ALTER TABLE users
ADD COLUMN IF NOT EXISTS artwork_count INT DEFAULT 0; 