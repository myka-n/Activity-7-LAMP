-- Create view for user artworks
CREATE OR REPLACE VIEW user_artworks AS
SELECT 
    a.art_id,
    a.user_id,
    a.art_title,
    a.art_description,
    a.image_path as image,
    a.created_at,
    u.username,
    c.cat_id,
    c.cat_name as category_name,
    (SELECT COUNT(*) FROM likes WHERE art_id = a.art_id) as like_count
FROM artworks a
JOIN users u ON a.user_id = u.user_id
LEFT JOIN artwork_categories ac ON a.art_id = ac.art_id
LEFT JOIN categories c ON ac.cat_id = c.cat_id; 