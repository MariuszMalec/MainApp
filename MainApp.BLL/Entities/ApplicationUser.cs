using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainApp.BLL.Entities
{
    [Table("Users")]
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
        public DateTime Created { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserRole { get; set; }
    }
}
