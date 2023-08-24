using System;
using System.Collections;
using System.Collections.Generic;

namespace Tracking.Models
{
    public class Event : BaseEntity
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Email { get; set; }
    }
}
