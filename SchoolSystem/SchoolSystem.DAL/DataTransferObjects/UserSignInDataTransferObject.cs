using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SchoolSystem.DAL.DataTransferObjects
{
    public class UserSignInDataTransferObject
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Изисква се имейл.")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Невалиден имейл адрес")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Изисква се парола.")]
        [MinLength(8, ErrorMessage = "Невалидна парола")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Невалидна парола")]
        public string Password { get; set; } = string.Empty;


    }
}
