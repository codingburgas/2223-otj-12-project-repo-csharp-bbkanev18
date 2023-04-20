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
    public class ManageUserTransferObject
    {
        public string? Id { get; set; } = string.Empty;

        [Display(Name = "FirstName")]
        [Required(ErrorMessage = "Първото име е задължително")]
        [MinLength(2, ErrorMessage = "Първото име трябва да съдържа поне 2 знака")]
        [MaxLength(250, ErrorMessage = "Първото име не може да бъде повече от 250 знака")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "MiddleName")]
        [Required(ErrorMessage = "Бащиното име е задължително")]
        [MinLength(2, ErrorMessage = "Бащиното име трябва да съдържа поне 2 знака")]
        [MaxLength(250, ErrorMessage = "Бащиното име не може да бъде повече от 250 знака")]
        public string MiddleName { get; set; } = string.Empty;

        [Display(Name = "LastName")]
        [Required(ErrorMessage = "Фамилията е задължителна")]
        [MinLength(2, ErrorMessage = "Фамилията трябва да съдържа поне 2 знака")]
        [MaxLength(250, ErrorMessage = "Фамилията не може да бъде повече от 250 знака")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Изисква се имейл")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Невалиден имейл адрес")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Изисква се парола")]
        [MinLength(8, ErrorMessage = "Паролата трябва да е поне 8 знака")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Паролата трябва да съдържа поне една главна буква, една малка буква, едно число и един специален знак")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Изисква се парола")]
        [Compare("Password", ErrorMessage = "Паролата не съвпадат")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Age")]
        [Required(ErrorMessage = "Изисква се възраст")]
        [Range(14, 100, ErrorMessage = "Възрастта трябва да е между 14 и 100 години")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Възрастта трябва да е цяло число")]
        public byte Age { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; } = string.Empty;

        [Display(Name = "Phone")]
        [RegularExpression(@"^((\+?359)|0)?8[789]\d{7}$", ErrorMessage = "Моля, въведете валиден български телефонен номер. Пример: +359888123456")]
        public string? Phone { get; set; } = string.Empty;

        public string? RoleId { get; set; } = string.Empty;

        public string? RoleName { get; set; } = string.Empty;
    }
}
