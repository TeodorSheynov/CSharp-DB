using System.ComponentModel.DataAnnotations.Schema;
using Castle.Components.DictionaryAdapter;

namespace TeisterMask.Data.Models
{
    public class EmployeeTask
    {
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey(nameof(Task))]
        public int TaskId { get; set; }
        public Task Task { get; set; }

    }
}