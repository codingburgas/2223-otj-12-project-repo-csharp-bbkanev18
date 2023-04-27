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
    public class CreateQuestionTransferObject
    {
        public Test Test { get; set; } = new Test();

        [Required(ErrorMessage = "Името на въпроса е задължително")]
        [MinLength(2, ErrorMessage = "Името на въпроса трябва да е поне 2 знака")]
        [MaxLength(1000, ErrorMessage = "Името на въпроса не може да бъде повече от 1000 знака")]
        public string QuestionName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Изисква се точки")]
        [Range(1, 10, ErrorMessage = "Може да сложите на въпрос между 1 до 10 точки")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Точките трябва да е цяло число")]
        public int Points { get; set; } = 1;


        [Required(ErrorMessage = "Изисква се верен отговор")]
        [MinLength(2, ErrorMessage = "Отговора трябва да е поне 2 знака")]
        [MaxLength(250, ErrorMessage = "Отговора на въпроса не може да бъде повече от 1000 знака")]
        public string CorrectAnswer { get; set; } = string.Empty;
        
        public List<string>? Answers { get; set; } = null;
    }
}
