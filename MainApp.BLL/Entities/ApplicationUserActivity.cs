using MainApp.BLL.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.BLL.Entities
{
    public class ApplicationUserActivity : Entity
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public ActivityActions Action { get; set; }
        public HttpContext HttpContext { get; set; }

    }
}
