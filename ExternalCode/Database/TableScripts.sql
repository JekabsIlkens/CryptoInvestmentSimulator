
/* Database table creation scripts for Cryptocurrency Investment Simulator */

/* User table */
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `avatar_url` varchar(512) DEFAULT NULL,
  `is_verified` int NOT NULL DEFAULT '0',
  `time_zone` varchar(10) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_unicode_ci
