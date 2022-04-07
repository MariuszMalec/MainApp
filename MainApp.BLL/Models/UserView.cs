using MainApp.BLL.Entities;
using System.ComponentModel.DataAnnotations;

namespace MainApp.BLL.Models
{
    public class UserView
    {
        [Required(ErrorMessage = "Please enter first name")]
        [MinLength(2)]
        [MaxLength(28)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide last name")]
        [MinLength(2)]
        [MaxLength(28)]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
