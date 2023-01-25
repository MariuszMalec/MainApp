using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainApp.BLL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MainApp.BLL.Models
{
    public class ApplicationUserRoleView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [HiddenInput(DisplayValue =false)]//TODO nie wyswietlanie w view value!
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string UserRole { get; set; }
    }
}