using MainApp.BLL.Entities;

namespace MainApp.BLL.Models
{
    public class TrainerView : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
