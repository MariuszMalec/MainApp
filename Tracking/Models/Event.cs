using System;
using System.Collections;
using System.Collections.Generic;

namespace Tracking.Models
{
    public class Event : BaseEntity
    {
        public string Action { get; set; }
        public string? Email { get; set; }
        public virtual User? User { get; set; }
    }
}
