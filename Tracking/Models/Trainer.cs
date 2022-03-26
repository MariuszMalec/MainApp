namespace Tracking.Models
{
    public class Trainer : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
