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

CREATE DATABASE IF NOT EXISTS cuidapetdb
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE  cuidapetdb;

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
  `status` enum('S','A','C','R') COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'S (Solicitado), A (Aprovado), C (Cancelado), R (Realizado)',
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `agendamento`
--

LOCK TABLES `agendamento` WRITE;
/*!40000 ALTER TABLE `agendamento` DISABLE KEYS */;
INSERT INTO `agendamento` VALUES (1,'2024-08-01','2024-08-01','08:00:00','R',1,1,1),(2,'2024-08-02','2024-08-02','09:00:00','R',2,2,2),(3,'2024-08-03','2024-08-03','10:00:00','A',3,3,3),(4,'2024-08-04','2024-08-04','11:00:00','A',4,4,4),(5,'2024-08-05',NULL,'14:00:00','S',5,5,5),(6,'2024-08-06','2024-08-06','15:00:00','R',6,1,6),(7,'2024-08-07','2024-08-07','16:00:00','A',7,2,7),(8,'2024-08-08',NULL,'08:30:00','S',8,3,8),(9,'2024-08-09','2024-08-09','09:30:00','C',9,4,9),(10,'2024-08-10','2024-08-10','10:30:00','R',10,5,10),(11,'2024-08-11','2024-08-11','11:30:00','A',11,1,1),(12,'2024-08-12',NULL,'14:30:00','S',12,2,2),(13,'2024-08-13','2024-08-13','15:30:00','R',13,3,3),(14,'2024-08-14','2024-08-14','16:30:00','A',14,4,4),(15,'2024-08-15',NULL,'08:45:00','S',15,5,5),(16,'2024-08-16','2024-08-16','09:45:00','R',16,1,6),(17,'2024-08-17','2024-08-17','10:45:00','C',17,2,7),(18,'2024-08-18',NULL,'11:45:00','S',18,3,8),(19,'2024-08-19','2024-08-19','14:45:00','A',19,4,9),(20,'2024-08-20','2024-08-20','15:45:00','R',20,5,10);
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categoria`
--

LOCK TABLES `categoria` WRITE;
/*!40000 ALTER TABLE `categoria` DISABLE KEYS */;
INSERT INTO `categoria` VALUES (1,'Ração','Alimentos para pets'),(2,'Brinquedos','Itens para entretenimento'),(3,'Higiene','Produtos de limpeza e cuidado'),(4,'Medicamentos','Remédios e suplementos'),(5,'Acessórios','Coleiras, guias, camas'),(6,'Petiscos','Agrados e recompensas'),(7,'Aquário','Itens para peixes'),(8,'Gaiolas','Casas para aves e pequenos'),(9,'Cosméticos','Produtos de beleza pet'),(10,'Veterinários','Materiais profissionais'),(11,'Transporte','Caixas e bolsas de viagem'),(12,'Jardim','Itens para pets ao ar livre'),(13,'Cama','Locais para dormir'),(14,'Alimentação','Comedouros e bebedouros'),(15,'Limpeza','Produtos para higienização'),(16,'Saúde','Itens preventivos'),(17,'Beleza','Cuidados estéticos'),(18,'Segurança','Itens de proteção'),(19,'Conforto','Produtos para bem-estar'),(20,'Treinamento','Itens educativos');
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
INSERT INTO `consulta` VALUES (1,'2024-08-01 08:00:00','Consulta de rotina. Animal saudável.',1,1,1,1),(2,'2024-08-02 09:00:00','Vacinação em dia. Orientações sobre alimentação.',2,2,2,2),(6,'2024-08-06 15:00:00','Exame dermatológico. Prescrição de medicamento.',6,6,1,6),(10,'2024-08-10 10:30:00','Check-up geral. Animal em bom estado.',10,10,5,10),(13,'2024-08-13 15:30:00','Consulta comportamental. Orientações ao tutor.',3,13,3,13),(16,'2024-08-16 09:45:00','Exame oftalmológico. Sem alterações.',6,16,1,16),(20,'2024-08-20 15:45:00','Consulta nutricional. Plano alimentar.',10,20,5,20);
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doenca`
--

LOCK TABLES `doenca` WRITE;
/*!40000 ALTER TABLE `doenca` DISABLE KEYS */;
INSERT INTO `doenca` VALUES (1,'Cinomose',1),(2,'Parvovirose',1),(3,'Hepatite',1),(4,'Raiva',1),(5,'Tosse dos Canis',1),(6,'Panleucopenia',2),(7,'Rinotraqueíte',2),(8,'Calicivirose',2),(9,'Clamidiose',2),(10,'Leucemia Felina',2),(11,'Mixomatose',3),(12,'Febre Hemorrágica',3),(13,'Doença Respiratória',4),(14,'Wet Tail',4),(15,'Psitacose',5),(16,'Newcastle',5),(17,'Podridão das Barbatanas',6),(18,'Íctio',6),(19,'Infecção Respiratória',7),(20,'Podridão do Casco',7);
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `especialidade`
--

LOCK TABLES `especialidade` WRITE;
/*!40000 ALTER TABLE `especialidade` DISABLE KEYS */;
INSERT INTO `especialidade` VALUES (1,'Clínica Geral','Atendimento geral para animais'),(2,'Cirurgia','Procedimentos cirúrgicos'),(3,'Dermatologia','Tratamento de pele e pelos'),(4,'Cardiologia','Problemas cardiovasculares'),(5,'Ortopedia','Problemas ósseos e articulares'),(6,'Oftalmologia','Tratamento dos olhos'),(7,'Neurologia','Sistema nervoso'),(8,'Oncologia','Tratamento de câncer'),(9,'Endocrinologia','Distúrbios hormonais'),(10,'Nutrição','Orientação alimentar'),(11,'Comportamento','Problemas comportamentais'),(12,'Reprodução','Medicina reprodutiva'),(13,'Fisioterapia','Reabilitação física'),(14,'Acupuntura','Medicina alternativa'),(15,'Homeopatia','Tratamento homeopático'),(16,'Exóticos','Animais não convencionais'),(17,'Urgência','Atendimento de emergência'),(18,'Preventiva','Medicina preventiva'),(19,'Geriatria','Cuidados com idosos'),(20,'Pediatria','Filhotes e jovens');
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `especie`
--

LOCK TABLES `especie` WRITE;
/*!40000 ALTER TABLE `especie` DISABLE KEYS */;
INSERT INTO `especie` VALUES (1,'Cão'),(2,'Gato'),(3,'Coelho'),(4,'Hamster'),(5,'Pássaro'),(6,'Peixe'),(7,'Tartaruga'),(8,'Chinchila'),(9,'Porquinho-da-índia'),(10,'Ferret');
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `estabelecimento`
--

LOCK TABLES `estabelecimento` WRITE;
/*!40000 ALTER TABLE `estabelecimento` DISABLE KEYS */;
INSERT INTO `estabelecimento` VALUES (1,'CuidaPet Clínica Central','C','12345678000101','1133334444','Rua Veterinária','100',NULL,'Vila Olímpia','São Paulo','SP',16),(2,'PetShop Amigos','P','23456789000102','1144445555','Av. dos Pets','200','Loja 1','Moema','São Paulo','SP',17),(3,'VetCenter Completo','A','34567890000103','1155556666','Rua Completa','300',NULL,'Brooklin','São Paulo','SP',16),(4,'Clínica Animal Care','C','45678901000104','1166667777','Av. Cuidado','400','Térreo','Campo Belo','São Paulo','SP',17),(5,'PetLand Shopping','P','56789012000105','1177778888','Rua Shopping','500','Loja 25','Santo Amaro','São Paulo','SP',16);
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
  `crmv` varchar(7) COLLATE utf8mb4_unicode_ci,
  `idPessoa` int unsigned NOT NULL,
  `idEstabelecimento` int unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `crmv` (`crmv`),
  UNIQUE KEY `idUsuario` (`idPessoa`),
  KEY `fk_Funcionario_Estabelecimento1_idx` (`idEstabelecimento`),
  CONSTRAINT `fk_Funcionario_Estabelecimento1` FOREIGN KEY (`idEstabelecimento`) REFERENCES `estabelecimento` (`id`),
  CONSTRAINT `veterinario_ibfk_2` FOREIGN KEY (`idPessoa`) REFERENCES `pessoa` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `funcionario`
--

LOCK TABLES `funcionario` WRITE;
/*!40000 ALTER TABLE `funcionario` DISABLE KEYS */;
INSERT INTO `funcionario` VALUES (1,'SP12345',11,1),(2,'SP23456',12,1),(3,'SP34567',13,2),(4,'SP45678',14,3),(5,'SP56789',15,4),(6,'AT00001',18,1),(7,'AT00002',19,2),(8,'AT00003',16,3),(9,'AT00004',17,4),(10,'AT00005',20,5);
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
INSERT INTO `funcionarioespecialidade` VALUES (1,1),(4,1),(1,2),(5,2),(1,3),(2,3),(2,4),(3,4),(3,5),(2,6),(4,6),(4,7),(5,7),(4,8),(5,9),(5,10),(5,11),(1,12),(2,13),(3,14),(4,15),(5,16),(1,17),(1,18),(2,18),(2,19),(3,19),(3,20),(4,20);
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
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `horariosatendimento`
--

LOCK TABLES `horariosatendimento` WRITE;
/*!40000 ALTER TABLE `horariosatendimento` DISABLE KEYS */;
INSERT INTO `horariosatendimento` VALUES (1,'SEG','08:00:00',1),(2,'SEG','14:00:00',1),(3,'TER','08:00:00',1),(4,'TER','14:00:00',1),(5,'QUA','08:00:00',2),(6,'QUA','14:00:00',2),(7,'QUI','08:00:00',2),(8,'QUI','14:00:00',2),(9,'SEX','08:00:00',3),(10,'SEX','14:00:00',3),(11,'SAB','08:00:00',3),(12,'SAB','14:00:00',3),(13,'SEG','09:00:00',4),(14,'SEG','15:00:00',4),(15,'TER','09:00:00',4),(16,'TER','15:00:00',4),(17,'QUA','09:00:00',5),(18,'QUA','15:00:00',5),(19,'QUI','09:00:00',5),(20,'QUI','15:00:00',5),(21,'SEX','10:00:00',1),(22,'SAB','10:00:00',2);
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
INSERT INTO `notificacao` VALUES (1,'Consulta Agendada','Sua consulta foi confirmada para amanhã','2024-07-31 18:00:00'),(2,'Vacina em Atraso','Seu pet precisa tomar a vacina anual','2024-08-01 09:00:00'),(3,'Promoção Especial','Descontos em rações premium','2024-08-02 10:00:00'),(4,'Lembrete de Consulta','Consulta hoje às 14h','2024-08-03 08:00:00'),(5,'Resultado de Exame','Exames disponíveis para consulta','2024-08-04 16:00:00'),(6,'Nova Vacina Disponível','Vacina contra gripe canina','2024-08-05 11:00:00'),(7,'Agendamento Cancelado','Consulta foi cancelada pelo veterinário','2024-08-06 15:00:00'),(8,'Pedido Processado','Seu pedido foi enviado','2024-08-07 13:00:00'),(9,'Horário Disponível','Novo horário liberado','2024-08-08 12:00:00'),(10,'Medicamento Pronto','Medicamento disponível para retirada','2024-08-09 14:00:00'),(11,'Consulta de Retorno','Agende sua consulta de retorno','2024-08-10 17:00:00'),(12,'Oferta Limitada','Produtos com 30% de desconto','2024-08-11 10:30:00'),(13,'Vacinação em Dia','Parabéns! Vacinação em dia','2024-08-12 09:15:00'),(14,'Consulta Confirmada','Consulta confirmada para quinta-feira','2024-08-13 19:00:00'),(15,'Novo Veterinário','Dr. João se juntou à nossa equipe','2024-08-14 08:30:00'),(16,'Limpeza de Dentes','Mês da saúde bucal pet','2024-08-15 11:45:00'),(17,'Castração Gratuita','Campanha de castração','2024-08-16 14:20:00'),(18,'Exame de Sangue','Agende check-up completo','2024-08-17 16:10:00'),(19,'Produto Indisponível','Item temporariamente em falta','2024-08-18 12:30:00'),(20,'Feliz Aniversário!','Parabéns pelo aniversário do seu pet','2024-08-19 07:00:00');
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedido`
--

LOCK TABLES `pedido` WRITE;
/*!40000 ALTER TABLE `pedido` DISABLE KEYS */;
INSERT INTO `pedido` VALUES (1,'F','2024-08-01 08:30:00',1,1,1),(2,'F','2024-08-02 09:30:00',2,2,2),(3,'A','2024-08-06 15:30:00',6,1,6),(4,'F','2024-08-10 11:00:00',10,5,10),(5,'A','2024-08-13 16:00:00',3,3,13),(6,'F','2024-08-16 10:15:00',6,1,16),(7,'F','2024-08-20 16:15:00',10,5,20),(8,'C','2024-08-03 14:00:00',3,3,3),(9,'A','2024-08-04 15:00:00',4,4,4),(10,'F','2024-08-07 17:00:00',7,2,7),(11,'A','2024-08-11 12:00:00',1,1,11),(12,'F','2024-08-14 17:30:00',4,4,14),(13,'C','2024-08-17 18:00:00',7,2,17),(14,'A','2024-08-19 15:15:00',9,4,19),(15,'F','2024-08-05 16:00:00',5,5,5),(16,'A','2024-08-08 17:30:00',8,3,8),(17,'F','2024-08-12 18:00:00',2,2,12),(18,'C','2024-08-15 19:00:00',5,5,15),(19,'A','2024-08-18 20:00:00',8,3,18),(20,'F','2024-08-21 08:00:00',21,1,1);
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
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pedidoproduto`
--

LOCK TABLES `pedidoproduto` WRITE;
/*!40000 ALTER TABLE `pedidoproduto` DISABLE KEYS */;
INSERT INTO `pedidoproduto` VALUES (1,2,89.90,1,1),(2,1,45.00,5,1),(3,1,67.50,2,2),(4,3,15.90,7,2),(5,1,24.90,4,3),(6,2,12.90,3,3),(7,1,125.00,8,4),(8,1,32.90,10,4),(9,2,38.50,6,5),(10,4,2.50,11,5),(11,1,95.00,12,6),(12,1,28.90,15,6),(13,1,156.90,14,7),(14,2,35.90,16,7),(15,3,18.90,18,8),(16,1,42.00,17,8),(17,2,29.90,19,9),(18,1,67.90,20,9),(19,1,8.90,21,10),(20,2,89.90,1,10),(21,1,180.00,9,11),(22,3,24.90,4,11),(23,2,67.50,2,12),(24,1,125.00,8,12),(25,1,95.00,12,13),(26,2,15.90,7,13),(27,3,12.90,3,14),(28,1,45.00,5,14),(29,1,220.00,13,15),(30,2,38.50,6,15),(31,4,2.50,11,16),(32,1,156.90,14,16),(33,2,28.90,15,17),(34,3,35.90,16,17),(35,1,42.00,17,18),(36,2,18.90,18,18),(37,1,29.90,19,19),(38,3,67.90,20,19),(39,2,8.90,21,20),(40,1,89.90,1,20);
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
  `cpf` char(11) COLLATE utf8mb4_unicode_ci NOT NULL,
  `nome` varchar(30) COLLATE utf8mb4_unicode_ci NOT NULL,
  `senha` varchar(150) COLLATE utf8mb4_unicode_ci NOT NULL,
  `email` varchar(30) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `telefone` char(11) COLLATE utf8mb4_unicode_ci NOT NULL,
  `tipo` enum('T','G','A','V','Ad') COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'T (Tutor), G (Gerente), A (Atendente), V (Veterinário), Ad (Administrador)',
  `status` enum('A','I') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'A' COMMENT 'A (Ativo), I (Inativo)',
  `logradouro` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `numero` varchar(10) COLLATE utf8mb4_unicode_ci NOT NULL,
  `complemento` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `bairro` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `cidade` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `estado` char(2) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `telefone_UNIQUE` (`telefone`),
  UNIQUE KEY `cpf_UNIQUE` (`cpf`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pessoa`
--

LOCK TABLES `pessoa` WRITE;
/*!40000 ALTER TABLE `pessoa` DISABLE KEYS */;
INSERT INTO `pessoa` VALUES (1,'12345678901','João Silva','$2y$10$abcd123','joao.silva@email.com','11987654321','T','A','Rua das Flores','123','Apto 1','Centro','São Paulo','SP'),(2,'23456789012','Maria Santos','$2y$10$efgh456','maria.santos@email.com','11876543210','T','A','Av. Brasil','456',NULL,'Jardim América','São Paulo','SP'),(3,'34567890123','Pedro Oliveira','$2y$10$ijkl789','pedro.oliveira@email.com','11765432109','T','A','Rua São João','789','Casa 2','Vila Madalena','São Paulo','SP'),(4,'45678901234','Ana Costa','$2y$10$mnop012','ana.costa@email.com','11654321098','T','A','Av. Paulista','1000','Sala 15','Bela Vista','São Paulo','SP'),(5,'56789012345','Carlos Pereira','$2y$10$qrst345','carlos.pereira@email.com','11543210987','T','A','Rua Augusta','234',NULL,'Consolação','São Paulo','SP'),(6,'67890123456','Lucia Ferreira','$2y$10$uvwx678','lucia.ferreira@email.com','11432109876','T','A','Rua Oscar Freire','567','Loja 3','Jardins','São Paulo','SP'),(7,'78901234567','Roberto Lima','$2y$10$yzab901','roberto.lima@email.com','11321098765','T','A','Av. Faria Lima','890','Conj 45','Itaim Bibi','São Paulo','SP'),(8,'89012345678','Fernanda Souza','$2y$10$cdef234','fernanda.souza@email.com','11210987654','T','A','Rua Haddock Lobo','345',NULL,'Cerqueira César','São Paulo','SP'),(9,'90123456789','Ricardo Alves','$2y$10$ghij567','ricardo.alves@email.com','11109876543','T','A','Av. Rebouças','678','Apto 12','Pinheiros','São Paulo','SP'),(10,'01234567890','Camila Torres','$2y$10$klmn890','camila.torres@email.com','11098765432','T','A','Rua da Consolação','901',NULL,'Centro','São Paulo','SP'),(11,'12345678902','Dr. Antonio Silva','$2y$10$opqr123','antonio.silva@clinica.com','11987654322','V','A','Rua Veterinária','100',NULL,'Vila Olímpia','São Paulo','SP'),(12,'23456789013','Dra. Beatriz Lima','$2y$10$stuv456','beatriz.lima@clinica.com','11876543211','V','A','Av. dos Veterinários','200','Sala 10','Moema','São Paulo','SP'),(13,'34567890124','Dr. Carlos Santos','$2y$10$wxyz789','carlos.santos@clinica.com','11765432110','V','A','Rua Animal','300',NULL,'Brooklin','São Paulo','SP'),(14,'45678901235','Dra. Diana Costa','$2y$10$abcd012','diana.costa@clinica.com','11654321099','V','A','Av. Pet Care','400','Conj 5','Campo Belo','São Paulo','SP'),(15,'56789012346','Dr. Eduardo Pereira','$2y$10$efgh345','eduardo.pereira@clinica.com','11543210988','V','A','Rua Saúde Animal','500',NULL,'Santo Amaro','São Paulo','SP'),(16,'67890123457','Marcos Gerente','$2y$10$ijkl678','marcos.gerente@petshop.com','11432109877','G','A','Rua Gestão','600','Casa 1','Vila Mariana','São Paulo','SP'),(17,'78901234568','Paula Gerente','$2y$10$mnop901','paula.gerente@clinica.com','11321098766','G','A','Av. Administração','700',NULL,'Ipiranga','São Paulo','SP'),(18,'89012345679','Sandra Atendente','$2y$10$qrst234','sandra.atendente@petshop.com','11210987655','A','A','Rua Atendimento','800','Apto 8','Saúde','São Paulo','SP'),(19,'90123456780','Rafael Atendente','$2y$10$uvwx567','rafael.atendente@clinica.com','11109876544','A','A','Av. Recepção','900',NULL,'Vila Prudente','São Paulo','SP'),(20,'01234567891','Admin Sistema','$2y$10$yzab890','admin@cuidapet.com','11098765433','Ad','A','Rua Sistema','1000','Sala Admin','Centro','São Paulo','SP'),(21,'11111111111','José Tutor','$2y$10$test123','jose.tutor@email.com','11888888888','T','A','Rua Teste','111',NULL,'Teste','São Paulo','SP'),(22,'22222222222','Clara Tutora','$2y$10$test456','clara.tutora@email.com','11777777777','T','A','Av. Teste','222',NULL,'Teste','São Paulo','SP');
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
) ENGINE=InnoDB AUTO_INCREMENT=61 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pessoanotificacao`
--

LOCK TABLES `pessoanotificacao` WRITE;
/*!40000 ALTER TABLE `pessoanotificacao` DISABLE KEYS */;
INSERT INTO `pessoanotificacao` VALUES (1,1,1,1),(2,0,1,2),(3,1,1,11),(4,1,2,3),(5,0,2,4),(6,1,2,12),(7,0,3,5),(8,1,3,6),(9,0,3,13),(10,1,4,7),(11,0,4,8),(12,1,4,14),(13,0,5,9),(14,1,5,10),(15,0,5,15),(16,1,6,1),(17,1,6,16),(18,0,6,3),(19,0,7,2),(20,1,7,17),(21,1,7,4),(22,1,8,5),(23,0,8,18),(24,1,8,6),(25,0,9,7),(26,1,9,19),(27,0,9,8),(28,1,10,9),(29,0,10,20),(30,1,10,10),(31,0,21,1),(32,1,21,2),(33,0,21,3),(34,1,22,4),(35,0,22,5),(36,1,22,6),(37,0,1,7),(38,1,2,8),(39,0,3,9),(40,1,4,10),(41,0,5,11),(42,1,6,12),(43,0,7,13),(44,1,8,14),(45,0,9,15),(46,1,10,16),(47,0,21,17),(48,1,22,18),(49,0,1,19),(50,1,2,20),(51,0,3,1),(52,1,4,2),(53,0,5,3),(54,1,6,4),(55,0,7,5),(56,1,8,6),(57,0,9,7),(58,1,10,8),(59,0,21,9),(60,1,22,10);
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
INSERT INTO `pessoapet` VALUES (1,1),(2,2),(3,3),(4,4),(5,5),(6,6),(7,7),(8,8),(9,9),(10,10),(11,1),(12,2),(13,3),(14,4),(15,5),(16,6),(17,7),(18,8),(19,9),(20,10),(21,21),(22,22);
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
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pet`
--

LOCK TABLES `pet` WRITE;
/*!40000 ALTER TABLE `pet` DISABLE KEYS */;
INSERT INTO `pet` VALUES (1,'Rex','M','2020-05-15',1),(2,'Bella','F','2019-08-22',2),(3,'Max','M','2021-03-10',3),(4,'Luna','F','2020-12-05',4),(5,'Charlie','M','2022-01-18',5),(6,'Mimi','F','2019-07-30',6),(7,'Garfield','M','2020-09-14',7),(8,'Princesa','F','2021-06-08',8),(9,'Simba','M','2020-11-25',9),(10,'Nala','F','2021-04-12',10),(11,'Coelho','M','2021-02-28',11),(12,'Branquinha','F','2020-10-15',12),(13,'Saltitão','M','2021-08-03',13),(14,'Bolinha','F','2021-05-20',14),(15,'Hammy','M','2021-12-10',15),(16,'Piu','M','2020-03-25',16),(17,'Amarelinha','F','2021-01-15',17),(18,'Loro','M','2019-11-08',18),(19,'Azul','M','2021-07-22',19),(20,'Dourada','F','2021-09-05',20),(21,'Tartaruga','F','2018-05-10',21),(22,'Jabuti','M','2017-08-15',22);
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `petdoenca`
--

LOCK TABLES `petdoenca` WRITE;
/*!40000 ALTER TABLE `petdoenca` DISABLE KEYS */;
INSERT INTO `petdoenca` VALUES (1,'2024-07-01',1,1),(2,'2024-07-02',2,6),(3,'2024-07-03',3,2),(4,'2024-07-04',6,7),(5,'2024-07-05',7,8),(6,'2024-07-06',11,11),(7,'2024-07-07',12,12),(8,'2024-07-08',13,13),(9,'2024-07-09',16,15),(10,'2024-07-10',17,16),(11,'2024-07-11',4,3),(12,'2024-07-12',5,4),(13,'2024-07-13',8,9),(14,'2024-07-14',9,10),(15,'2024-07-15',14,14),(16,'2024-07-16',15,13),(17,'2024-07-17',18,15),(18,'2024-07-18',19,17),(19,'2024-07-19',20,18),(20,'2024-07-20',21,19);
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
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `produto`
--

LOCK TABLES `produto` WRITE;
/*!40000 ALTER TABLE `produto` DISABLE KEYS */;
INSERT INTO `produto` VALUES (1,'Ração Premium Cães',89.90,'D',NULL,'Ração super premium para cães adultos',1,1),(2,'Ração Gatos Filhotes',67.50,'P',55.90,'Ração especial para filhotes',1,2),(3,'Bola de Borracha',12.90,'D',NULL,'Brinquedo resistente',2,3),(4,'Shampoo Neutro',24.90,'D',NULL,'Para pelos sensíveis',3,1),(5,'Antipulgas Spray',45.00,'D',NULL,'Proteção contra parasitas',4,1),(6,'Coleira de Couro',38.50,'P',29.90,'Coleira resistente e elegante',5,2),(7,'Petisco Natural',15.90,'D',NULL,'Recompensa saudável',6,3),(8,'Filtro Aquário',125.00,'D',NULL,'Filtro para aquários médios',7,1),(9,'Gaiola Grande',180.00,'I',NULL,'Para aves grandes',8,2),(10,'Condicionador',32.90,'D',NULL,'Para pelos macios',9,3),(11,'Seringa 5ml',2.50,'D',NULL,'Material veterinário',10,1),(12,'Caixa Transporte',95.00,'P',79.90,'Para viagens seguras',11,2),(13,'Casa Externa',220.00,'D',NULL,'Casa resistente às intempéries',12,3),(14,'Cama Ortopédica',156.90,'D',NULL,'Cama confortável para pets',13,1),(15,'Comedouro Duplo',28.90,'P',22.50,'Comedouro com bebedouro',14,2),(16,'Tapete Higiênico',35.90,'D',NULL,'Para treinamento',15,3),(17,'Vitamina C Pet',42.00,'D',NULL,'Suplemento vitamínico',16,1),(18,'Escova de Pelos',18.90,'D',NULL,'Para escovação diária',17,2),(19,'Protetor Solar Pet',29.90,'P',24.90,'Proteção contra UV',18,3),(20,'Almofada Térmica',67.90,'D',NULL,'Para aquecimento',19,1),(21,'Clicker Training',8.90,'D',NULL,'Para adestramento',20,2);
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
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `raca`
--

LOCK TABLES `raca` WRITE;
/*!40000 ALTER TABLE `raca` DISABLE KEYS */;
INSERT INTO `raca` VALUES (1,'Labrador',1),(2,'Golden Retriever',1),(3,'Bulldog Francês',1),(4,'Poodle',1),(5,'Pastor Alemão',1),(6,'Siamês',2),(7,'Persa',2),(8,'Maine Coon',2),(9,'Ragdoll',2),(10,'British Shorthair',2),(11,'Angorá',3),(12,'Mini Lop',3),(13,'Holandês',3),(14,'Sírio',4),(15,'Chinês',4),(16,'Canário',5),(17,'Periquito',5),(18,'Calopsita',5),(19,'Betta',6),(20,'Guppy',6),(21,'Tigre d\'água',7),(22,'Jabuti',7),(23,'Comum',8),(24,'Americano',9),(25,'Angora',10);
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vacina`
--

LOCK TABLES `vacina` WRITE;
/*!40000 ALTER TABLE `vacina` DISABLE KEYS */;
INSERT INTO `vacina` VALUES (1,'V8 Múltipla',365,1,1),(2,'V10 Múltipla',365,2,1),(3,'Antirrábica Canina',365,4,1),(4,'Gripe Canina',365,5,1),(5,'Tríplice Felina',365,6,2),(6,'Quíntupla Felina',365,7,2),(7,'Antirrábica Felina',365,4,2),(8,'Leucemia Felina',365,10,2),(9,'Mixomatose',180,11,3),(10,'Febre Hemorrágica Coelhos',180,12,3),(11,'Newcastle Aves',365,16,5),(12,'Psitacose',365,15,5),(13,'V12 Múltipla',365,1,1),(14,'Bordetella',365,5,1),(15,'Giárdia',365,3,1),(16,'Coronavírus',365,2,1),(17,'Clamídia Felina',365,9,2),(18,'Rinotraqueíte Felina',365,7,2),(19,'Hepatite Canina',365,3,1),(20,'Parainfluenza',365,5,1);
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vacinacao`
--

LOCK TABLES `vacinacao` WRITE;
/*!40000 ALTER TABLE `vacinacao` DISABLE KEYS */;
INSERT INTO `vacinacao` VALUES (1,'2024-06-01','L001A',1,1,1,1),(2,'2024-06-02','L002B',2,2,2,2),(3,'2024-06-03','L003C',3,1,1,1),(4,'2024-06-04','L004D',4,3,3,3),(5,'2024-06-05','L005E',5,6,1,6),(6,'2024-06-06','L006F',6,7,2,7),(7,'2024-06-07','L007G',7,6,1,6),(8,'2024-06-08','L008H',8,10,5,10),(9,'2024-06-09','L009I',9,11,1,1),(10,'2024-06-10','L010J',10,12,2,2),(11,'2024-06-11','L011K',11,16,1,6),(12,'2024-06-12','L012L',12,17,2,7),(13,'2024-06-13','L013M',1,5,5,5),(14,'2024-06-14','L014N',2,4,4,4),(15,'2024-06-15','L015O',3,9,4,9),(16,'2024-06-16','L016P',4,8,3,8),(17,'2024-06-17','L017Q',5,15,5,5),(18,'2024-06-18','L018R',6,14,4,4),(19,'2024-06-19','L019S',7,13,3,3),(20,'2024-06-20','L020T',8,18,3,8);
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

-- Dump completed on 2025-08-28 13:47:19
