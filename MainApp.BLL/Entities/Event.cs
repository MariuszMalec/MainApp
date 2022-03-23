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
        public string Action { get; set; }

        public virtual User User { get; set; }
    }
}
