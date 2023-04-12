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
        [Required(ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage = "The firstname must be at least 2 characters")]
        [MaxLength(250, ErrorMessage = "The firstname cannot be more than 250 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "MiddleName")]
        [Required(ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage = "The middlename must be at least 2 characters")]
        [MaxLength(250, ErrorMessage = "The middlename cannot be more than 250 characters")]
        public string MiddleName { get; set; } = string.Empty;

        [Display(Name = "LastName")]
        [Required(ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage = "The lastname must be at least 2 characters")]
        [MaxLength(250, ErrorMessage = "The lastname cannot be more than 250 characters")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "The Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Age")]
        [Required(ErrorMessage = "Age is required.")]
        [Range(14, 100, ErrorMessage = "The age must be between 14 and 100.")]
        public byte Age { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; } = string.Empty;

        [Display(Name = "Phone")]
        [RegularExpression(@"^((\+?359)|0)?8[789]\d{7}$", ErrorMessage = "Please enter a valid Bulgarian phone number. Example: +359888123456")]
        public string? Phone { get; set; } = string.Empty;

        public string? RoleId { get; set; } = string.Empty;

        public string? RoleName { get; set; } = string.Empty;
    }
}
