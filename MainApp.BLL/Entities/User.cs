﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.BLL.Entities
{
    public class User : Person
    {
        public virtual Role Role { get; set; }
        public string PasswordHash { get; set; }
    }
}
