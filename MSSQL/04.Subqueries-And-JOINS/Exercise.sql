USE [SoftUni]
--1.	Employee Address
SELECT TOP(5) e.[EmployeeID], e.[JobTitle],e.[AddressID],a.AddressText FROM [Employees] AS e
LEFT JOIN [Addresses] AS a
ON e.AddressID=a.AddressID
ORDER BY e.AddressID

--2.	Addresses with Towns
SELECT TOP(50) e.[FirstName],e.[LastName],t.[Name],a.[AddressText] FROM [Employees] AS e
LEFT JOIN [Addresses] AS a 
ON e.AddressID=a.AddressID
LEFT JOIN [Towns] as t
ON a.TownID=t.TownID
ORDER BY e.[FirstName],e.[LastName]

--3.	Sales Employee
SELECT e.[EmployeeID] ,e.[FirstName],e.[LastName],d.[Name] FROM [Employees] AS e
LEFT JOIN [Departments] AS d
ON e.DepartmentID=d.DepartmentID
WHERE d.[Name] = 'Sales'

--4.	Employee Departments
SELECT TOP(5) e.[EmployeeID] ,e.[FirstName],e.[Salary],d.[Name] FROM [Employees] AS e
LEFT JOIN [Departments] AS d
ON e.DepartmentID=d.DepartmentID
WHERE e.[Salary] > 15000
ORDER BY e.DepartmentID

--5.	Employees Without Project
SELECT TOP(3) e.[EmployeeID],e.[FirstName] FROM [Employees] AS e
LEFT JOIN [EmployeesProjects] AS ep
ON e.EmployeeID=ep.EmployeeID
LEFT JOIN [Projects] as p
ON ep.ProjectID=p.ProjectID
WHERE ep.ProjectID IS NULL
ORDER BY e.EmployeeID 

--6.	Employees Hired After
SELECT e.[FirstName],e.[LastName],e.[HireDate],d.[Name] FROM [Employees] AS e
LEFT JOIN [Departments] AS d
ON e.DepartmentID=d.DepartmentID
WHERE e.[HireDate] > '1/1/1999' AND d.[Name] IN ('Sales','Finance')
ORDER BY e.[HireDate]

--7.	Employees with Project
SELECT TOP(5) e.[EmployeeID],e.[FirstName],p.[Name] FROM [Employees] AS e
INNER JOIN [EmployeesProjects] AS ep
ON e.[EmployeeID]=ep.EmployeeID
LEFT JOIN [Projects] AS p
ON ep.ProjectID=p.ProjectID
WHERE p.[StartDate] > '2002/08/13' AND p.[EndDate] IS NULL
ORDER BY e.[EmployeeID]

--8.	Employee 24
SELECT e.[EmployeeID],e.[FirstName],
CASE
		WHEN YEAR(p.[StartDate]) >=2005 THEN NULL
		ELSE p.[Name]
END AS [ProjectName]
FROM [Employees] AS e
INNER JOIN [EmployeesProjects] AS ep
ON e.[EmployeeID] =ep.[EmployeeID]
INNER JOIN [Projects] AS p
ON ep.[ProjectID]=p.[ProjectID]
WHERE e.[EmployeeID]=24

--9.	Employee Manager
SELECT e.[EmployeeID],e.[FirstName],e.[ManagerID],em.[FirstName] AS [ManagerName] 
FROM [Employees] AS e
INNER JOIN [Employees] AS em
ON e.[ManagerID]=em.[EmployeeID]
WHERE e.[ManagerID] IN (3,7)
ORDER BY e.[EmployeeID]

--10. Employee Summary
SELECT TOP(50) e.[EmployeeID]
	   ,CONCAT(e.[FirstName]+' ',e.[LastName] ) AS [EmployeeName]
	   ,CONCAT(em.[FirstName]+' ',em.[LastName]) AS [ManagerName] 
	   ,d.[Name] AS [DepartmentName]
FROM [Employees] AS e
INNER JOIN [Employees] AS em
ON e.[ManagerID]=em.[EmployeeID]
INNER JOIN [Departments] as d
ON e.[DepartmentID]=d.[DepartmentID]
ORDER BY e.[EmployeeID]

--11. Min Average Salary
SELECT MIN(a.MinSalary) [MinAverageSalary] FROM (
										SELECT DepartmentID ,AVG([Salary]) AS [MinSalary] 
										FROM [Employees]
										GROUP BY DepartmentID
										) AS a

USE [Geography]

--12. Highest Peaks in Bulgaria
SELECT * FROM(
				SELECT c.[CountryCode],m.[MountainRange],p.[PeakName],p.[Elevation] FROM [Countries] AS c
				INNER JOIN [MountainsCountries] AS mc
				ON c.CountryCode=mc.CountryCode
				LEFT JOIN [Mountains] AS m
				ON m.Id=mc.MountainId
				LEFT JOIN [Peaks] AS p
				ON p.MountainId=m.Id
				WHERE c.[CountryName]='Bulgaria'
				) AS BgMountains
WHERE BgMountains.Elevation > 2835
ORDER BY BgMountains.[Elevation] DESC

--13. Count Mountain Ranges
SELECT c.[CountryCode],COUNT(m.[MountainRange]) FROM [Countries] AS c
				INNER JOIN [MountainsCountries] AS mc
				ON c.CountryCode=mc.CountryCode
				LEFT JOIN [Mountains] AS m
				ON m.Id=mc.MountainId
				WHERE c.[CountryCode] IN ('BG','RU','US')
				GROUP BY c.[CountryCode]

--14. Countries with Rivers
SELECT c.[CountryName],r.[RiverName] FROM [Countries] AS c
LEFT JOIN [Continents] AS cn
ON c.[ContinentCode]=cn.ContinentCode
LEFT JOIN [CountriesRivers] AS cr
ON c.[CountryCode]=cr.[CountryCode]
LEFT JOIN [Rivers] AS r
ON cr.[RiverId]=r.[Id]
WHERE cn.ContinentName= 'Africa'
ORDER BY c.[CountryName]

--15. *Continents and Currencies
SELECT [ContinentCode]
	   ,[CurrencyCode],
	   [CurrencyCount] AS [CurrencyUsage]
FROM(
	SELECT *,
	DENSE_RANK() OVER(PARTITION BY [ContinentCode] ORDER BY [CurrencyCount] DESC) AS [CurrencyRank]
	FROM 
	(
			SELECT [ContinentCode],[CurrencyCode],COUNT([CurrencyCode]) AS [CurrencyCount] 
			FROM [Countries]
			GROUP BY [ContinentCode], [CurrencyCode]
	) AS [CurrencySubQuery]
	WHERE [CurrencyCount] > 1
) AS [rankTable]
WHERE [CurrencyRank] = 1
ORDER BY [ContinentCode]


--Countries Without Any Mountains
SELECT COUNT(*) AS [Count] FROM (
				SELECT mc.CountryCode AS [Count] FROM [Countries] AS c
				LEFT JOIN [MountainsCountries] AS mc
				ON c.[CountryCode]=mc.[CountryCode]
				WHERE mc.[MountainId] IS NULL
) AS [CountriesWIthoutMountain]

--16. Highest Peak and Longest River by Country
SELECT TOP(5) * FROM
(
SELECT c.[CountryName],
	   MAX(p.Elevation) AS [HighestPeakElevation] ,
	   MAX(r.[Length]) AS [LongestRiverLengt]
  FROM [Countries] AS c
  LEFT JOIN [MountainsCountries] AS mc
    ON c.[CountryCode]=mc.[CountryCode]
  LEFT JOIN [Mountains] AS m
    ON m.[Id]=mc.[MountainId]
  LEFT JOIN [Peaks] AS p
    ON p.[MountainId]=m.[Id]
  LEFT JOIN [CountriesRivers] AS cr
	ON c.[CountryCode]=cr.[CountryCode]
  LEFT JOIN [Rivers] AS r
	ON cr.[RiverId]=r.[Id]
 GROUP BY c.[CountryName]
 ) AS [RiverPeaks]
 ORDER BY [HighestPeakElevation] DESC,[LongestRiverLengt] DESC