using MainApp.BLL.Entities;
using System.ComponentModel.DataAnnotations;

namespace MainApp.BLL.Models
{
    public class TrainerView : Entity
    {
        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(25)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide last name")]
        [StringLength(25)]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
