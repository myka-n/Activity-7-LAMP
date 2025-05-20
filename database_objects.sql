-- Trigger for artwork upload logging
DELIMITER //
CREATE TRIGGER after_artwork_insert
AFTER INSERT ON artworks
FOR EACH ROW
BEGIN
    INSERT INTO artwork_logs (art_id, action, timestamp)
    VALUES (NEW.art_id, 'UPLOAD', NOW());
END //
DELIMITER ;

-- Trigger for user activity tracking
DELIMITER //
CREATE TRIGGER after_user_login
AFTER UPDATE ON users
FOR EACH ROW
BEGIN
    IF NEW.last_login != OLD.last_login THEN
        INSERT INTO user_activity_logs (user_id, activity_type, timestamp)
        VALUES (NEW.user_id, 'LOGIN', NOW());
    END IF;
END //
DELIMITER ;

-- Function to calculate user's total artwork count
DELIMITER //
CREATE FUNCTION get_user_artwork_count(user_id INT)
RETURNS INT
DETERMINISTIC
BEGIN
    DECLARE artwork_count INT;
    SELECT COUNT(*) INTO artwork_count
    FROM artworks
    WHERE user_id = user_id;
    RETURN artwork_count;
END //
DELIMITER ;

-- Function to check if user is active
DELIMITER //
CREATE FUNCTION is_user_active(user_id INT)
RETURNS BOOLEAN
DETERMINISTIC
BEGIN
    DECLARE active_status BOOLEAN;
    SELECT is_active INTO active_status
    FROM users
    WHERE user_id = user_id;
    RETURN active_status;
END //
DELIMITER ;

-- Stored Procedure for user statistics
DELIMITER //
CREATE PROCEDURE get_user_statistics(IN user_id INT)
BEGIN
    SELECT 
        u.username,
        u.email,
        COUNT(a.art_id) as total_artworks,
        MAX(a.created_at) as last_upload,
        COUNT(DISTINCT ac.cat_id) as categories_used
    FROM users u
    LEFT JOIN artworks a ON u.user_id = a.user_id
    LEFT JOIN artwork_categories ac ON a.art_id = ac.art_id
    WHERE u.user_id = user_id
    GROUP BY u.user_id;
END //
DELIMITER ;

-- Stored Procedure for category statistics
DELIMITER //
CREATE PROCEDURE get_category_statistics()
BEGIN
    SELECT 
        c.cat_name,
        COUNT(ac.art_id) as artwork_count,
        COUNT(DISTINCT a.user_id) as artist_count
    FROM categories c
    LEFT JOIN artwork_categories ac ON c.cat_id = ac.cat_id
    LEFT JOIN artworks a ON ac.art_id = a.art_id
    GROUP BY c.cat_id
    ORDER BY artwork_count DESC;
END //
DELIMITER ;

-- Event to clean up expired password reset tokens
DELIMITER //
CREATE EVENT cleanup_reset_tokens
ON SCHEDULE EVERY 1 DAY
DO
BEGIN
    DELETE FROM password_reset_tokens
    WHERE expiry_date < NOW() OR used = 1;
END //
DELIMITER ;

-- Event to update user statistics
DELIMITER //
CREATE EVENT update_user_statistics
ON SCHEDULE EVERY 1 HOUR
DO
BEGIN
    UPDATE users u
    SET artwork_count = (
        SELECT COUNT(*) 
        FROM artworks a 
        WHERE a.user_id = u.user_id
    );
END //
DELIMITER ; 