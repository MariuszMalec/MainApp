using MainApp.BLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MainApp.BLL.Models
{
    public class LoginView
    {
        public string Email { get; set; }
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string ProviderName { get; set; }
    }
}
