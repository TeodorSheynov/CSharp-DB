namespace TeisterMask.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class ImportEmployeeDto
    {
        [Required]
        [MaxLength(40)]
        [MinLength(3)]
        [RegularExpression(@"^[A-Za-z0-9]+$")]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"\d{3}-\d{3}-\d{4}")]
        public string Phone { get; set; }
        public int[] Tasks { get; set; }

    }

    
}
