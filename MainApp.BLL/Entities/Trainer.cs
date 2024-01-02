using System;

namespace MainApp.BLL.Entities
{
    public  class Trainer : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
#nullable enable
        public string? PhoneNumber { get; set; }
        public string? TrainerPicture { get; set; }
#nullable disable
    }
}
