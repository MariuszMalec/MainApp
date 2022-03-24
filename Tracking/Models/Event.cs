using System;
using System.Collections;
using System.Collections.Generic;

namespace Tracking.Models
{
    public class Event : Entity
    {
        public string Action { get; set; }
        public string? Email { get; set; }
        public virtual User? User { get; set; }
    }
}
