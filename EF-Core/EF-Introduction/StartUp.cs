using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext db = new SoftUniContext();
            string result = RemoveTown(db);
            Console.WriteLine(result);
        }
        //3.	Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
            var employees = context
                .Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString();
        }
        //4.	Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var emplpyees = context
                .Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var e in emplpyees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString();

        }
        //5.	Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        //6.	Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(address);
            Employee employeeNakov = context
                 .Employees
                 .First(e => e.LastName == "Nakov");
            employeeNakov.Address = address;
            context.SaveChanges();

            var employees = context
                .Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => new
                {
                    e.Address.AddressText
                })
                .Take(10)
                .ToArray();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var a in employees)
            {
                stringBuilder.AppendLine($"{a.AddressText}");
            }

            return stringBuilder.ToString().TrimEnd();
        }
        //7.	Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employeeProjects = context
                .Employees
                .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name,
                        StartDate = p.Project.StartDate,
                        EndDate = p.Project.EndDate
                    })

                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employeeProjects)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");
                foreach (var p in e.Projects)
                {
                    var startDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt");
                    var endDate = p.EndDate.HasValue ? p.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt"):"not finished";
                    sb.AppendLine($"--{p.ProjectName} - {startDate} - {endDate}");
                }
            }
            return sb.ToString().TrimEnd();
        }
        //8.	Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var adresses = context
                .Addresses
                .Select(a => new
                {
                    AdressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeesCount =a.Employees.Count
                })
                .OrderByDescending(e => e.EmployeesCount)
                .ThenBy(t => t.TownName)
                .ThenBy(a => a.AdressText)
                .Take(10)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var a in adresses)
            {
                sb.AppendLine($"{a.AdressText}, {a.TownName} - {a.EmployeesCount} employees");
            }
            return sb.ToString().TrimEnd();
        }
        //9.	Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context
                .Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select(pr => new
                    {
                        pr.Project.Name
                    })
                })
                .FirstOrDefault(e => e.EmployeeId == 147);


            StringBuilder sb = new StringBuilder();

           
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                foreach (var p in employee.Projects.OrderBy(x=>x.Name))
                {
                    sb.AppendLine($"{p.Name}");
                }
            
            return sb.ToString().TrimEnd();
        }
        //10.	Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var moreThanFive = context
                .Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    departmentName = x.Name,
                    managerName = $"{x.Manager.FirstName} {x.Manager.LastName}",
                    employees = x.Employees
                })
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var d in moreThanFive)
            {
                sb.AppendLine($"{d.departmentName} - {d.managerName}");
                foreach (var e in d.employees.OrderBy(e=>e.FirstName).ThenBy(x=>x.LastName))
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }
            return sb.ToString().TrimEnd();
        }
        //11.	Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {
            var tenLatestProjects = context
                .Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .ToArray();
            
            StringBuilder sb = new StringBuilder();
            foreach (var p in tenLatestProjects.OrderBy(x=>x.Name))
            {
                sb.AppendLine($"{p.Name}");
                sb.AppendLine($"{p.Description}");
                sb.AppendLine($"{p.StartDate:M/d/yyyy h:mm:ss tt}");
            }
            return sb.ToString().TrimEnd();
        }
        //12.	Increase Salaries
        public static string IncreaseSalaries(SoftUniContext context)
        {
            IQueryable<Employee> employeesToIncreaseSalary = context
                .Employees
                .Where(d => d.Department.Name == "Engineering" ||
                          d.Department.Name == "Tool Design" ||
                          d.Department.Name == "Marketing" ||
                          d.Department.Name == "Information Services");

            foreach (var e in employeesToIncreaseSalary)
            {
                e.Salary += e.Salary * 0.12m;
            }
            context.SaveChanges();

            var employeesToDisplay = employeesToIncreaseSalary
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employeesToDisplay)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }
        //13.	Find Employees by First Name Starting with "Sa"
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employeesStartingWithSA = context
                .Employees
                .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var e in employeesStartingWithSA)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");

            }
            return sb.ToString().TrimEnd();
        }
        //14.	Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {
            var deleteProject = context
                .Projects
                .Where(p => p.ProjectId == 2)
                .First();

            var referenceEmployeeProjects = context
                .EmployeesProjects
                .Where(e => e.ProjectId == 2);
            foreach (var ep in referenceEmployeeProjects)
            {
                context.EmployeesProjects.Remove(ep);
            }
            context.Projects.Remove(deleteProject);
            context.SaveChanges();

            var tenProjects = context
                .Projects
                .Select(p => p.Name)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var item in tenProjects)
            {
                sb.AppendLine(item);
            }

            return sb.ToString().TrimEnd();
        }
        //15.	Remove Town
        public static string RemoveTown(SoftUniContext context)
        {
            var employeesReference = context
                .Employees
                .Where(e => e.Address.Town.Name == "Seattle");
                
                
            foreach (var e in employeesReference)
            {
                e.AddressId = null;
            }


            var adressReference = context
                .Addresses
                .Where(a => a.Town.Name == "Seattle")
                .ToArray();
            foreach (var a in adressReference)
            {
                context.Addresses.Remove(a);
            }

            var townToDelete = context
                .Towns
                .Where(t => t.Name == "Seattle")
                .First();

            context.Towns.Remove(townToDelete);

            context.SaveChanges();

            return $"{adressReference.Length} addresses in Seattle were deleted";
        }
    }
}

