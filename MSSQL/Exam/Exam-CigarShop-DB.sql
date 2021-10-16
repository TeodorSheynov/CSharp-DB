CREATE DATABASE [CigarShop]

USE [CigarShop]

CREATE TABLE Sizes
(
Id INT PRIMARY KEY IDENTITY,
[Length] INT NOT NULL,
CHECK([Length] BETWEEN 10 AND 25),
RingRange DECIMAL(2,1),
CHECK(RingRange BETWEEN 1.5 AND 7.5)
)

CREATE TABLE Tastes
(
Id INT PRIMARY KEY IDENTITY,
TasteType VARCHAR(20) NOT NULL,
TasteStrength VARCHAR(15) NOT NULL,
ImageURL NVARCHAR(100) NOT NULL
)

CREATE TABLE Brands
(
Id INT PRIMARY KEY IDENTITY,
BrandName VARCHAR(30) UNIQUE NOT NULL,
BrandDescription VARCHAR(MAX)
)

CREATE TABLE Cigars
(
Id INT PRIMARY KEY IDENTITY,
CigarName VARCHAR(80) NOT NULL,
BrandId INT FOREIGN KEY REFERENCES Brands(Id) NOT NULL,
TastId INT FOREIGN KEY REFERENCES Tastes(Id) NOT NULL,
SizeId INT FOREIGN KEY REFERENCES Sizes(Id) NOT NULL,
PriceForSingleCigar DECIMAL(20,2) NOT NULL,
ImageURL NVARCHAR(100) NOT NULL
)

CREATE TABLE Addresses
(
Id INT PRIMARY KEY IDENTITY,
Town VARCHAR(30) NOT NULL,
Country NVARCHAR(30) NOT NULL,
Streat NVARCHAR(100) NOT NULL,
ZIP VARCHAR(20) NOT NULL
)

CREATE TABLE Clients(
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(30) NOT NULL,
LastName NVARCHAR(30) NOT NULL,
Email NVARCHAR(50) NOT NULL,
AddressId INT FOREIGN KEY REFERENCES Addresses(Id) NOT NULL
)

CREATE TABLE ClientsCigars(
ClientId INT FOREIGN KEY REFERENCES Clients(Id) NOT NULL,
CigarId INT FOREIGN KEY REFERENCES Cigars(Id) NOT NULL
PRIMARY KEY (ClientId,CigarId)
)

INSERT INTO Cigars(CigarName,BrandId,TastId,SizeId,PriceForSingleCigar,ImageURL)
VALUES
('COHIBA ROBUSTO',9,1,5,15.50,'cohiba-robusto-stick_18.jpg'),
('COHIBA SIGLO I',9,1,10,410.00,'cohiba-siglo-i-stick_12.jpg'),
('HOYO DE MONTERREY LE HOYO DU MAIRE',14,5,11,7.50,'hoyo-du-maire-stick_17.jpg'),
('HOYO DE MONTERREY LE HOYO DE SAN JUAN',14,4,15,32.00,'hoyo-de-san-juan-stick_20.jpg'),
('TRINIDAD COLONIALES',2,3,8,85.21,'trinidad-coloniales-stick_30.jpg')

INSERT INTO Addresses(Town,Country,Streat,ZIP)
VALUES
('Sofia','Bulgaria','18 Bul. Vasil levski',1000),
('Athens','Greece','4342 McDonald Avenue',10435),
('Zagreb','Croatia','4333 Lauren Drive',10000)

UPDATE Cigars
SET PriceForSingleCigar*=1.2
WHERE TastId =1

UPDATE Brands
SET BrandDescription='New description'
WHERE BrandDescription IS NULL


DELETE FROM Clients
WHERE [Id] IN(
SELECT Id FROM Clients
WHERE AddressId IN (SELECT Id FROM Addresses
WHERE LEFT(Country,1)='C')
)

DELETE FROM Addresses
WHERE LEFT(Country,1)='C'



SELECT* FROM ClientsCigars
WHERE ClientId=14


-- disable all constraints
EXEC sp_MSForEachTable "ALTER TABLE ? NOCHECK CONSTRAINT all"

-- delete data in all tables
EXEC sp_MSForEachTable "DELETE FROM ?"

-- enable all constraints
exec sp_MSForEachTable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"

SELECT CigarName,PriceForSingleCigar,ImageURL FROM Cigars
ORDER BY PriceForSingleCigar,CigarName DESC

SELECT c.Id,c.CigarName,c.PriceForSingleCigar,t.TasteType,t.TasteStrength FROM Cigars AS c
LEFT JOIN Tastes AS t ON c.TastId=t.Id
WHERE t.TasteType IN ('Earthy','Woody')
ORDER BY c.PriceForSingleCigar DESC

SELECT Id,CONCAT(FirstName,' ',LastName),Email FROM Clients
WHERE Id NOT IN (SELECT DISTINCT ClientId FROM ClientsCigars)
ORDER BY FirstName,LastName

SELECT TOP(5) c.CigarName,c.PriceForSingleCigar,c.ImageURL FROM Cigars AS c
LEFT JOIN Sizes AS s ON c.SizeId=s.Id
WHERE  s.[Length] >12 AND (c.CigarName LIKE '%ci%' OR c.PriceForSingleCigar>50) AND s.RingRange>2.55
ORDER BY c.CigarName,c.PriceForSingleCigar DESC

SELECT CONCAT(c.FirstName,' ',c.LastName) AS FullName
	   ,a.Country
	   ,a.ZIP
	   ,FORMAT(MAX(cig.PriceForSingleCigar),'C','en-us') AS CigarPrice
	   FROM Clients AS c
LEFT JOIN Addresses AS a ON c.AddressId=a.Id
LEFT JOIN ClientsCigars AS cs ON c.Id=cs.ClientId
LEFT JOIN Cigars AS cig ON cig.Id=cs.CigarId
WHERE a.ZIP NOT LIKE '%[^0-9]%'
GROUP BY c.FirstName,c.LastName,a.Country, a.ZIP
ORDER BY c.FirstName,c.LastName

SELECT LastName,AVG([Length])AS CiagrLength,CEILING(AVG(RingRange)) AS CiagrRingRange FROM(
SELECT c.LastName,s.[Length] ,s.RingRange FROM ClientsCigars AS cs
JOIN [Clients] AS c ON cs.[ClientId]=c.Id
JOIN Cigars AS cig ON cs.CigarId=cig.Id
JOIN Sizes AS s ON cig.SizeId=s.Id
) as SubQ
GROUP BY LastName
ORDER BY CiagrLength DESC


SELECT * FROM [Clients]

GO
CREATE OR ALTER FUNCTION udf_ClientWithCigars(@name NVARCHAR(30))
RETURNS INT
AS
BEGIN
		DECLARE @Result INT
		SET @Result =(SELECT COUNT(*) as ID FROM Clients AS c
								LEFT JOIN ClientsCigars AS cs ON c.Id=cs.ClientId
								WHERE c.FirstName=@name
								)
		RETURN @Result
END
GO

SELECT dbo.udf_ClientWithCigars('Betty')
GO


CREATE PROCEDURE usp_SearchByTaste(@taste VARCHAR(20))
AS
SELECT c.CigarName,
       CONCAT('$',c.PriceForSingleCigar) AS Price,
	   t.TasteType,
	   b.BrandName,
	   CONCAT(s.[Length],' cm') AS CigarLength,
	   CONCAT(s.RingRange,' cm') AS CigarRingRange
FROM Cigars AS c
LEFT JOIN Tastes AS t ON c.TastId=t.Id
LEFT JOIN Brands AS b ON b.Id=c.BrandId
LEFT JOIN Sizes AS s ON s.Id=c.SizeId
WHERE t.TasteType=@taste
ORDER BY CigarLength,CigarRingRange DESC
GO

EXEC usp_SearchByTaste 'Woody'