CREATE DATABASE  IF NOT EXISTS `cuidapetdb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `cuidapetdb`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: cuidapetdb
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

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
  `status` enum('S','A','C','R') COLLATE utf8mb4_unicode_ci DEFAULT 'S' COMMENT 'S (Solicitado), A (Aprovado), C (Cancelado), R (Realizado)',
  `idPet` int unsigned NOT NULL,
  `idFuncionario` int unsigned NOT NULL,
  `idTutor` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idPet` (`idPet`),
  KEY `idVeterinario` (`idFuncionario`),
  KEY `fk_Agendamento_Pessoa1_idx` (`idTutor`),
  CONSTRAINT `agendamento_ibfk_1` FOREIGN KEY (`idPet`) REFERENCES `pet` (`id`),
  CONSTRAINT `agendamento_ibfk_2` FOREIGN KEY (`idFuncionario`) REFERENCES `funcionario` (`id`),
  CONSTRAINT `fk_Agendamento_Pessoa1` FOREIGN KEY (`idTutor`) REFERENCES `pessoa` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `agendamento`
--

LOCK TABLES `agendamento` WRITE;
/*!40000 ALTER TABLE `agendamento` DISABLE KEYS */;
INSERT INTO `agendamento` VALUES (1,'2024-08-20','2024-08-21','08:00:00','A',1,1,9),(2,'2024-08-22','2024-08-22','14:00:00','A',2,1,10),(3,'2024-08-23',NULL,'09:00:00','S',3,2,11),(4,'2024-08-21','2024-08-22','10:00:00','R',4,3,12),(5,'2024-08-19','2024-08-20','15:00:00','R',5,2,9);
/*!40000 ALTER TABLE `agendamento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categoria`
--

DROP TABLE IF EXISTS `categoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categoria` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `nome` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descricao` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categoria`
--

LOCK TABLES `categoria` WRITE;
/*!40000 ALTER TABLE `categoria` DISABLE KEYS */;
INSERT INTO `categoria` VALUES (1,'Ração','Alimentos para pets'),(2,'Medicamentos','Remédios e suplementos'),(3,'Brinquedos','Brinquedos e entretenimento'),(4,'Higiene','Produtos de limpeza e higiene'),(5,'Acessórios','Coleiras, camas, transportadores'),(6,'Petiscos','Snacks e petiscos');
/*!40000 ALTER TABLE `categoria` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `consulta`
--

DROP TABLE IF EXISTS `consulta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `consulta` (
  `id` int unsigned NOT NULL,
  `dataConsulta` datetime NOT NULL,
  `anotacoes` varchar(512) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `idTutor` int unsigned NOT NULL,
  `idPet` int unsigned NOT NULL,
  `idFuncionario` int unsigned NOT NULL,
  `idAgendamento` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Consulta_Pessoa1_idx` (`idTutor`),
  KEY `fk_Consulta_Pet1_idx` (`idPet`),
  KEY `fk_Consulta_Funcionario2_idx` (`idFuncionario`),
  KEY `fk_Consulta_Agendamento1_idx` (`idAgendamento`),
  CONSTRAINT `fk_Consulta_Agendamento1` FOREIGN KEY (`idAgendamento`) REFERENCES `agendamento` (`id`),
  CONSTRAINT `fk_Consulta_Funcionario2` FOREIGN KEY (`idFuncionario`) REFERENCES `funcionario` (`id`),
  CONSTRAINT `fk_Consulta_Pessoa1` FOREIGN KEY (`idTutor`) REFERENCES `pessoa` (`id`),
  CONSTRAINT `fk_Consulta_Pet1` FOREIGN KEY (`idPet`) REFERENCES `pet` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `consulta`
--

LOCK TABLES `consulta` WRITE;
/*!40000 ALTER TABLE `consulta` DISABLE KEYS */;
INSERT INTO `consulta` VALUES (4,'2024-08-22 10:00:00','Consulta de rotina. Pet saudável, recomendado retorno em 6 meses.',12,4,3,4),(5,'2024-08-20 15:00:00','Vacinação V10. Pet apresentou boa reação. Próxima dose em 1 ano.',9,5,2,5);
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
  `nome` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `idEspecie` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idEspecie` (`idEspecie`),
  CONSTRAINT `doenca_ibfk_1` FOREIGN KEY (`idEspecie`) REFERENCES `especie` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doenca`
--

LOCK TABLES `doenca` WRITE;
/*!40000 ALTER TABLE `doenca` DISABLE KEYS */;
INSERT INTO `doenca` VALUES (1,'Parvovirose',1),(2,'Cinomose',1),(3,'Hepatite Canina',1),(4,'Raiva',1),(5,'Leptospirose',1),(6,'Tosse dos Canis',1),(7,'Panleucopenia Felina',2),(8,'Rinotraqueíte',2),(9,'Calicivirose',2),(10,'Raiva',2),(11,'Leucemia Felina',2),(12,'Imunodeficiência Felina',2);
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
  `nome` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descricao` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `especialidade`
--

LOCK TABLES `especialidade` WRITE;
/*!40000 ALTER TABLE `especialidade` DISABLE KEYS */;
INSERT INTO `especialidade` VALUES (1,'Clínica Geral','Atendimento geral para pets'),(2,'Cirurgia','Procedimentos cirúrgicos'),(3,'Dermatologia','Tratamento de pele e pelagem'),(4,'Cardiologia','Especialista em coração'),(5,'Ortopedia','Tratamento de ossos e articulações'),(6,'Oftalmologia','Tratamento dos olhos');
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
  `nome` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `especie`
--

LOCK TABLES `especie` WRITE;
/*!40000 ALTER TABLE `especie` DISABLE KEYS */;
INSERT INTO `especie` VALUES (1,'Canina'),(2,'Felina'),(3,'Ave'),(4,'Roedor'),(5,'Réptil'),(6,'Peixe');
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
  `nome` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `tipo` enum('C','P','A') COLLATE utf8mb4_unicode_ci DEFAULT NULL COMMENT 'C(Clínica), P(Petshop), A(Ambos)',
  `CNPJ` char(14) COLLATE utf8mb4_unicode_ci NOT NULL,
  `telefone` char(11) COLLATE utf8mb4_unicode_ci NOT NULL,
  `logradouro` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `numero` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `complemento` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `bairro` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `cidade` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `estado` char(2) COLLATE utf8mb4_unicode_ci NOT NULL,
  `idGerente` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `CNPJ` (`CNPJ`),
  KEY `fk_Estabelecimento_Pessoa1_idx` (`idGerente`),
  CONSTRAINT `fk_Estabelecimento_Pessoa1` FOREIGN KEY (`idGerente`) REFERENCES `pessoa` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `estabelecimento`
--

LOCK TABLES `estabelecimento` WRITE;
/*!40000 ALTER TABLE `estabelecimento` DISABLE KEYS */;
INSERT INTO `estabelecimento` VALUES (1,'CuidaPet Vila Madalena','A','12345678901234','1130001000','Rua Harmonia','123',NULL,'Vila Madalena','São Paulo','SP',2),(2,'PetShop Jardins','P','23456789012345','1130002000','Rua Oscar Freire','456','Loja 1','Jardins','São Paulo','SP',3),(3,'Clínica Veterinária Pinheiros','C','34567890123456','1130003000','Av. Rebouças','789',NULL,'Pinheiros','São Paulo','SP',2);
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
  `crmv` varchar(7) COLLATE utf8mb4_unicode_ci NOT NULL,
  `cpf` char(11) COLLATE utf8mb4_unicode_ci NOT NULL,
  `idPessoa` int unsigned NOT NULL,
  `idEstabelecimento` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `crmv` (`crmv`),
  UNIQUE KEY `idUsuario` (`idPessoa`),
  UNIQUE KEY `cpf_UNIQUE` (`cpf`),
  KEY `fk_Funcionario_Estabelecimento1_idx` (`idEstabelecimento`),
  CONSTRAINT `fk_Funcionario_Estabelecimento1` FOREIGN KEY (`idEstabelecimento`) REFERENCES `estabelecimento` (`id`),
  CONSTRAINT `veterinario_ibfk_2` FOREIGN KEY (`idPessoa`) REFERENCES `pessoa` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `funcionario`
--

LOCK TABLES `funcionario` WRITE;
/*!40000 ALTER TABLE `funcionario` DISABLE KEYS */;
INSERT INTO `funcionario` VALUES (1,'SP12345','12345678901',4,1),(2,'SP23456','23456789012',5,1),(3,'SP34567','34567890123',6,3),(4,'AT12345','45678901234',7,1),(5,'AT23456','56789012345',8,2);
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `funcionarioespecialidade`
--

LOCK TABLES `funcionarioespecialidade` WRITE;
/*!40000 ALTER TABLE `funcionarioespecialidade` DISABLE KEYS */;
INSERT INTO `funcionarioespecialidade` VALUES (1,1),(2,1),(2,2),(1,3),(3,4),(3,5);
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
  `diaSemana` enum('DOM','SEG','TER','QUA','QUI','SEX','SAB') COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '\n',
  `horario` time NOT NULL,
  `idFuncionario` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idVeterinario` (`idFuncionario`),
  CONSTRAINT `horariosatendimento_ibfk_1` FOREIGN KEY (`idFuncionario`) REFERENCES `funcionario` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `horariosatendimento`
--

LOCK TABLES `horariosatendimento` WRITE;
/*!40000 ALTER TABLE `horariosatendimento` DISABLE KEYS */;
INSERT INTO `horariosatendimento` VALUES (1,'SEG','08:00:00',1),(2,'SEG','14:00:00',1),(3,'TER','08:00:00',1),(4,'TER','14:00:00',1),(5,'QUA','08:00:00',1),(6,'QUA','14:00:00',1),(7,'QUI','09:00:00',2),(8,'QUI','15:00:00',2),(9,'SEX','09:00:00',2),(10,'SEX','15:00:00',2),(11,'SAB','08:00:00',2),(12,'SEG','10:00:00',3),(13,'SEG','16:00:00',3),(14,'QUA','10:00:00',3),(15,'QUA','16:00:00',3),(16,'SEX','10:00:00',3),(17,'SEX','16:00:00',3);
/*!40000 ALTER TABLE `horariosatendimento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notificacao`
--

DROP TABLE IF EXISTS `notificacao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notificacao` (
  `id` int unsigned NOT NULL,
  `titulo` varchar(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descricao` varchar(150) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `dataEnvio` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notificacao`
--

LOCK TABLES `notificacao` WRITE;
/*!40000 ALTER TABLE `notificacao` DISABLE KEYS */;
INSERT INTO `notificacao` VALUES (1,'Vacina em Atraso','Seu pet precisa renovar a vacinação antirrábica','2024-08-23 09:00:00'),(2,'Consulta Confirmada','Sua consulta foi confirmada para amanhã às 8h','2024-08-21 14:00:00'),(3,'Promoção Especial','Brinquedos com 30% de desconto esta semana!','2024-08-20 10:00:00'),(4,'Lembrete de Consulta','Não esqueça da consulta do seu pet hoje às 10h','2024-08-22 08:00:00');
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
  `status` enum('A','F','C') COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'A = Andamento, F = Finalizado, C = Cancelado',
  `realizadoEm` datetime NOT NULL,
  `idTutor` int unsigned NOT NULL,
  `idFuncionario` int unsigned NOT NULL,
  `idAgendamento` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idUsuario` (`idTutor`),
  KEY `fk_Pedido_Funcionario1_idx` (`idFuncionario`),
  KEY `fk_Pedido_Agendamento1_idx` (`idAgendamento`),
  CONSTRAINT `fk_Pedido_Agendamento1` FOREIGN KEY (`idAgendamento`) REFERENCES `agendamento` (`id`),
  CONSTRAINT `fk_Pedido_Funcionario1` FOREIGN KEY (`idFuncionario`) REFERENCES `funcionario` (`id`),
  CONSTRAINT `pedido_ibfk_1` FOREIGN KEY (`idTutor`) REFERENCES `pessoa` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedido`
--

LOCK TABLES `pedido` WRITE;
/*!40000 ALTER TABLE `pedido` DISABLE KEYS */;
INSERT INTO `pedido` VALUES (1,'F','2024-08-22 10:30:00',12,3,4),(2,'A','2024-08-20 15:30:00',9,2,5);
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
  KEY `idProduto` (`idProduto`),
  KEY `idPedido` (`idPedido`),
  CONSTRAINT `produtopedido_ibfk_1` FOREIGN KEY (`idProduto`) REFERENCES `produto` (`id`),
  CONSTRAINT `produtopedido_ibfk_2` FOREIGN KEY (`idPedido`) REFERENCES `pedido` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedidoproduto`
--

LOCK TABLES `pedidoproduto` WRITE;
/*!40000 ALTER TABLE `pedidoproduto` DISABLE KEYS */;
INSERT INTO `pedidoproduto` VALUES (1,1,75.50,2,1),(2,1,35.00,6,1),(3,1,89.90,1,2),(4,2,12.90,7,2);
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
  `nome` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `senha` varchar(150) COLLATE utf8mb4_unicode_ci NOT NULL,
  `email` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `telefone` char(11) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `tipo` enum('T','G','A','V','Ad') COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'T (Tutor), G (Gerente), A (Atendente), V (Veterinário), Ad (Administrador)',
  `status` enum('A','I') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'A' COMMENT 'A (Ativo), I (Inativo)',
  `logradouro` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `numero` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `complemento` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `bairro` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `cidade` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `estado` char(2) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pessoa`
--

LOCK TABLES `pessoa` WRITE;
/*!40000 ALTER TABLE `pessoa` DISABLE KEYS */;
INSERT INTO `pessoa` VALUES (1,'João Silva','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','joao@admin.com','11987654321','Ad','A','Rua das Flores','123',NULL,'Centro','São Paulo','SP'),(2,'Maria Santos','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','maria@gerente.com','11876543210','G','A','Av. Paulista','1000','Sala 10','Bela Vista','São Paulo','SP'),(3,'Pedro Lima','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','pedro@gerente.com','11765432109','G','A','Rua Augusta','500',NULL,'Consolação','São Paulo','SP'),(4,'Dr. Ana Costa','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','ana@vet.com','11654321098','V','A','Rua Veterinários','200','Apt 5','Vila Madalena','São Paulo','SP'),(5,'Dr. Carlos Mendes','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','carlos@vet.com','11543210987','V','A','Rua dos Pets','300',NULL,'Jardins','São Paulo','SP'),(6,'Dra. Lucia Rocha','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','lucia@vet.com','11432109876','V','A','Av. Rebouças','1500','Bloco A','Pinheiros','São Paulo','SP'),(7,'Julia Oliveira','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','julia@atend.com','11321098765','A','A','Rua do Atendimento','100',NULL,'Vila Olímpia','São Paulo','SP'),(8,'Roberto Ferreira','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','roberto@atend.com','11210987654','A','A','Rua dos Funcionários','250','Casa 2','Mooca','São Paulo','SP'),(9,'Carlos Pereira','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','carlos@tutor.com','11109876543','T','A','Rua dos Pets','400',NULL,'Vila Mariana','São Paulo','SP'),(10,'Fernanda Alves','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','fernanda@tutor.com','11098765432','T','A','Av. Ibirapuera','800','Apt 101','Ibirapuera','São Paulo','SP'),(11,'Ricardo Silva','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','ricardo@tutor.com','11987651234','T','A','Rua das Palmeiras','150',NULL,'Brooklin','São Paulo','SP'),(12,'Mariana Costa','$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi','mariana@tutor.com','11876541239','T','A','Av. Faria Lima','2000','Conj 1502','Itaim Bibi','São Paulo','SP');
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
  `statusLida` tinyint NOT NULL COMMENT '0 - Não lida, 1 - Lida',
  `idPessoa` int unsigned NOT NULL,
  `idNotificacao` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Pessoa_has_Notificacao_Notificacao1_idx` (`idNotificacao`),
  KEY `fk_Pessoa_has_Notificacao_Pessoa1_idx` (`idPessoa`),
  CONSTRAINT `fk_Pessoa_has_Notificacao_Notificacao1` FOREIGN KEY (`idNotificacao`) REFERENCES `notificacao` (`id`),
  CONSTRAINT `fk_Pessoa_has_Notificacao_Pessoa1` FOREIGN KEY (`idPessoa`) REFERENCES `pessoa` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pessoanotificacao`
--

LOCK TABLES `pessoanotificacao` WRITE;
/*!40000 ALTER TABLE `pessoanotificacao` DISABLE KEYS */;
INSERT INTO `pessoanotificacao` VALUES (1,0,9,1),(2,1,10,2),(3,0,11,3),(4,1,12,4),(5,0,9,3),(6,0,10,3);
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pessoapet`
--

LOCK TABLES `pessoapet` WRITE;
/*!40000 ALTER TABLE `pessoapet` DISABLE KEYS */;
INSERT INTO `pessoapet` VALUES (1,9),(2,10),(3,11),(4,12),(5,9),(6,10);
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
  `nome` varchar(20) COLLATE utf8mb4_unicode_ci NOT NULL,
  `sexo` enum('M','F') COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'M (Macho), F (Fêmea)',
  `dataNascimento` date DEFAULT NULL,
  `idRaca` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Pet_Raca1_idx` (`idRaca`),
  CONSTRAINT `fk_Pet_Raca1` FOREIGN KEY (`idRaca`) REFERENCES `raca` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pet`
--

LOCK TABLES `pet` WRITE;
/*!40000 ALTER TABLE `pet` DISABLE KEYS */;
INSERT INTO `pet` VALUES (1,'Rex','M','2020-03-15',1),(2,'Bella','F','2019-07-22',2),(3,'Max','M','2021-01-10',4),(4,'Luna','F','2020-11-05',10),(5,'Thor','M','2022-02-14',3),(6,'Mia','F','2021-06-30',11);
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
  KEY `idPet` (`idPet`),
  KEY `idDoenca` (`idDoenca`),
  CONSTRAINT `petdoenca_ibfk_1` FOREIGN KEY (`idPet`) REFERENCES `pet` (`id`),
  CONSTRAINT `petdoenca_ibfk_2` FOREIGN KEY (`idDoenca`) REFERENCES `doenca` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `petdoenca`
--

LOCK TABLES `petdoenca` WRITE;
/*!40000 ALTER TABLE `petdoenca` DISABLE KEYS */;
INSERT INTO `petdoenca` VALUES (1,'2021-05-15',1,6),(2,'2020-08-10',2,1),(3,'2022-01-20',4,8);
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
  `nome` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `preco` decimal(10,2) NOT NULL,
  `status` enum('I','D','P') COLLATE utf8mb4_unicode_ci DEFAULT 'D' COMMENT 'I (Indisponível), D (Disponível), P (Promoção)',
  `precoPromocao` decimal(10,2) DEFAULT NULL,
  `descricao` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `idCategoria` int unsigned NOT NULL,
  `idEstabelecimento` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idCategoria` (`idCategoria`),
  KEY `fk_produto_estabelecimento1_idx` (`idEstabelecimento`),
  CONSTRAINT `fk_produto_estabelecimento1` FOREIGN KEY (`idEstabelecimento`) REFERENCES `estabelecimento` (`id`),
  CONSTRAINT `produto_ibfk_1` FOREIGN KEY (`idCategoria`) REFERENCES `categoria` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `produto`
--

LOCK TABLES `produto` WRITE;
/*!40000 ALTER TABLE `produto` DISABLE KEYS */;
INSERT INTO `produto` VALUES (1,'Ração Premium Cães Adultos',89.90,'D',NULL,'Ração super premium para cães adultos',1,1),(2,'Ração Gatos Castrados',75.50,'D',NULL,'Ração especial para gatos castrados',1,1),(3,'Antipulgas e Carrapatos',45.00,'D',NULL,'Medicamento contra parasitas',2,1),(4,'Bola Interativa',25.90,'P',19.90,'Brinquedo interativo para cães',3,2),(5,'Shampoo Neutro',18.50,'D',NULL,'Shampoo hipoalergênico',4,2),(6,'Coleira Antipulgas',35.00,'D',NULL,'Coleira com repelente natural',5,2),(7,'Petisco Natural Cães',12.90,'D',NULL,'Petisco 100% natural',6,2),(8,'Vermífugo',28.00,'I',NULL,'Medicamento contra vermes',2,1);
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
  `nome` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `idEspecie` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idEspecie` (`idEspecie`),
  CONSTRAINT `raca_ibfk_1` FOREIGN KEY (`idEspecie`) REFERENCES `especie` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `raca`
--

LOCK TABLES `raca` WRITE;
/*!40000 ALTER TABLE `raca` DISABLE KEYS */;
INSERT INTO `raca` VALUES (1,'Golden Retriever',1),(2,'Labrador',1),(3,'Pastor Alemão',1),(4,'Poodle',1),(5,'Bulldog',1),(6,'Beagle',1),(7,'Yorkshire',1),(8,'Shih Tzu',1),(9,'Vira-lata',1),(10,'Persa',2),(11,'Siamês',2),(12,'Maine Coon',2),(13,'British Shorthair',2),(14,'Vira-lata',2),(15,'Canário',3),(16,'Papagaio',3),(17,'Calopsita',3),(18,'Hamster',4),(19,'Chinchila',4),(20,'Porquinho da Índia',4);
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
  `nome` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `periodoEmDias` smallint unsigned DEFAULT NULL,
  `idDoenca` int unsigned NOT NULL,
  `idEspecie` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idDoenca` (`idDoenca`),
  KEY `fk_Vacina_Especie1_idx` (`idEspecie`),
  CONSTRAINT `fk_Vacina_Especie1` FOREIGN KEY (`idEspecie`) REFERENCES `especie` (`id`),
  CONSTRAINT `vacina_ibfk_1` FOREIGN KEY (`idDoenca`) REFERENCES `doenca` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vacina`
--

LOCK TABLES `vacina` WRITE;
/*!40000 ALTER TABLE `vacina` DISABLE KEYS */;
INSERT INTO `vacina` VALUES (1,'V8 Canina',365,1,1),(2,'V10 Canina',365,2,1),(3,'Antirrábica Canina',365,4,1),(4,'Gripe Canina',365,6,1),(5,'V4 Felina',365,7,2),(6,'V5 Felina',365,8,2),(7,'Antirrábica Felina',365,10,2);
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
  `lote` varchar(20) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `idVacina` int unsigned NOT NULL,
  `idPet` int unsigned NOT NULL,
  `idFuncionario` int unsigned NOT NULL,
  `idTutor` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idVacina` (`idVacina`),
  KEY `idPet` (`idPet`),
  KEY `fk_VacinaPet_Funcionario1_idx` (`idFuncionario`),
  KEY `fk_Vacinacao_Pessoa1_idx` (`idTutor`),
  CONSTRAINT `fk_Vacinacao_Pessoa1` FOREIGN KEY (`idTutor`) REFERENCES `pessoa` (`id`),
  CONSTRAINT `fk_VacinaPet_Funcionario1` FOREIGN KEY (`idFuncionario`) REFERENCES `funcionario` (`id`),
  CONSTRAINT `vacinapet_ibfk_1` FOREIGN KEY (`idVacina`) REFERENCES `vacina` (`id`),
  CONSTRAINT `vacinapet_ibfk_2` FOREIGN KEY (`idPet`) REFERENCES `pet` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vacinacao`
--

LOCK TABLES `vacinacao` WRITE;
/*!40000 ALTER TABLE `vacinacao` DISABLE KEYS */;
INSERT INTO `vacinacao` VALUES (1,'2024-01-15','LOT001',1,1,1,9),(2,'2024-01-15','LOT002',3,1,1,9),(3,'2024-02-10','LOT003',1,2,2,10),(4,'2024-02-10','LOT004',3,2,2,10),(5,'2024-01-20','LOT005',5,4,3,12),(6,'2024-01-20','LOT006',7,4,3,12);
/*!40000 ALTER TABLE `vacinacao` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-08-25 21:40:59
