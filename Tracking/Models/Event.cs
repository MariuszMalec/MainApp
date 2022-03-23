using System;
using System.Collections;
using System.Collections.Generic;

namespace Tracking.Models
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IdUser { get; set; }

        public List<string> Actions;
    }
}
