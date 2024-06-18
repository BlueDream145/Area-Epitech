-- --------------------------------------------------------
-- Hôte :                        127.0.0.1
-- Version du serveur:           5.7.26 - MySQL Community Server (GPL)
-- SE du serveur:                Win64
-- HeidiSQL Version:             10.2.0.5599
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Listage de la structure de la base pour area
CREATE DATABASE IF NOT EXISTS `area` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `area`;

-- Listage de la structure de la table area. accounts
CREATE TABLE IF NOT EXISTS `accounts` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Service` int(11) NOT NULL,
  `Username` text NOT NULL,
  `Password` text NOT NULL,
  `OwnerId` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=24 DEFAULT CHARSET=latin1;

-- Listage des données de la table area.accounts : 17 rows
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
INSERT INTO `accounts` (`ID`, `Service`, `Username`, `Password`, `OwnerId`) VALUES
	(9, 3, '', '', 0),
	(8, 2, '', '', 0);
/*!40000 ALTER TABLE `accounts` ENABLE KEYS */;

-- Listage de la structure de la table area. actions
CREATE TABLE IF NOT EXISTS `actions` (
  `ID` int(11) NOT NULL,
  `Name` text,
  `Description` text,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- Listage des données de la table area.actions : 17 rows
/*!40000 ALTER TABLE `actions` DISABLE KEYS */;
INSERT INTO `actions` (`ID`, `Name`, `Description`) VALUES
	(0, 'Test', 'This is an action test.'),
	(1, 'Search in gallery', 'Search in gallery'),
	(8, 'Get favorites image', 'Get favorites image'),
	(2, 'Get currencies values', 'Get currencies values'),
	(3, 'Get specific currency value', 'Get specific currency value'),
	(4, 'Get news by tag', 'Get news by tag.'),
	(5, 'Get news', 'Get latest news'),
	(6, 'Get weather by location', 'Get weather by location.'),
	(7, 'Get local weather', 'Get local weather.'),
	(9, 'Get account images', 'Get account images'),
	(10, 'Check domain informations', 'Check domain informations'),
	(11, 'Get friends.', 'Get friends.'),
	(12, 'Send e-mail.', 'Send e-mail.'),
	(13, 'Get videos by tag.', 'Get videos by tag.'),
	(14, 'Get videos.', 'Get videos.'),
	(15, 'Get all pastes', 'Get all pastes.'),
	(16, 'Create paste', 'Create a paste.');
/*!40000 ALTER TABLE `actions` ENABLE KEYS */;

-- Listage de la structure de la table area. reactions
CREATE TABLE IF NOT EXISTS `reactions` (
  `ID` int(11) NOT NULL,
  `Name` text NOT NULL,
  `Description` text NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- Listage des données de la table area.reactions : 11 rows
/*!40000 ALTER TABLE `reactions` DISABLE KEYS */;
INSERT INTO `reactions` (`ID`, `Name`, `Description`) VALUES
	(0, 'Test', 'This is a reaction test.'),
	(1, 'NewsUpdate', 'News update.'),
	(2, 'WeatherUpdate', 'Weather updater.'),
	(3, 'CurrencyUpdate', 'Currency Update.'),
	(4, 'ImgurUpdate', 'Imgur update.'),
	(5, 'DomainUpdate', 'Domaine update.'),
	(6, 'SteamUpdate', 'Steam Update.'),
	(7, 'MailSent', 'Mail sent.'),
	(8, 'VideosUpdate', 'Videos Update.'),
	(9, 'PastesUpdate', 'Pastes Update.'),
	(10, 'PasteCreated', 'Paste created.');
/*!40000 ALTER TABLE `reactions` ENABLE KEYS */;

-- Listage de la structure de la table area. services
CREATE TABLE IF NOT EXISTS `services` (
  `ID` int(11) NOT NULL,
  `Name` text NOT NULL,
  `Description` text NOT NULL,
  `RegisteredUsers` text,
  `Actions` text,
  `Reactions` text,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- Listage des données de la table area.services : 10 rows
/*!40000 ALTER TABLE `services` DISABLE KEYS */;
INSERT INTO `services` (`ID`, `Name`, `Description`, `RegisteredUsers`, `Actions`, `Reactions`) VALUES
	(0, 'Test', 'Ceci est un service de test.', '0;', '0;', '0;'),
	(1, 'Imgur', 'Manager your pictures and feedline.', '0;', '1;8;9;', '4;'),
	(2, 'Currency', 'Currency values everytime.', '0;', '2;3;', '3;'),
	(3, 'News', 'Get latest news?', '0;', '4;5;', '1;'),
	(4, 'Weather', 'The weather all over the world.', '0;', '6;7;', '2;'),
	(5, 'WhoIs', 'Check the status of a domain', '0;', '10;', '5;'),
	(6, 'Steam', 'Gérer vos activités Steam.', '0;', '11;', '6;'),
	(7, 'Gmail', 'Manager your Gmail account.', '0;', '12;', '7;'),
	(8, 'Dailymotion', 'Search for videos.', '0;', '13;14;', '8;'),
	(9, 'Pastebin', 'Manager your pastes !', '0;', '15;16;', '9;10;');
/*!40000 ALTER TABLE `services` ENABLE KEYS */;

-- Listage de la structure de la table area. users
CREATE TABLE IF NOT EXISTS `users` (
  `ID` int(11) NOT NULL,
  `Username` text NOT NULL,
  `Name` text NOT NULL,
  `Mail` text NOT NULL,
  `Password` text NOT NULL,
  `Token` text,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

-- Listage des données de la table area.users : 1 rows
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`ID`, `Username`, `Name`, `Mail`, `Password`, `Token`) VALUES
	(0, 'testaa', 'testt', 'test@bobo.a', 'test', 'BN3YEBLOUSNF');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
