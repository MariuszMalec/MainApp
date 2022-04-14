using MainApp.BLL.Entities;
using System.ComponentModel.DataAnnotations;

namespace MainApp.BLL.Models
{
    public class UserView
    {
        [Required(ErrorMessage = "Please enter first name")]
        [MinLength(2)]
        [MaxLength(28)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide last name")]
        [MinLength(2)]
        [MaxLength(28)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Phone number")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{3}$", ErrorMessage = "Not a valid mobile phone number. Format ###-###-###")]
        public string? PhoneNumber { get; set; }
        [Display(Name = "User role")]
        public string UserRole { get; set; }
    }
}
