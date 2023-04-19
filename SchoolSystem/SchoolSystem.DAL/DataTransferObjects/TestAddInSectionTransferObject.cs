using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class TestAddInSectionTransferObject
    {
        public string Id { get; set; } = string.Empty;
        [Display(Name = "Test name")]
        [Required(ErrorMessage = "Test name is required.")]
        [MinLength(2, ErrorMessage = "The test name must be at least 2 characters")]
        [MaxLength(250, ErrorMessage = "The test name cannot be more than 250 characters")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Time limit")]
        [Required(ErrorMessage = "Time limit is required.")]
        [Range(1, 30, ErrorMessage = "The time must be between 1 and 30 minutes.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Time limit must be an integer value.")]
        public int TimeLimit { get; set; }
        public DateTime? Deadline { get; set; }
        public string SectionId { get; set; } = string.Empty;
        public string SectionName { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
    }
}
