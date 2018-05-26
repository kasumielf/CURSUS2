-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: devdb
-- ------------------------------------------------------
-- Server version	5.7.19-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `tbl_user_data`
--

DROP TABLE IF EXISTS `tbl_user_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tbl_user_data` (
  `uuid` int(11) NOT NULL AUTO_INCREMENT,
  `id` varchar(12) NOT NULL,
  `password` varchar(12) NOT NULL,
  `username` varchar(20) NOT NULL,
  `birthday` timestamp NULL DEFAULT '0000-00-00 00:00:00',
  `last_login` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `create_time` timestamp NULL DEFAULT '0000-00-00 00:00:00',
  `weight` decimal(5,2) DEFAULT NULL,
  `gender` int(11) DEFAULT '0',
  `current_map` int(11) DEFAULT NULL,
  `x` float NOT NULL DEFAULT '0.62',
  `y` float NOT NULL DEFAULT '1.61',
  PRIMARY KEY (`uuid`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_user_data`
--

LOCK TABLES `tbl_user_data` WRITE;
/*!40000 ALTER TABLE `tbl_user_data` DISABLE KEYS */;
INSERT INTO `tbl_user_data` VALUES (1,'kasumielf','1','asd','1986-08-27 15:00:00','2017-08-15 19:28:36','2017-03-12 02:26:00',76.10,0,0,2.9,1.63),(2,'t1','1','test1','1986-08-27 15:00:00','2017-08-15 20:13:13','2017-03-12 02:26:00',80.00,0,0,-25.4343,167.607),(3,'t2','1','test2','1986-08-27 15:00:00','2017-08-15 20:13:24','0000-00-00 00:00:00',81.00,0,0,-27.8098,168.602);
/*!40000 ALTER TABLE `tbl_user_data` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-11-19 12:21:47
