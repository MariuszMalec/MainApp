using System;

namespace Tracking.Models
{
    public class BaseEntity : IBaseEntity
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
