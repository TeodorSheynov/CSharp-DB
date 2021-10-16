CREATE DATABASE [Relations]

use [Relations]


--Problem 1.	One-To-One Relationship
CREATE TABLE [Passports](
	[PassportID] INT IDENTITY(101,1) PRIMARY KEY NOT NULL,
	[PassportNumber] VARCHAR(15) UNIQUE NOT NULL
)

CREATE TABLE [Persons](
	[PersonID] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[FirstName] NVARCHAR(30) NOT NULL,
	[Salary] DECIMAL(10,2) NOT NULL,
	[PassportID] INT FOREIGN KEY REFERENCES [Passports]([PassportID]) UNIQUE NOT NULL
)

INSERT INTO [Passports]([PassportNumber]) VALUES
('N34FG21B'),
('K65LO4R7'),
('ZE657QP2')

INSERT INTO [Persons]([FirstName],[Salary],[PassportID]) VALUES
('Roberto',43300.00,102),
('Tom',56100.00,103),
('Yana',60200.00,101)

--Problem 2.	One-To-Many Relationship
CREATE TABLE [Manufacturers](
	[ManufacturerID] INT IDENTITY PRIMARY KEY NOT NULL,
	[Name] VARCHAR(20) UNIQUE NOT NULL,
	[EstablishedOn] DATE NOT NULL
)

CREATE TABLE [Models](
[ModelID] INT IDENTITY(101,1) PRIMARY KEY NOT NULL,
[Name] VARCHAR(20) NOT NULL,
[ManufacturerID] INT FOREIGN KEY REFERENCES [Manufacturers]([ManufacturerID]) NOT NULL
)

INSERT INTO [Manufacturers]([Name],[EstablishedOn]) VALUES
('BMW','07/03/1916'),
('Tesla','01/01/2003'),
('Lada','01/05/1966')

INSERT INTO [Models]([Name],[ManufacturerID]) VALUES
('X1',1),
('i6',1),
('Model S',2),
('Model X',2),
('Model 3',2),
('Nova',3)

--Problem 3.	Many-To-Many Relationship
CREATE TABLE [Students](
[StudentID] INT IDENTITY PRIMARY KEY NOT NULL,
[Name] VARCHAR(20) NOT NULL
)

CREATE TABLE [Exams](
[ExamID] INT IDENTITY(101,1) PRIMARY KEY NOT NULL,
[Name] VARCHAR(20) NOT NULL
)

CREATE TABLE [StudentsExams](
[StudentID] INT FOREIGN KEY REFERENCES [Students]([StudentID]) NOT NULL,
[ExamID] INT FOREIGN KEY REFERENCES [Exams]([ExamID]) NOT NULL,
PRIMARY KEY ([StudentID],[ExamID])
)

INSERT INTO [Students]([Name]) VALUES
('Mila'),
('Toni'),
('Ron')

INSERT INTO [Exams]([Name]) VALUES
('SpringMVC'),
('Neo4j'),
('Oracle 11g')


INSERT INTO [StudentsExams]([StudentID],[ExamID]) VALUES
(1,101),
(1,102),
(2,101),
(3,103),
(2,102),
(2,103)

--Problem 6.	University Database
CREATE DATABASE [University_DB]

USE [University_DB]

CREATE TABLE [Majors](
[MajorID] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
[Name] VARCHAR(20) UNIQUE NOT NULL
)

CREATE TABLE [Subjects](
[SubjectID] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
[SubjectName] VARCHAR(20) UNIQUE NOT NULL
)

CREATE TABLE [Students](
[StudentID] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
[StudenNumber] INT NOT NULL,
[StudentName] VARCHAR(20) NOT NULL,
[MajorID] INT FOREIGN KEY REFERENCES [Majors]([MajorID]) NOT NULL
)

CREATE TABLE [Payments](
[PaymentID] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
[PaymentDate] DATE NOT NULL,
[PaymentAmount] DECIMAL(10,2) NOT NULL,
[StudentID] INT FOREIGN KEY REFERENCES [Students]([StudentID])NOT NULL
)

CREATE TABLE [Agenda](
[StudentID] INT FOREIGN KEY REFERENCES [Students]([StudentID]) NOT NULL,
[SubjectID] INT FOREIGN KEY REFERENCES [Subjects]([SubjectID])NOT NULL,
PRIMARY KEY ([StudentID],[SubjectID])
)

USE [Relations]
CREATE TABLE [Teachers](
[TeacherID] INT IDENTITY(101,1) PRIMARY KEY,
[Name] NVARCHAR(20) NOT NULL,
[ManagerID] INT FOREIGN KEY REFERENCES Teachers([TeacherID])
)

INSERT INTO [Teachers] VALUES
('John', NULL),
('Maya', 106),
('Silvia', 106),
('Ted', 105),
('Mark', 101),
('Greta', 101)