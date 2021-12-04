using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using TeisterMask.Data.Models;
using TeisterMask.Data.Models.Enums;
using TeisterMask.DataProcessor.ImportDto;

namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;

    using Data;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Projects");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportProjectsDto[]), root);
            using StringReader sr = new StringReader(xmlString);

            var projectsDto =(ImportProjectsDto[]) serializer.Deserialize(sr);


            var projects = new HashSet<Project>();

            foreach (var project in projectsDto)
            {
                if (!IsValid(project))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isOpenDateValid = DateTime.TryParseExact(project.OpenDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None
                    , out DateTime OpenDate);
                if (!isOpenDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? DueDate = null;
                if (!String.IsNullOrWhiteSpace(project.DueDate))
                {
                    bool isDueDateValid = DateTime.TryParseExact(project.DueDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None
                        , out DateTime date);
                    if (!isDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DueDate = date;
                }

                Project pro = new Project()
                {
                    DueDate = DueDate,
                    Name = project.Name,
                    OpenDate = OpenDate,
                };
                var tasks = new HashSet<Task>();
                foreach (TasksDto projectTask in project.Tasks)
                {
                    if (!IsValid(projectTask))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isTaskOpenDateValid = DateTime.TryParseExact(projectTask.OpenDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None
                        , out DateTime OpenDateTask);
                    if (!isTaskOpenDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isTaskDueDateValid = DateTime.TryParseExact(projectTask.DueDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None
                        , out DateTime DueDateTask);
                    if (!isTaskDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (pro.OpenDate > OpenDateTask)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (DueDateTask > pro.DueDate && pro.DueDate.HasValue)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task()
                    {
                        Name = projectTask.Name,
                        OpenDate = OpenDateTask,
                        DueDate = DueDateTask,
                        ExecutionType = (ExecutionType) projectTask.ExecutionType,
                        LabelType = (LabelType) projectTask.LabelType
                    };
                    tasks.Add(task);
                }

                pro.Tasks = tasks;
                projects.Add(pro);
                sb.AppendLine(String.Format(SuccessfullyImportedProject, pro.Name, pro.Tasks.Count));
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportEmployeeDto[] dto = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString);
            var eEmployees = new HashSet<Employee>();
            foreach (var employeeDto in dto)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee e = new Employee()
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone
                };

                var tasks = new HashSet<EmployeeTask>();
                foreach (var employeeDtoTask in employeeDto.Tasks.Distinct())
                {
                    Task task = context
                        .Tasks
                        .Find(employeeDtoTask);

                    if (task == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    EmployeeTask et = new EmployeeTask()
                    {
                        Employee = e,
                        Task = task
                    };

                    tasks.Add(et);

                }

                e.EmployeesTasks = tasks;
                eEmployees.Add(e);
                sb.AppendLine(String.Format(SuccessfullyImportedEmployee, e.Username, e.EmployeesTasks.Count));
            }
            context.Employees.AddRange(eEmployees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}