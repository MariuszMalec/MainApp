using System;

namespace Tracking.Models
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
