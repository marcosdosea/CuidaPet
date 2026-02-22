-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: cuidapetdb
-- ------------------------------------------------------
-- Server version	8.0.45

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20260214204525_alteracao_identity','8.0.18');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `agendamento`
--

DROP TABLE IF EXISTS `agendamento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `agendamento` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `dataSolicitacao` date NOT NULL,
  `dataConfirmacao` date DEFAULT NULL,
  `horario` time NOT NULL,
  `status` enum('S','A','C','R') NOT NULL,
  `idPet` int unsigned NOT NULL,
  `idFuncionario` int unsigned NOT NULL,
  `idTutor` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Agendamento_Pessoa1_idx` (`idTutor`),
  KEY `idPet` (`idPet`),
  KEY `idVeterinario` (`idFuncionario`),
  CONSTRAINT `agendamento_ibfk_1` FOREIGN KEY (`idPet`) REFERENCES `pet` (`id`),
  CONSTRAINT `agendamento_ibfk_2` FOREIGN KEY (`idFuncionario`) REFERENCES `funcionario` (`id`),
  CONSTRAINT `fk_Agendamento_Pessoa1` FOREIGN KEY (`idTutor`) REFERENCES `pessoa` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `agendamento`
--

LOCK TABLES `agendamento` WRITE;
/*!40000 ALTER TABLE `agendamento` DISABLE KEYS */;
INSERT INTO `agendamento` VALUES (1,'2026-02-20','2026-02-21','08:00:00','A',1,2,7),(2,'2026-02-20','2026-02-21','13:00:00','A',2,3,8),(3,'2026-02-21',NULL,'08:00:00','S',3,2,7),(4,'2026-02-21',NULL,'13:00:00','S',4,3,8);
/*!40000 ALTER TABLE `agendamento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetRoleClaims`
--

DROP TABLE IF EXISTS `AspNetRoleClaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetRoleClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetRoleClaims`
--

LOCK TABLES `AspNetRoleClaims` WRITE;
/*!40000 ALTER TABLE `AspNetRoleClaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetRoleClaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetRoles`
--

DROP TABLE IF EXISTS `AspNetRoles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetRoles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetRoles`
--

LOCK TABLES `AspNetRoles` WRITE;
/*!40000 ALTER TABLE `AspNetRoles` DISABLE KEYS */;
INSERT INTO `AspNetRoles` VALUES ('1c5c272c-0ab9-483a-8f56-f939a173d197','Gerente','GERENTE',NULL),('52718f75-bbe1-49f6-a31c-b9d6bcd6595f','Veterinário','VETERINÁRIO',NULL),('59fad09c-c32b-4850-b538-1edb560cd820','Atendente','ATENDENTE',NULL),('c6dfe35f-29a9-4ae0-857e-f44fd997f5fc','Tutor','TUTOR',NULL),('e7ca9cf8-4d58-4c23-97d6-bf8bebdc24c8','Administrador','ADMINISTRADOR',NULL);
/*!40000 ALTER TABLE `AspNetRoles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserClaims`
--

DROP TABLE IF EXISTS `AspNetUserClaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserClaims`
--

LOCK TABLES `AspNetUserClaims` WRITE;
/*!40000 ALTER TABLE `AspNetUserClaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetUserClaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserLogins`
--

DROP TABLE IF EXISTS `AspNetUserLogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserLogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `ProviderDisplayName` longtext,
  `UserId` varchar(255) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserLogins`
--

LOCK TABLES `AspNetUserLogins` WRITE;
/*!40000 ALTER TABLE `AspNetUserLogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetUserLogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserRoles`
--

DROP TABLE IF EXISTS `AspNetUserRoles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserRoles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserRoles`
--

LOCK TABLES `AspNetUserRoles` WRITE;
/*!40000 ALTER TABLE `AspNetUserRoles` DISABLE KEYS */;
INSERT INTO `AspNetUserRoles` VALUES ('aaaaaaaa-0000-0000-0000-000000000002','1c5c272c-0ab9-483a-8f56-f939a173d197'),('aaaaaaaa-0000-0000-0000-000000000003','52718f75-bbe1-49f6-a31c-b9d6bcd6595f'),('aaaaaaaa-0000-0000-0000-000000000004','52718f75-bbe1-49f6-a31c-b9d6bcd6595f'),('aaaaaaaa-0000-0000-0000-000000000005','59fad09c-c32b-4850-b538-1edb560cd820'),('31a3d081-c5b8-429e-bc89-635360cc033e','c6dfe35f-29a9-4ae0-857e-f44fd997f5fc'),('aaaaaaaa-0000-0000-0000-000000000006','c6dfe35f-29a9-4ae0-857e-f44fd997f5fc'),('aaaaaaaa-0000-0000-0000-000000000007','c6dfe35f-29a9-4ae0-857e-f44fd997f5fc'),('aaaaaaaa-0000-0000-0000-000000000001','e7ca9cf8-4d58-4c23-97d6-bf8bebdc24c8');
/*!40000 ALTER TABLE `AspNetUserRoles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUsers`
--

DROP TABLE IF EXISTS `AspNetUsers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUsers` (
  `Id` varchar(255) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `ConcurrencyStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUsers`
--

LOCK TABLES `AspNetUsers` WRITE;
/*!40000 ALTER TABLE `AspNetUsers` DISABLE KEYS */;
INSERT INTO `AspNetUsers` VALUES ('31a3d081-c5b8-429e-bc89-635360cc033e','douglas','DOUGLAS','douglas@gmail.com','DOUGLAS@GMAIL.COM',0,'AQAAAAIAAYagAAAAELsG8zeeBXYPKRDjNTZqdt5ePnDhEg7rxVv0m4hERiOashewkSbSTMHSaZcQ1rPExw==','NKHVVNUZ5ZIFELZ3ULA3CEEL2UM6TPZJ','f4b38ce1-e200-4a68-ab70-dfaa2740d2c5','19934966072',0,0,NULL,1,0),('aaaaaaaa-0000-0000-0000-000000000001','admin','ADMIN','admin@cuidapet.com','ADMIN@CUIDAPET.COM',0,'AQAAAAIAAYagAAAAELsG8zeeBXYPKRDjNTZqdt5ePnDhEg7rxVv0m4hERiOashewkSbSTMHSaZcQ1rPExw==','SECURITYSTAMP000000000000000001','concurrency-0000-0000-0000-000000000001','61999000001',0,0,NULL,1,0),('aaaaaaaa-0000-0000-0000-000000000002','gerente','GERENTE','gerente@cuidapet.com','GERENTE@CUIDAPET.COM',0,'AQAAAAIAAYagAAAAELsG8zeeBXYPKRDjNTZqdt5ePnDhEg7rxVv0m4hERiOashewkSbSTMHSaZcQ1rPExw==','SECURITYSTAMP000000000000000002','concurrency-0000-0000-0000-000000000002','61999000002',0,0,NULL,1,0),('aaaaaaaa-0000-0000-0000-000000000003','vet_carlos','VET_CARLOS','carlos@cuidapet.com','CARLOS@CUIDAPET.COM',0,'AQAAAAIAAYagAAAAELsG8zeeBXYPKRDjNTZqdt5ePnDhEg7rxVv0m4hERiOashewkSbSTMHSaZcQ1rPExw==','SECURITYSTAMP000000000000000003','concurrency-0000-0000-0000-000000000003','61999000003',0,0,NULL,1,0),('aaaaaaaa-0000-0000-0000-000000000004','vet_ana','VET_ANA','ana@cuidapet.com','ANA@CUIDAPET.COM',0,'AQAAAAIAAYagAAAAELsG8zeeBXYPKRDjNTZqdt5ePnDhEg7rxVv0m4hERiOashewkSbSTMHSaZcQ1rPExw==','SECURITYSTAMP000000000000000004','concurrency-0000-0000-0000-000000000004','61999000004',0,0,NULL,1,0),('aaaaaaaa-0000-0000-0000-000000000005','atendente','ATENDENTE','atendente@cuidapet.com','ATENDENTE@CUIDAPET.COM',0,'AQAAAAIAAYagAAAAELsG8zeeBXYPKRDjNTZqdt5ePnDhEg7rxVv0m4hERiOashewkSbSTMHSaZcQ1rPExw==','SECURITYSTAMP000000000000000005','concurrency-0000-0000-0000-000000000005','61999000005',0,0,NULL,1,0),('aaaaaaaa-0000-0000-0000-000000000006','tutor_joao','TUTOR_JOAO','joao@gmail.com','JOAO@GMAIL.COM',0,'AQAAAAIAAYagAAAAELsG8zeeBXYPKRDjNTZqdt5ePnDhEg7rxVv0m4hERiOashewkSbSTMHSaZcQ1rPExw==','SECURITYSTAMP000000000000000006','concurrency-0000-0000-0000-000000000006','61999000006',0,0,NULL,1,0),('aaaaaaaa-0000-0000-0000-000000000007','tutor_maria','TUTOR_MARIA','maria@gmail.com','MARIA@GMAIL.COM',0,'AQAAAAIAAYagAAAAELsG8zeeBXYPKRDjNTZqdt5ePnDhEg7rxVv0m4hERiOashewkSbSTMHSaZcQ1rPExw==','SECURITYSTAMP000000000000000007','concurrency-0000-0000-0000-000000000007','61999000007',0,0,NULL,1,0);
/*!40000 ALTER TABLE `AspNetUsers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `AspNetUserTokens`
--

DROP TABLE IF EXISTS `AspNetUserTokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AspNetUserTokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(128) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AspNetUserTokens`
--

LOCK TABLES `AspNetUserTokens` WRITE;
/*!40000 ALTER TABLE `AspNetUserTokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `AspNetUserTokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categoria`
--

DROP TABLE IF EXISTS `categoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categoria` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(30) NOT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categoria`
--

LOCK TABLES `categoria` WRITE;
/*!40000 ALTER TABLE `categoria` DISABLE KEYS */;
INSERT INTO `categoria` VALUES (1,'Medicamentos','Rem├®dios e suplementos veterin├írios'),(2,'Acessórios','Coleiras, brinquedos e camas'),(3,'Alimentos','Ra├º├Áes e petiscos'),(4,'Higiene','Shampoos, escovas e perfumes');
/*!40000 ALTER TABLE `categoria` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `consulta`
--

DROP TABLE IF EXISTS `consulta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `consulta` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `dataConsulta` datetime NOT NULL,
  `anotacoes` varchar(512) DEFAULT NULL,
  `idTutor` int unsigned NOT NULL,
  `idPet` int unsigned NOT NULL,
  `idFuncionario` int unsigned NOT NULL,
  `idAgendamento` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Consulta_Agendamento1_idx` (`idAgendamento`),
  KEY `fk_Consulta_Funcionario2_idx` (`idFuncionario`),
  KEY `fk_Consulta_Pessoa1_idx` (`idTutor`),
  KEY `fk_Consulta_Pet1_idx` (`idPet`),
  CONSTRAINT `fk_Consulta_Agendamento1` FOREIGN KEY (`idAgendamento`) REFERENCES `agendamento` (`id`),
  CONSTRAINT `fk_Consulta_Funcionario2` FOREIGN KEY (`idFuncionario`) REFERENCES `funcionario` (`id`),
  CONSTRAINT `fk_Consulta_Pessoa1` FOREIGN KEY (`idTutor`) REFERENCES `pessoa` (`id`),
  CONSTRAINT `fk_Consulta_Pet1` FOREIGN KEY (`idPet`) REFERENCES `pet` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `consulta`
--

LOCK TABLES `consulta` WRITE;
/*!40000 ALTER TABLE `consulta` DISABLE KEYS */;
INSERT INTO `consulta` VALUES (1,'2026-02-21 08:30:00','Pet apresenta bom estado geral. Vacina├º├úo em dia.',7,1,2,1),(2,'2026-02-21 13:30:00','Gata com leve irrita├º├úo na pele. Prescrito shampoo dermatol├│gico.',8,2,3,2);
/*!40000 ALTER TABLE `consulta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `doenca`
--

DROP TABLE IF EXISTS `doenca`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `doenca` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(30) NOT NULL,
  `idEspecie` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idEspecie` (`idEspecie`),
  CONSTRAINT `doenca_ibfk_1` FOREIGN KEY (`idEspecie`) REFERENCES `especie` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doenca`
--

LOCK TABLES `doenca` WRITE;
/*!40000 ALTER TABLE `doenca` DISABLE KEYS */;
INSERT INTO `doenca` VALUES (1,'Cinomose',1),(2,'Parvovirose',1),(3,'Raiva',1),(4,'Leucemia Felina',2),(5,'Raiva (Felina)',2);
/*!40000 ALTER TABLE `doenca` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `especialidade`
--

DROP TABLE IF EXISTS `especialidade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `especialidade` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(30) NOT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `especialidade`
--

LOCK TABLES `especialidade` WRITE;
/*!40000 ALTER TABLE `especialidade` DISABLE KEYS */;
INSERT INTO `especialidade` VALUES (1,'Clínica Geral','Atendimento clínco geral para pets'),(2,'Dermatologia','Cuidados com pele, pelo e unhas'),(3,'Ortopedia','Ossos, articulações e reabilitação'),(4,'Oncologia','Diagnóstico e tratamento de tumores');
/*!40000 ALTER TABLE `especialidade` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `especie`
--

DROP TABLE IF EXISTS `especie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `especie` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `especie`
--

LOCK TABLES `especie` WRITE;
/*!40000 ALTER TABLE `especie` DISABLE KEYS */;
INSERT INTO `especie` VALUES (1,'Cão'),(2,'Gato'),(3,'Coelho');
/*!40000 ALTER TABLE `especie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `estabelecimento`
--

DROP TABLE IF EXISTS `estabelecimento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `estabelecimento` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(30) NOT NULL,
  `tipo` enum('C','P','A') DEFAULT NULL,
  `CNPJ` char(14) NOT NULL,
  `telefone` char(11) NOT NULL,
  `logradouro` varchar(100) NOT NULL,
  `numero` varchar(10) NOT NULL,
  `complemento` varchar(50) DEFAULT NULL,
  `bairro` varchar(50) NOT NULL,
  `cidade` varchar(100) NOT NULL,
  `estado` char(2) NOT NULL,
  `idGerente` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `CNPJ` (`CNPJ`),
  KEY `fk_Estabelecimento_Pessoa1_idx` (`idGerente`),
  CONSTRAINT `fk_Estabelecimento_Pessoa1` FOREIGN KEY (`idGerente`) REFERENCES `pessoa` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `estabelecimento`
--

LOCK TABLES `estabelecimento` WRITE;
/*!40000 ALTER TABLE `estabelecimento` DISABLE KEYS */;
INSERT INTO `estabelecimento` VALUES (1,'CuidaPet Clínica Veterinária','C','12345678000195','61933001122','SHCS EQ 106/107','1','Loja 2','Cruzeiro','Brasília','DF',3);
/*!40000 ALTER TABLE `estabelecimento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `funcionario`
--

DROP TABLE IF EXISTS `funcionario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `funcionario` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `crmv` varchar(7) DEFAULT NULL,
  `idPessoa` int unsigned NOT NULL,
  `idEstabelecimento` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `idUsuario` (`idPessoa`),
  UNIQUE KEY `crmv` (`crmv`),
  KEY `fk_Funcionario_Estabelecimento1_idx` (`idEstabelecimento`),
  CONSTRAINT `fk_Funcionario_Estabelecimento1` FOREIGN KEY (`idEstabelecimento`) REFERENCES `estabelecimento` (`id`),
  CONSTRAINT `veterinario_ibfk_2` FOREIGN KEY (`idPessoa`) REFERENCES `pessoa` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `funcionario`
--

LOCK TABLES `funcionario` WRITE;
/*!40000 ALTER TABLE `funcionario` DISABLE KEYS */;
INSERT INTO `funcionario` VALUES (1,NULL,3,1),(2,'DF12345',4,1),(3,'DF67890',5,1),(4,NULL,6,1);
/*!40000 ALTER TABLE `funcionario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `funcionarioespecialidade`
--

DROP TABLE IF EXISTS `funcionarioespecialidade`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `funcionarioespecialidade` (
  `idFuncionario` int unsigned NOT NULL,
  `idEspecialidade` int unsigned NOT NULL,
  PRIMARY KEY (`idFuncionario`,`idEspecialidade`),
  KEY `fk_Funcionario_has_Especialidade_Especialidade1_idx` (`idEspecialidade`),
  KEY `fk_Funcionario_has_Especialidade_Funcionario1_idx` (`idFuncionario`),
  CONSTRAINT `fk_Funcionario_has_Especialidade_Especialidade1` FOREIGN KEY (`idEspecialidade`) REFERENCES `especialidade` (`id`),
  CONSTRAINT `fk_Funcionario_has_Especialidade_Funcionario1` FOREIGN KEY (`idFuncionario`) REFERENCES `funcionario` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `funcionarioespecialidade`
--

LOCK TABLES `funcionarioespecialidade` WRITE;
/*!40000 ALTER TABLE `funcionarioespecialidade` DISABLE KEYS */;
INSERT INTO `funcionarioespecialidade` VALUES (2,1),(3,1),(2,2),(3,3);
/*!40000 ALTER TABLE `funcionarioespecialidade` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `horariosatendimento`
--

DROP TABLE IF EXISTS `horariosatendimento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `horariosatendimento` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `diaSemana` enum('DOM','SEG','TER','QUA','QUI','SEX','SAB') NOT NULL,
  `horario` time NOT NULL,
  `idFuncionario` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idVeterinario1` (`idFuncionario`),
  CONSTRAINT `horariosatendimento_ibfk_1` FOREIGN KEY (`idFuncionario`) REFERENCES `funcionario` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `horariosatendimento`
--

LOCK TABLES `horariosatendimento` WRITE;
/*!40000 ALTER TABLE `horariosatendimento` DISABLE KEYS */;
INSERT INTO `horariosatendimento` VALUES (1,'SEG','08:00:00',2),(2,'TER','08:00:00',2),(3,'QUA','08:00:00',2),(4,'QUI','08:00:00',2),(5,'SEX','08:00:00',2),(6,'SEG','13:00:00',3),(7,'TER','13:00:00',3),(8,'QUA','13:00:00',3),(9,'QUI','13:00:00',3),(10,'SEX','13:00:00',3);
/*!40000 ALTER TABLE `horariosatendimento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notificacao`
--

DROP TABLE IF EXISTS `notificacao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notificacao` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `titulo` varchar(45) NOT NULL,
  `descricao` varchar(150) DEFAULT NULL,
  `dataEnvio` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notificacao`
--

LOCK TABLES `notificacao` WRITE;
/*!40000 ALTER TABLE `notificacao` DISABLE KEYS */;
INSERT INTO `notificacao` VALUES (1,'Lembrete de Vacina','Seu pet Rex est├í com a vacina V10 a vencer.','2026-02-21 08:00:00'),(2,'Consulta Agendada','Consulta confirmada para amanh├ú ├ás 08:00.','2026-02-21 09:00:00'),(3,'Promo├º├úo na Loja','Ra├º├úo Premium com 20% de desconto esta semana!','2026-02-21 10:00:00'),(4,'Novo cadastro pendente','Um novo estabelecimento aguarda aprova├º├úo no sistema.','2026-02-21 08:00:00'),(5,'Relat├│rio mensal','O relat├│rio de fevereiro est├í dispon├¡vel para download.','2026-02-21 09:00:00'),(6,'Funcion├írio adicionado','Novo veterin├írio cadastrado no estabelecimento.','2026-02-21 07:30:00'),(7,'Meta de atendimento','Meta de 50 consultas no m├¬s atingida. Parab├®ns ├á equipe!','2026-02-21 10:00:00'),(8,'Estoque baixo','Produto \"Ra├º├úo Premium\" com estoque abaixo do m├¡nimo.','2026-02-21 11:00:00'),(9,'Agenda do dia','Voc├¬ tem 3 consultas agendadas para hoje.','2026-02-21 07:00:00'),(10,'Prontu├írio atualizado','O prontu├írio do pet Rex foi atualizado com sucesso.','2026-02-21 09:30:00'),(11,'Lembrete de consulta','Consulta com Luna ├ás 13:00 em 15 minutos.','2026-02-21 12:45:00'),(12,'Novo agendamento','Agendamento solicitado por Jo├úo para o pet Bolt.','2026-02-21 08:15:00'),(13,'Agendamento cancelado','Tutor Maria cancelou o agendamento de amanh├ú.','2026-02-21 10:30:00'),(14,'Pedido finalizado','Pedido #1 do tutor Jo├úo foi finalizado com sucesso.','2026-02-21 09:05:00'),(15,'Vacina próxima do vencimento','A vacina V10 do Rex vence em 30 dias. Agende agora!','2026-02-21 08:00:00'),(16,'Consulta confirmada','Sua consulta de amanh├ú ├ás 08:00 foi confirmada.','2026-02-21 09:00:00'),(17,'Pedido conclu├¡do','Seu pedido foi processado. Retire no estabelecimento.','2026-02-21 09:10:00'),(18,'Promoção exclusiva','Coleira Antipulgas com 20% de desconto só hoje!','2026-02-21 10:00:00'),(19,'Resultado de exame','O resultado do exame da Mia est├í dispon├¡vel.','2026-02-21 11:00:00'),(20,'Bem-vindo ao CuidaPet','Ol├í Douglas! Seu cadastro foi realizado com sucesso.','2026-02-14 12:00:00');
/*!40000 ALTER TABLE `notificacao` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pedido`
--

DROP TABLE IF EXISTS `pedido`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pedido` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `status` enum('A','F','C') NOT NULL,
  `realizadoEm` datetime NOT NULL,
  `idTutor` int unsigned NOT NULL,
  `idFuncionario` int unsigned NOT NULL,
  `idAgendamento` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Pedido_Agendamento1_idx` (`idAgendamento`),
  KEY `fk_Pedido_Funcionario1_idx` (`idFuncionario`),
  KEY `idUsuario1` (`idTutor`),
  CONSTRAINT `fk_Pedido_Agendamento1` FOREIGN KEY (`idAgendamento`) REFERENCES `agendamento` (`id`),
  CONSTRAINT `fk_Pedido_Funcionario1` FOREIGN KEY (`idFuncionario`) REFERENCES `funcionario` (`id`),
  CONSTRAINT `pedido_ibfk_1` FOREIGN KEY (`idTutor`) REFERENCES `pessoa` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedido`
--

LOCK TABLES `pedido` WRITE;
/*!40000 ALTER TABLE `pedido` DISABLE KEYS */;
INSERT INTO `pedido` VALUES (1,'A','2026-02-21 09:00:00',7,2,1),(2,'F','2026-02-21 14:00:00',8,3,2);
/*!40000 ALTER TABLE `pedido` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pedidoproduto`
--

DROP TABLE IF EXISTS `pedidoproduto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pedidoproduto` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `quantidade` int NOT NULL,
  `preco` decimal(10,2) NOT NULL,
  `idProduto` int unsigned NOT NULL,
  `idPedido` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idPedido` (`idPedido`),
  KEY `idProduto` (`idProduto`),
  CONSTRAINT `produtopedido_ibfk_1` FOREIGN KEY (`idProduto`) REFERENCES `produto` (`id`),
  CONSTRAINT `produtopedido_ibfk_2` FOREIGN KEY (`idPedido`) REFERENCES `pedido` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedidoproduto`
--

LOCK TABLES `pedidoproduto` WRITE;
/*!40000 ALTER TABLE `pedidoproduto` DISABLE KEYS */;
INSERT INTO `pedidoproduto` VALUES (1,1,151.92,1,1),(2,1,38.50,2,1),(3,1,49.90,3,2),(4,1,23.90,4,2);
/*!40000 ALTER TABLE `pedidoproduto` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pessoa`
--

DROP TABLE IF EXISTS `pessoa`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pessoa` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `cpf` char(11) NOT NULL,
  `idUsuario` varchar(450) NOT NULL,
  `status` enum('A','I') NOT NULL DEFAULT 'A',
  `logradouro` varchar(100) NOT NULL,
  `numero` varchar(10) NOT NULL,
  `complemento` varchar(50) DEFAULT NULL,
  `bairro` varchar(50) NOT NULL,
  `cidade` varchar(100) NOT NULL,
  `estado` char(2) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `cpf_UNIQUE` (`cpf`),
  UNIQUE KEY `idUsuario_UNIQUE` (`idUsuario`),
  CONSTRAINT `fk_pessoa_usuario` FOREIGN KEY (`idUsuario`) REFERENCES `AspNetUsers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pessoa`
--

LOCK TABLES `pessoa` WRITE;
/*!40000 ALTER TABLE `pessoa` DISABLE KEYS */;
INSERT INTO `pessoa` VALUES (1,'88339233041','31a3d081-c5b8-429e-bc89-635360cc033e','A','Quadra SHCES Quadra 1405 Bloco C','842',NULL,'Cruzeiro Novo','Brasília','DF'),(2,'44049280094','aaaaaaaa-0000-0000-0000-000000000001','A','Setor Comercial Sul Quadra 2','100','Bloco A','Asa Sul','Brasília','DF'),(3,'77008957084','aaaaaaaa-0000-0000-0000-000000000002','A','Av. W3 Norte','2000',NULL,'Asa Norte','Brasília','DF'),(4,'24376574015','aaaaaaaa-0000-0000-0000-000000000003','A','SGAS Quadra 913 Bloco B','10','Apto 301','Asa Sul','Brasília','DF'),(5,'67172370074','aaaaaaaa-0000-0000-0000-000000000004','A','CLN 210 Bloco B','22',NULL,'Asa Norte','Brasília','DF'),(6,'47550978000','aaaaaaaa-0000-0000-0000-000000000005','A','SMPW Quadra 25 Conj. 1','5',NULL,'Park Way','Brasília','DF'),(7,'08037024008','aaaaaaaa-0000-0000-0000-000000000006','A','SQN 208 Bloco G','412',NULL,'Asa Norte','Brasília','DF'),(8,'60216736030','aaaaaaaa-0000-0000-0000-000000000007','A','SQS 306 Bloco I','201',NULL,'Asa Sul','Brasília','DF');
/*!40000 ALTER TABLE `pessoa` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pessoanotificacao`
--

DROP TABLE IF EXISTS `pessoanotificacao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pessoanotificacao` (
  `id` int NOT NULL AUTO_INCREMENT,
  `statusLida` tinyint NOT NULL,
  `idPessoa` int unsigned NOT NULL,
  `idNotificacao` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Pessoa_has_Notificacao_Notificacao1_idx` (`idNotificacao`),
  KEY `fk_Pessoa_has_Notificacao_Pessoa1_idx` (`idPessoa`),
  CONSTRAINT `fk_Pessoa_has_Notificacao_Notificacao1` FOREIGN KEY (`idNotificacao`) REFERENCES `notificacao` (`id`),
  CONSTRAINT `fk_Pessoa_has_Notificacao_Pessoa1` FOREIGN KEY (`idPessoa`) REFERENCES `pessoa` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pessoanotificacao`
--

LOCK TABLES `pessoanotificacao` WRITE;
/*!40000 ALTER TABLE `pessoanotificacao` DISABLE KEYS */;
INSERT INTO `pessoanotificacao` VALUES (1,0,7,1),(2,0,7,2),(3,0,8,3),(4,0,2,4),(5,0,2,5),(6,1,2,3),(7,0,3,6),(8,0,3,7),(9,0,3,8),(10,1,3,5),(11,0,4,9),(12,1,4,10),(13,0,4,6),(14,0,5,9),(15,0,5,11),(16,1,5,10),(17,0,6,12),(18,1,6,13),(19,1,6,14),(20,0,7,15),(21,1,7,16),(22,1,7,17),(23,0,7,18),(24,0,8,15),(25,0,8,18),(26,0,8,19),(27,1,1,20),(28,0,1,15),(29,0,1,18);
/*!40000 ALTER TABLE `pessoanotificacao` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pessoapet`
--

DROP TABLE IF EXISTS `pessoapet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pessoapet` (
  `idPet` int unsigned NOT NULL,
  `idPessoa` int unsigned NOT NULL,
  KEY `fk_Pet_has_Pessoa_Pessoa1_idx` (`idPessoa`),
  KEY `fk_Pet_has_Pessoa_Pet1_idx` (`idPet`),
  CONSTRAINT `fk_Pet_has_Pessoa_Pessoa1` FOREIGN KEY (`idPessoa`) REFERENCES `pessoa` (`id`),
  CONSTRAINT `fk_Pet_has_Pessoa_Pet1` FOREIGN KEY (`idPet`) REFERENCES `pet` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pessoapet`
--

LOCK TABLES `pessoapet` WRITE;
/*!40000 ALTER TABLE `pessoapet` DISABLE KEYS */;
INSERT INTO `pessoapet` VALUES (1,7),(3,7),(2,8),(4,8);
/*!40000 ALTER TABLE `pessoapet` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pet`
--

DROP TABLE IF EXISTS `pet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pet` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(20) NOT NULL,
  `sexo` enum('M','F') NOT NULL,
  `dataNascimento` date DEFAULT NULL,
  `idRaca` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Pet_Raca1_idx` (`idRaca`),
  CONSTRAINT `fk_Pet_Raca1` FOREIGN KEY (`idRaca`) REFERENCES `raca` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pet`
--

LOCK TABLES `pet` WRITE;
/*!40000 ALTER TABLE `pet` DISABLE KEYS */;
INSERT INTO `pet` VALUES (1,'Rex','M','2021-03-10',1),(2,'Mia','F','2020-07-22',4),(3,'Bolt','M','2022-11-05',2),(4,'Luna','F','2023-01-15',5);
/*!40000 ALTER TABLE `pet` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `petdoenca`
--

DROP TABLE IF EXISTS `petdoenca`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `petdoenca` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `dataDiagnostico` date DEFAULT NULL,
  `idPet` int unsigned NOT NULL,
  `idDoenca` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idDoenca` (`idDoenca`),
  KEY `idPet1` (`idPet`),
  CONSTRAINT `petdoenca_ibfk_1` FOREIGN KEY (`idPet`) REFERENCES `pet` (`id`),
  CONSTRAINT `petdoenca_ibfk_2` FOREIGN KEY (`idDoenca`) REFERENCES `doenca` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `petdoenca`
--

LOCK TABLES `petdoenca` WRITE;
/*!40000 ALTER TABLE `petdoenca` DISABLE KEYS */;
INSERT INTO `petdoenca` VALUES (1,'2023-05-10',1,1);
/*!40000 ALTER TABLE `petdoenca` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `produto`
--

DROP TABLE IF EXISTS `produto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `produto` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(30) NOT NULL,
  `preco` decimal(10,2) NOT NULL,
  `status` enum('I','D','P') DEFAULT 'D',
  `precoPromocao` decimal(10,2) DEFAULT NULL,
  `descricao` varchar(50) DEFAULT NULL,
  `idCategoria` int unsigned NOT NULL,
  `idEstabelecimento` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_produto_estabelecimento1_idx` (`idEstabelecimento`),
  KEY `idCategoria` (`idCategoria`),
  CONSTRAINT `fk_produto_estabelecimento1` FOREIGN KEY (`idEstabelecimento`) REFERENCES `estabelecimento` (`id`),
  CONSTRAINT `produto_ibfk_1` FOREIGN KEY (`idCategoria`) REFERENCES `categoria` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `produto`
--

LOCK TABLES `produto` WRITE;
/*!40000 ALTER TABLE `produto` DISABLE KEYS */;
INSERT INTO `produto` VALUES (1,'Ração Premium Adulto 15kg',189.90,'D',151.92,'Ração premium para cães adultos',3,1),(2,'Vermífugo Drontal Plus',38.50,'D',NULL,'Comprimido antiparasit├írio',1,1),(3,'Shampoo Dermatológico 500ml',49.90,'D',NULL,'Para peles sens├¡veis',4,1),(4,'Coleira Antipulgas M',29.90,'D',23.90,'Dura├º├úo de at├® 8 meses',2,1),(5,'Petisco Natural Cão 200g',18.00,'D',NULL,'Snack saud├ível livre de conservantes',3,1);
/*!40000 ALTER TABLE `produto` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `raca`
--

DROP TABLE IF EXISTS `raca`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `raca` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(50) NOT NULL,
  `idEspecie` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idEspecie1` (`idEspecie`),
  CONSTRAINT `raca_ibfk_1` FOREIGN KEY (`idEspecie`) REFERENCES `especie` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `raca`
--

LOCK TABLES `raca` WRITE;
/*!40000 ALTER TABLE `raca` DISABLE KEYS */;
INSERT INTO `raca` VALUES (1,'Labrador Retriever',1),(2,'Bulldog Francês',1),(3,'Golden Retriever',1),(4,'Persa',2),(5,'Siamês',2),(6,'Maine Coon',2),(7,'Angorá',3),(8,'Nova Zelândia',3);
/*!40000 ALTER TABLE `raca` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vacina`
--

DROP TABLE IF EXISTS `vacina`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vacina` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(100) NOT NULL,
  `periodoEmDias` smallint unsigned DEFAULT NULL,
  `idDoenca` int unsigned NOT NULL,
  `idEspecie` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Vacina_Especie1_idx` (`idEspecie`),
  KEY `idDoenca1` (`idDoenca`),
  CONSTRAINT `fk_Vacina_Especie1` FOREIGN KEY (`idEspecie`) REFERENCES `especie` (`id`),
  CONSTRAINT `vacina_ibfk_1` FOREIGN KEY (`idDoenca`) REFERENCES `doenca` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vacina`
--

LOCK TABLES `vacina` WRITE;
/*!40000 ALTER TABLE `vacina` DISABLE KEYS */;
INSERT INTO `vacina` VALUES (1,'V10 (Dupla Canina)',365,1,1),(2,'Parvo Monovalente',365,2,1),(3,'Antirábica Canina',365,3,1),(4,'Leucemia Felina (FeLV)',365,4,2),(5,'Antirábica Felina',365,5,2);
/*!40000 ALTER TABLE `vacina` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vacinacao`
--

DROP TABLE IF EXISTS `vacinacao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vacinacao` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `dataVacina` date NOT NULL,
  `lote` varchar(20) DEFAULT NULL,
  `idVacina` int unsigned NOT NULL,
  `idPet` int unsigned NOT NULL,
  `idFuncionario` int unsigned NOT NULL,
  `idTutor` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Vacinacao_Pessoa1_idx` (`idTutor`),
  KEY `fk_VacinaPet_Funcionario1_idx` (`idFuncionario`),
  KEY `idPet2` (`idPet`),
  KEY `idVacina` (`idVacina`),
  CONSTRAINT `fk_Vacinacao_Pessoa1` FOREIGN KEY (`idTutor`) REFERENCES `pessoa` (`id`),
  CONSTRAINT `fk_VacinaPet_Funcionario1` FOREIGN KEY (`idFuncionario`) REFERENCES `funcionario` (`id`),
  CONSTRAINT `vacinapet_ibfk_1` FOREIGN KEY (`idVacina`) REFERENCES `vacina` (`id`),
  CONSTRAINT `vacinapet_ibfk_2` FOREIGN KEY (`idPet`) REFERENCES `pet` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vacinacao`
--

LOCK TABLES `vacinacao` WRITE;
/*!40000 ALTER TABLE `vacinacao` DISABLE KEYS */;
INSERT INTO `vacinacao` VALUES (1,'2026-02-21','LOTE-V10-2026A',1,1,2,7),(2,'2026-02-21','LOTE-ARB-2026A',3,1,2,7),(3,'2026-02-21','LOTE-FEL-2026B',5,2,3,8),(4,'2026-02-21','LOTE-FEL-2026C',4,2,3,8);
/*!40000 ALTER TABLE `vacinacao` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'cuidapetdb'
--

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-02-21 19:48:46
