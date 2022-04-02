using MainApp.BLL.Entities;
using System.ComponentModel.DataAnnotations;

namespace MainApp.BLL.Models
{
    public class RegisterView
    {
        [Required(ErrorMessage = "Please enter first name")]
        [MinLength(2)]
        [MaxLength(28)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide last name")]
        [MinLength(2)]
        [MaxLength(28)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
