/*M!999999\- enable the sandbox mode */ 
-- MariaDB dump 10.19-11.7.2-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: metadata
-- ------------------------------------------------------
-- Server version	11.4.6-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*M!100616 SET @OLD_NOTE_VERBOSITY=@@NOTE_VERBOSITY, NOTE_VERBOSITY=0 */;

--
-- Table structure for table `AppUsers`
--

DROP TABLE IF EXISTS `AppUsers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `AppUsers` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` longtext NOT NULL,
  `PasswordHash` longblob NOT NULL,
  `PasswordSalt` longblob NOT NULL,
  `Role` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AppUsers`
--

LOCK TABLES `AppUsers` WRITE;
/*!40000 ALTER TABLE `AppUsers` DISABLE KEYS */;
INSERT INTO `AppUsers` VALUES
(1,'admin','E‡\":£šÕÄoñâÚbˆÍÐJIü?“d	SÑ‹pÄ|Õ­[ÃÝTXüKh]mãÇµéM;(¡JFªO','÷±À¡\ZéK”2à‚»X4éìYx‹Yw\n’ÓºR÷gw0üúÁ¾Ú\"ILÖñ„H5¼\Z+{	Â|‰ýëÍKN%Ï…Ss/=6´›¤wtŽ-19Öj|bfñw{zþ÷&îÕ@7\nxžA2¡©>FýŸ—nBðßä',1);
/*!40000 ALTER TABLE `AppUsers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `DataStores`
--

DROP TABLE IF EXISTS `DataStores`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `DataStores` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Data` longtext NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `sourceType` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `DataStores`
--

LOCK TABLES `DataStores` WRITE;
/*!40000 ALTER TABLE `DataStores` DISABLE KEYS */;
INSERT INTO `DataStores` VALUES
(3,'\"string\"','2025-07-05 14:39:12',1),
(4,'\"eeee\"','2025-07-05 14:39:18',1),
(5,'{\n  \"id\": 1,\n  \"name\": \"Test Item\",\n  \"description\": \"This is a test object\",\n  \"createdBy\": \"admin\",\n  \"createdAt\": \"2025-07-04T10:30:00Z\",\n  \"tags\": [\"example\", \"test\", \"json\"],\n  \"isActive\": true,\n  \"metadata\": {\n    \"source\": \"unit-test\",\n    \"importance\": \"high\"\n  }\n}','2025-07-05 14:39:30',1);
/*!40000 ALTER TABLE `DataStores` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES
('20250704105538_InitialCreate','8.0.17'),
('20250704113459_AddUserRoleToAppUser','8.0.17'),
('20250704181405_InitDataStore','8.0.17'),
('20250705045937_AddSrcType','8.0.17');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'metadata'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*M!100616 SET NOTE_VERBOSITY=@OLD_NOTE_VERBOSITY */;

-- Dump completed on 2025-07-05 17:42:15
