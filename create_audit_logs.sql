CREATE TABLE IF NOT EXISTS audit_logs (
    log_id INT AUTO_INCREMENT PRIMARY KEY,
    admin_id INT NOT NULL,
    action VARCHAR(50) NOT NULL,
    details TEXT,
    timestamp DATETIME NOT NULL,
    ip_address VARCHAR(45),
    FOREIGN KEY (admin_id) REFERENCES users(user_id)
); 