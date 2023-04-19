using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class FileAddInSectionTransferObject
    {
        public string Id { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please upload file.")]
        public IFormFile File { get; set; }
        public string SectionId { get; set; } = string.Empty;
        public string SectionName { get; set; } = string.Empty;
        public string CourseId { get; set; } = string.Empty;
    }
}
