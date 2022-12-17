
/* Database table creation scripts for Cryptocurrency Investment Simulator */


/* Time zone table */
CREATE TABLE `time_zone` (
  `zone_id` int NOT NULL AUTO_INCREMENT,
  `change` int NOT NULL,
  PRIMARY KEY (`zone_id`)
);


/* User table */
CREATE TABLE `user` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `email` varchar(45) NOT NULL,
  `username` varchar(16) NOT NULL,
  `avatar_url` varchar(512) NOT NULL,
  `verified` int NOT NULL,
  `zone_id` int NOT NULL,
  PRIMARY KEY (`user_id`),
  KEY `fk_user_time_zone_idx` (`zone_id`),
  CONSTRAINT `fk_user_time_zone` FOREIGN KEY (`zone_id`) REFERENCES `time_zone` (`zone_id`)
);
  
  
/* Wallet table */
CREATE TABLE `wallet` (
  `wallet_id` int NOT NULL AUTO_INCREMENT,
  `symbol` varchar(4) NOT NULL,
  `balance` decimal(12,6) NOT NULL,
  `user_id` int NOT NULL,
  PRIMARY KEY (`wallet_id`),
  KEY `fk_wallet_user1_idx` (`user_id`),
  CONSTRAINT `fk_wallet_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`)
);
  
  
/* Cryptocurrency symbol table */
CREATE TABLE `crypto_symbol` (
  `crypto_id` int NOT NULL AUTO_INCREMENT,
  `symbol` varchar(4) NOT NULL,
  PRIMARY KEY (`crypto_id`)
);
  
  
/* Fiat symbol table */
CREATE TABLE `fiat_symbol` (
  `fiat_id` int NOT NULL AUTO_INCREMENT,
  `symbol` varchar(4) NOT NULL,
  PRIMARY KEY (`fiat_id`)
);
  
  
/* Market data table */
CREATE TABLE `market_data` (
  `data_id` int NOT NULL AUTO_INCREMENT,
  `date_time` datetime NOT NULL,
  `unit_value` decimal(12,6) NOT NULL,
  `daily_change` decimal(6,2) NOT NULL,
  `weekly_change` decimal(6,2) NOT NULL,
  `crypto_id` int NOT NULL,
  `fiat_id` int NOT NULL,
  PRIMARY KEY (`data_id`),
  KEY `fk_market_data_crypto_symbol1_idx` (`crypto_id`),
  KEY `fk_market_data_fiat_symbol1_idx` (`fiat_id`),
  CONSTRAINT `fk_market_data_crypto_symbol1` FOREIGN KEY (`crypto_id`) REFERENCES `crypto_symbol` (`crypto_id`),
  CONSTRAINT `fk_market_data_fiat_symbol1` FOREIGN KEY (`fiat_id`) REFERENCES `fiat_symbol` (`fiat_id`)
);


/* Leverage ratio table */
CREATE TABLE `leverage_ratio` (
  `ratio_id` int NOT NULL AUTO_INCREMENT,
  `multiplier` int NOT NULL,
  PRIMARY KEY (`ratio_id`)
);


/* Transaction status table */
CREATE TABLE `status` (
  `status_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(10) NOT NULL,
  PRIMARY KEY (`status_id`)
);


/* Transaction table */
CREATE TABLE `transaction` (
  `transaction_id` int NOT NULL AUTO_INCREMENT,
  `date_time` datetime NOT NULL,
  `fiat_amount` decimal(12,6) NOT NULL,
  `crypto_amount` decimal(12,6) NOT NULL,
  `margin` decimal(12,6) DEFAULT NULL,
  `ratio_id` int NOT NULL,
  `status_id` int NOT NULL,
  `wallet_id` int NOT NULL,
  `data_id` int NOT NULL,
  PRIMARY KEY (`transaction_id`),
  KEY `fk_transaction_leverage_ratio1_idx` (`ratio_id`),
  KEY `fk_transaction_status1_idx` (`status_id`),
  KEY `fk_transaction_wallet1_idx` (`wallet_id`),
  KEY `fk_transaction_market_data1_idx` (`data_id`),
  CONSTRAINT `fk_transaction_leverage_ratio1` FOREIGN KEY (`ratio_id`) REFERENCES `leverage_ratio` (`ratio_id`),
  CONSTRAINT `fk_transaction_market_data1` FOREIGN KEY (`data_id`) REFERENCES `market_data` (`data_id`),
  CONSTRAINT `fk_transaction_status1` FOREIGN KEY (`status_id`) REFERENCES `status` (`status_id`),
  CONSTRAINT `fk_transaction_wallet1` FOREIGN KEY (`wallet_id`) REFERENCES `wallet` (`wallet_id`)
);
