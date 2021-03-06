using MainApp.BLL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.BLL.Entities
{
    public class Event : Entity
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public string? Email { get; set; }
    }
}
