
/* Database insertion scripts for helper tables */


/* All supported time zones */
INSERT INTO time_zone (time_zone.change) VALUES (-12);
INSERT INTO time_zone (time_zone.change) VALUES (-11);
INSERT INTO time_zone (time_zone.change) VALUES (-10);
INSERT INTO time_zone (time_zone.change) VALUES (-9);
INSERT INTO time_zone (time_zone.change) VALUES (-8);
INSERT INTO time_zone (time_zone.change) VALUES (-7);
INSERT INTO time_zone (time_zone.change) VALUES (-6);
INSERT INTO time_zone (time_zone.change) VALUES (-5);
INSERT INTO time_zone (time_zone.change) VALUES (-4);
INSERT INTO time_zone (time_zone.change) VALUES (-3);
INSERT INTO time_zone (time_zone.change) VALUES (-2);
INSERT INTO time_zone (time_zone.change) VALUES (-1);
INSERT INTO time_zone (time_zone.change) VALUES (0);
INSERT INTO time_zone (time_zone.change) VALUES (1);
INSERT INTO time_zone (time_zone.change) VALUES (2);
INSERT INTO time_zone (time_zone.change) VALUES (3);
INSERT INTO time_zone (time_zone.change) VALUES (4);
INSERT INTO time_zone (time_zone.change) VALUES (5);
INSERT INTO time_zone (time_zone.change) VALUES (6);
INSERT INTO time_zone (time_zone.change) VALUES (7);
INSERT INTO time_zone (time_zone.change) VALUES (8);
INSERT INTO time_zone (time_zone.change) VALUES (9);
INSERT INTO time_zone (time_zone.change) VALUES (10);
INSERT INTO time_zone (time_zone.change) VALUES (11);
INSERT INTO time_zone (time_zone.change) VALUES (12);


/* All supported transaction statuses */
INSERT INTO cisdb.status (status.name) VALUES ('Open');
INSERT INTO cisdb.status (status.name) VALUES ('Closed');
INSERT INTO cisdb.status (status.name) VALUES ('Liquidated');


/* All supported leverage ratios/multipliers */
INSERT INTO cisdb.leverage_ratio (leverage_ratio.multiplier) VALUES (1);
INSERT INTO cisdb.leverage_ratio (leverage_ratio.multiplier) VALUES (2);
INSERT INTO cisdb.leverage_ratio (leverage_ratio.multiplier) VALUES (5);
INSERT INTO cisdb.leverage_ratio (leverage_ratio.multiplier) VALUES (10);


/* All supported cryptocurrencies */
INSERT INTO cisdb.crypto_symbol (crypto_symbol.symbol) VALUES ('BTC');
INSERT INTO cisdb.crypto_symbol (crypto_symbol.symbol) VALUES ('ETH');
INSERT INTO cisdb.crypto_symbol (crypto_symbol.symbol) VALUES ('ADA');
INSERT INTO cisdb.crypto_symbol (crypto_symbol.symbol) VALUES ('ATOM');
INSERT INTO cisdb.crypto_symbol (crypto_symbol.symbol) VALUES ('DOGE');


/* All supported fiat currencies */
INSERT INTO cisdb.fiat_symbol (fiat_symbol.symbol) VALUES ('EUR');
