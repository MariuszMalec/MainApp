using System.ComponentModel.DataAnnotations;

namespace MainApp.WebAppMvc.Models
{
    public class TrainerView : Entity
    {
        [Required(ErrorMessage = "Please enter first name")]
        [MinLength(2)]
        [MaxLength(28)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide last name")]
        [MinLength(2)]
        [MaxLength(28)]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Phone")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{3}$", ErrorMessage = "Not a valid mobile phone number. Format ###-###-###")]
#nullable enable
        public string? PhoneNumber { get; set; }
#nullable disable
    }
}
