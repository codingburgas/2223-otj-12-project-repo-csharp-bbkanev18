using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class CourseCreateTransferObject
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "The name of course is required")]
        [MinLength(2, ErrorMessage = "The name must be at least 2 characters")]
        [MaxLength(250, ErrorMessage = "The name cannot be more than 250 characters")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "SectionName")]
        [MinLength(2, ErrorMessage = "The section name must be at least 2 characters")]
        [MaxLength(250, ErrorMessage = "The section name cannot be more than 250 characters")]
        public List<string>? SectionName { get; set; }
    }
}
