using SchoolSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class CourseSectionTransferObject
    {
        public string Id { get; set; } = string.Empty;
        [Display(Name = "SectionName")]
        [Required(ErrorMessage = "Името на раздела е задължително")]
        [MinLength(2, ErrorMessage = "Името на раздела трябва да е поне 2 знака")]
        [MaxLength(250, ErrorMessage = "Името на раздела не може да бъде повече от 250 знака")]
        public string Name { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public List<Test> Tests { get; set; } = new List<Test>();
        public List<Models.File> Files { get; set; } = new List<Models.File>();
    }
}
