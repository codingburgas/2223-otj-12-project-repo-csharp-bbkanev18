using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class CourseCreateTransferObject
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Името на курса е задължително")]
        [MinLength(2, ErrorMessage = "Името трябва да съдържа поне 2 знака")]
        [MaxLength(250, ErrorMessage = "Името не може да бъде повече от 250 знака")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "SectionName")]
        [MinLength(2, ErrorMessage = "Името на раздела трябва да е поне 2 знака")]
        [MaxLength(250, ErrorMessage = "Името на раздела не може да бъде повече от 250 знака")]
        public List<string>? SectionName { get; set; }
    }
}
