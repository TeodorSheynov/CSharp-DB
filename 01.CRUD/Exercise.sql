
--PROBLEM 1. CreateDatabase
CREATE DATABASE [Minions]

USE [Minions]
--PROBLEM 2. Create Tables
CREATE TABLE [Minions](
	[Id]	INT PRIMARY KEY NOT NULL,
	[Name]  NVARCHAR(20)    NOT NULL,
	[Age]   INT 
)

CREATE TABLE [Towns](
	[Id]   INT      NOT NULL,
	[Name] NVARCHAR NOT NULL
)

ALTER TABLE [Towns]
ADD CONSTRAINT PK_TownsId PRIMARY KEY ([Id])

--PROBLEM 3. Alter Minions Table
ALTER TABLE [Minions]
ADD [TownId] INT

ALTER TABLE [Minions]
ADD CONSTRAINT FK_MinionsTown FOREIGN KEY ([TownId]) REFERENCES [Towns]([Id])

-- PROBLEM 4. Insert Records in Both Tables
INSERT INTO [Towns]([Id],[Name]) VALUES
(1,'Sofia'),
(2,'Plovdiv'),
(3,'Varna'),
(4,'Burgas')

INSERT INTO [Minions]([Id],[Name],[Age],[TownId]) VALUES
(1,'Pesho',24,3),
(2,'Gosho',12,4),
(3,'Krasi',6,4),
(4,'Vlado',46,1)


--PROBLEM 5. Create Table People
CREATE TABLE [People](
[Id] BIGINT IDENTITY(1,1) PRIMARY KEY NOT NULL,
[Name] NVARCHAR(200) NOT NULL,
[Picture] VARBINARY (2000),
[Height] FLOAT(2),
[Weight] FLOAT(2),
[Gender] CHAR(1) NOT NULL CHECK (Gender= 'm' OR Gender= 'f'),
[Birthdate] DATETIME2	NOT NULL,
[Biography] NVARCHAR(MAX)
)
INSERT INTO [People]([Name],[Picture],[Height],[Weight],[Gender],[Birthdate],[Biography]) VALUES
('Jani',250,2.21,56.23,'f','2021-04-21','sad story'),
('teo',550,2.21,56.23,'f','2021-04-21','ssddd story'),
('hose',750,2.21,56.23,'f','2021-04-21','sadddd story'),
('jamal',230,2.21,56.23,'f','2021-04-21','sadaaa story'),
('Bezait',142,2.21,56.23,'f','2021-04-21','sadcryptostory')