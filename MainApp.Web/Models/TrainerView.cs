using MainApp.BLL.Entities;

namespace MainApp.Web.Models
{
    public class TrainerView : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
