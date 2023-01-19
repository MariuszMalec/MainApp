using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
    }
}
