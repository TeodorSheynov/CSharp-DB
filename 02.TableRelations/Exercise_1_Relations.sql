CREATE DATABASE [SoftUni]

USE [SoftUni]

CREATE TABLE [Towns](
[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
[Name] NVARCHAR(25) NOT NULL
)

CREATE TABLE [Addresses](
[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
[AdressText] NVARCHAR(100) ,
[TownId] INT FOREIGN KEY REFERENCES [Towns]([Id])
)

CREATE TABLE [Department](
[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
[Name] NVARCHAR(15) NOT NULL
)

CREATE TABLE [Employees](
[Id] INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
[FirstName] NVARCHAR(15) NOT NULL,
[MiddleName] NVARCHAR(20),
[LastName] NVARCHAR(20) NOT NULL,
[JobTitle] VARCHAR(15) NOT NULL,
[DepartmentId] INT FOREIGN KEY REFERENCES [Department]([Id]),
[HireDate] DATETIME2 NOT NULL,
[Salary] DECIMAL NOT NULL,
[AddressId] INT FOREIGN KEY REFERENCES [Addresses]([Id])
)