##
# Exercises: CRUD

This document defines the **exercise assignments** for the [&quot;Databases Basics - MSSQL&quot; course @ Software University.](https://softuni.bg/trainings/3491/ms-sql-september-2021)

## 1.Examine the Databases

Download and get familiar with the **SoftUni** , **Diablo** and **Geography** database schemas and tables. You will use them in the current and following exercises to write queries.

## Part I – Queries for SoftUni Database

## 2.Find All Information About Departments

Create an SQL query that finds **all available information about the Departments.**

### Example

| **DepartmentID** | **Name** | **ManagerID** |
| --- | --- | --- |
| 1 | Engineering | 12 |
| 2 | Tool Design | 4 |
| 3 | Sales | 273 |
| … | … | … |

## 3.Find all Department Names

Create an SQL query that finds **all Department names**.

### Example

| **Name** |
| --- |
| Engineering |
| Tool Design |
| Sales |
| … |

## 4.Find Salary of Each Employee

Create an SQL query that finds the **first name** , **last name,** and **salary** for each employee.

### Example

| **FirstName** | **LastName** | **Salary** |
| --- | --- | --- |
| Guy | Gilbert | 12500.00 |
| Kevin | Brown | 13500.00 |
| Roberto | Tamburello | 43300.00 |
| … | … | … |

## 5.Find Full Name of Each Employee

Create an SQL query that finds the **first** , **middle,** and **last name** for each employee.

### Example

| **FirstName** | **MiddleName** | **LastName** |
| --- | --- | --- |
| Guy | R | Gilbert |
| Kevin | F | Brown |
| Roberto | NULL | Tamburello |
| … | … | … |

## 6.Find Email Address of Each Employee

Create an SQL query that finds the **email address** for each employee, by his **first and last name**. Consider that the email domain is **softuni.bg**. Emails should look like &quot;John.Doe@softuni.bg&quot;. The **produced column** should be named **&quot;Full Email Address&quot;**.

### Example

| **Full Email Address** |
| --- |
| Guy.Gilbert@softuni.bg |
| Kevin.Brown@softuni.bg |
| Roberto.Tamburello@softuni.bg |
| … |

## 7.Find All Different Employee&#39;s Salaries

Create an SQL query that finds **all different employee&#39;s salaries**. Display the salaries only in a column named &quot; **Salary**&quot;.

### Example

| **Salary** |
| --- |
| 9000.00 |
| 9300.00 |
| 9500.00 |
| … |

## 8.Find all Information About Employees

Create an SQL query that finds **all information** about the employees whose **job title** is &quot; **Sales Representative&quot;.**

### Example

| **ID** | **First Name**|**Last Name** | **Middle Name**|**Job Title**|**DeptID**|**Mngr ID** | **HireDate** | **Salary** | **AddressID** |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 275 | Michael | Blythe | G | Sales Representative | 3 | 268 | … | 23100.00 | 60 |
| 276 | Linda | Mitchell | C | Sales Representative | 3 | 268 | … | 23100.00 | 170 |
| 277 | Jillian | Carson | NULL | Sales Representative | 3 | 268 | … | 23100.00 | 61 |
| … | … | … | … | … | … | … | … | … | … |

## 9.Find Names of All Employees by Salaryin Range

Create an SQL query to find the **first name** , **last name** , and **job title** for all employees whose salary is in a **range** between **20000** and **30000**.

### Example

| **FirstName** | **LastName** | **JobTitle** |
| --- | --- | --- |
| Rob | Walters | Senior Tool Designer |
| Thierry | D&#39;Hers | Tool Designer |
| JoLynn | Dobney | Production Supervisor |
| … | … | … |

## 10. Find Names of All Employees

Create an SQL query that finds the  **full name**  of all employees whose  **salary**  is exactly  **25000, 14000, 12500, or 23600**. The result should be displayed in a column named &quot;Full Name&quot;, which is a combination of  **first** ,  **middle** , and  **last**  names separated by a  **single space**.

### Example

| **Full Name** |
| --- |
| Guy R Gilbert |
| Thierry B D&#39;Hers |
| JoLynn M Dobney |

## 11. Find All Employees Without Manager

Create an SQL query that finds  **the first and last names**  of those employees that  **do not have a manager**.

### Example

| **FirstName** | **LastName** |
| --- | --- |
| Ken | Sanchez |
| Svetlin | Nakov |
| … | … |

## 12. Find All Employees with Salary More Than 50000

Create an SQL query that finds  **the first name** ,  **last name** , and  **salary**  for employees with  **a salary**  higher than 50000. Order the result in decreasing order by salary.

### Example

| **FirstName** | **LastName** | **Salary** |
| --- | --- | --- |
| Ken | Sanchez | 125500.00 |
| James | Hamilton | 84100.00 |
| … | … | … |

## 13. Find 5 Best Paid Employees.

Create an SQL query that finds the  **first and last names** of the  **5 best-paid Employees,**  ordered in  **descending by their salary.**

### Example

| **FirstName** | **LastName** |
| --- | --- |
| Ken | Sanchez |
| James | Hamilton |
| … | … |

## 14.Find All Employees Except Marketing

Create an SQL query that finds the  **first** and **last names**  of all employees whose  **department ID is not 4.**

### Example

| **FirstName** | **LastName** |
| --- | --- |
| Guy | Gilbert |
| Roberto | Tamburello |
| Rob | Walters |

## 15.Sort Employees Table

Create an Write a SQL query that sorts all records in the Employees table by the following criteria:

- By  **salary**  in  **decreasing**  order
- Then by the  **first name**   **alphabetically**
- Then by the  **last name descending**
- Then by  **middle name alphabetically**

### Example

| **ID** | **First Name**|**Last Name** | **Middle Name**|**Job Title**|**DeptID**|**Mngr ID** | **HireDate** | **Salary** | **AddressID** |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 109 | Ken | Sanchez | J | Chief Executive Officer | 16 | NULL | … | 125500.00 | 177 |
| 148 | James | Hamilton | R | Vice President of Production | 7 | 109 | … | 84100.00 | 158 |
| 273 | Brian | Welcker | S | Vice President of Sales | 3 | 109 | … | 72100.00 | 134 |
| … | … | … | … | … | … | … | … | … | … |

## 16. Create View Employees with Salaries

Create an SQL query that creates a view &quot; **V\_EmployeesSalaries&quot;**  with  **first name** ,  **last name** , and  **salary**  for each employee.

### Example

| **FirstName** | **LastName** | **Salary** |
| --- | --- | --- |
| Guy | Gilbert | 12500.00 |
| Kevin | Brown | 13500.00 |
| … | … | … |

## 17.Create View Employees with Job Titles

Create an SQL query to create view "**V\_EmployeeNameJobTitle&quot;** with  **full employee name**  and  **job title**. When the middle name is  **NULL**  replace it with **an empty string (&#39;&#39;)**.

### Example

| **Full Name** | **Job Title** |
| --- | --- |
| Guy R Gilbert | Production Technician |
| Kevin F Brown | Marketing Assistant |
| Roberto Tamburello | Engineering Manager |
| … | … |

## 18. Distinct Job Titles

Create an SQL query that finds  **all distinct job titles**.

### Example

| **JobTitle** |
| --- |
| Accountant |
| Accounts Manager |
| Accounts Payable Specialist |
| … |

## 19.Find First 10 Started Projects

Create an SQL query that finds  **the first 10 projects which were started** , select  **all information about them** , and  **sort** the result by  **starting date** ,  **then by name**.

### Example

| **ID** | **Name** | **Description** | **StartDate** | **EndDate** |
| --- | --- | --- | --- | --- |
| 6 | HL Road Frame | Research, design and development of HL Road … | 1998-05-02 00:00:00 | 2003-06-01 00:00:00 |
| 2 | Cycling Cap | Research, design and development of C… | 2001-06-01 00:00:00 | 2003-06-01 00:00:00 |
| 5 | HL Mountain Frame | Research, design and development of HL M… | 2001-06-01 00:00:00 | 2003-06-01 00:00:00 |
| … | … | … | … | … |

## 20. Last 7 Hired Employees

Create an SQL query that finds  **the last 7 hired employees, select**   **their first, last name, and hire date**.

### Example

| **FirstName** | **LastName** | **HireDate** |
| --- | --- | --- |
| Rachel | Valdez | 2005-07-01 00:00:00 |
| Lynn | Tsoflias | 2005-07-01 00:00:00 |
| Syed | Abbas | 2005-04-15 00:00:00 |
| … | … | … |

## 21.Increase Salaries

Create an SQL query that increases salaries by  **12%**  of all employees that work in the ether  **Engineering** ,  **Tool Design** ,  **Marketing** , or  **Information Services**  departments. As a result, select and display **only the &quot;Salaries&quot; column**  from the  **Employees**  table. After that exercise, you should restore the database to the original data.

### Example

| **Salary** |
| --- |
| 12500.00 |
| 15120.00 |
| 48496.00 |
| 33376.00 |
| … |





