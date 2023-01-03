using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainApp.BLL.Entities;
using Microsoft.AspNetCore.Identity;

namespace MainApp.BLL.Models
{
    public class ApplicationUserRoleView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}