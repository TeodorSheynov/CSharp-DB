CREATE DATABASE [Service]

USE [Service]

CREATE TABLE [Users](
[Id] INT PRIMARY KEY IDENTITY,
[Username] VARCHAR(30) UNIQUE NOT NULL,
[Password] VARCHAR(50) NOT NULL,
[Name] VARCHAR(50),
[Birthdate] DATETIME2,
[Age] INT ,
CHECK([AGE] BETWEEN  14 AND 110),
[Email] VARCHAR(50) NOT NULL
)

CREATE TABLE [Departments](
[Id] INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE [Employees](
[Id] INT PRIMARY KEY IDENTITY,
[FirstName] VARCHAR(25),
[LastName] VARCHAR(25),
[Birthdate] DATETIME2,
[Age] INT ,
CHECK([Age] BETWEEN 18 AND 110),
[DepartmentId] INT FOREIGN KEY REFERENCES [Departments]([Id])
)

CREATE TABLE [Categories](
[Id] INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50) NOT NULL,
[DepartmentId] INT FOREIGN KEY REFERENCES [Departments]([Id]) NOT NULL
)

CREATE TABLE [Status](
[Id] INT PRIMARY KEY IDENTITY,
[Label] VARCHAR(50) NOT NULL
)

CREATE TABLE [Reports](
[Id] INT PRIMARY KEY IDENTITY,
[CategoryId] INT FOREIGN KEY REFERENCES [Categories]([Id]) NOT NULL,
[StatusId] INT FOREIGN KEY REFERENCES [Status]([Id]) NOT NULL,
[OpenDate] DATETIME2 NOT NULL,
[CloseDate] DATETIME2 ,
[Description] VARCHAR(200) NOT NULL,
[UserId] INT FOREIGN KEY REFERENCES [Users]([Id]) NOT NULL,
[EmployeeId] INT FOREIGN KEY REFERENCES [Employees]([Id])

)

INSERT INTO [Employees]([FirstName],[LastName],[Birthdate],[DepartmentId])
VALUES
('Marlo','O''Malley','1958-9-21',1),
('Niki','Stanaghan','1969-11-26',4),
('Ayrton','Senna','1960-03-21',9),
('Ronnie','Peterson','1944-02-14',9),
('Giovanna','Amati','1959-07-20',5)

INSERT INTO [Reports]([CategoryId],[StatusId],[OpenDate],[CloseDate],[Description],[UserId],[EmployeeId])
VALUES
(1,1,'2017-04-13',NULL,'Stuck Road on Str.133',6,2),
(6,3,'2015-09-05','2015-12-06','Charity trail running',3,5),
(14,2,'2015-09-07',NULL,'Falling bricks on Str.58',5,2),
(4,3,'2017-07-03','2017-07-06','Cut off streetlight on Str.11',1,1)

UPDATE [Reports]
SET [CloseDate] = GETDATE()
WHERE [CloseDate] IS NULL

DELETE FROM [Reports]
WHERE [StatusId] =4

-- disable all constraints
EXEC sp_MSForEachTable "ALTER TABLE ? NOCHECK CONSTRAINT all"

-- delete data in all tables
EXEC sp_MSForEachTable "DELETE FROM ?"

-- enable all constraints
exec sp_MSForEachTable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"

SELECT [Description],FORMAT([OpenDate],'dd-MM-yyyy') FROM [Reports]
WHERE [EmployeeId] IS NULL
ORDER BY [OpenDate] ASC,[Description] ASC


SELECT r.[Description],c.[Name] FROM [Reports] AS r
LEFT JOIN [Categories] AS c
ON r.[CategoryId]=c.[Id]
ORDER BY r.[Description],c.[Name]

SELECT TOP(5) [CategoryName],[ReportsNumber] FROM (
				SELECT c.[Name] AS [CategoryName], 
						COUNT(r.[CategoryId]) AS [ReportsNumber] 
				FROM [Reports] AS r
				LEFT JOIN [Categories] AS c
				ON r.[CategoryId]=c.[Id]
				GROUP BY c.[Name]
				) AS [CategoryReportsSubQuery]
ORDER BY [ReportsNumber] DESC,[CategoryName]

SELECT u.[Username],c.[Name] FROM [Reports] AS r
LEFT JOIN [Categories] AS c
ON r.[CategoryId]=c.[Id]
LEFT JOIN [Users] AS u
ON u.[Id]=r.[UserId]
WHERE FORMAT(u.[Birthdate],'dd-MM')= FORMAT(r.[OpenDate],'dd-MM')
ORDER BY u.[Username],c.[Name]



SELECT CONCAT(e.[FirstName],' ',e.[LastName]) AS [FullName]
, COUNT(DISTINCT r.[UserId]) AS [UsersCount]
FROM [Reports] AS r
RIGHT JOIN [Employees] AS e
ON r.[EmployeeId]=e.[Id]
GROUP BY e.[FirstName],e.[LastName]
ORDER BY UsersCount DESC, FullName ASC



   SELECT ISNULL(e.[FirstName]+' '+e.[LastName],'None') AS [Employee] ,
		  ISNULL(d.[Name],'None') AS [Department],
		  ISNULL(c.[Name],'None') AS [Category],
		  r.[Description],
		  FORMAT(r.[OpenDate],'dd.MM.yyyy') AS [OpenDate],
		  s.[Label] AS [Status],
  		  ISNULL(u.[Name],'None') AS [User]
	 FROM [Reports] AS r
LEFT JOIN [Employees] AS e ON r.[EmployeeId]=e.[Id]
LEFT JOIN [Departments] AS d ON e.[DepartmentId]=d.[Id]
LEFT JOIN [Categories] AS c ON r.[CategoryId]=c.[Id]
LEFT JOIN [Status] AS s ON r.[StatusId]=s.[Id]
LEFT JOIN [Users] AS u ON r.[UserId]= u.[Id]
 ORDER BY e.[FirstName] DESC,
		  e.[LastName] DESC,
		  [Department] ASC,
		  [Category] ASC,
		  r.[Description] ASC,
		  [OpenDate] ASC,
		  [Status] ASC,
		  [User] ASC

GO
CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
AS
BEGIN
	DECLARE @Result INT
		IF @StartDate IS NULL OR @EndDate IS NULL
	BEGIN
	SET @Result=0
	END
		ELSE
	BEGIN
	SET @Result=DATEDIFF(HOUR,@StartDate,@EndDate)
	END
	RETURN @Result
END
GO
SELECT dbo.udf_HoursToComplete(OpenDate, CloseDate) AS TotalHours
   FROM Reports
GO

CREATE PROCEDURE usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT) 
AS
	DECLARE @CategoryID INT =(SELECT [CategoryId] FROM [Reports]
								WHERE [Id]=@ReportId)
	
	DECLARE @EmployeeDepartment INT=( SELECT [DepartmentId] FROM [Employees]
									WHERE [Id]=@EmployeeId)

	

	DECLARE @CategoryDepartment INT =(SELECT [DepartmentId] FROM [Categories]
										WHERE [Id]=@CategoryID)

	IF @EmployeeDepartment <> @CategoryDepartment
	THROW 50005,'Employee doesn''t belong to the appropriate department!',1;
	
	UPDATE [Reports]
	SET [EmployeeId]=@EmployeeId
	WHERE [Id]=@ReportId
GO

EXEC usp_AssignEmployeeToReport 30, 1

EXEC usp_AssignEmployeeToReport 17, 2