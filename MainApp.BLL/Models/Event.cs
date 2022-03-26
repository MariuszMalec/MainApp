using MainApp.BLL.Entities;

namespace MainApp.BLL.Models
{
    public class Event : Entity
    {
        public string Action { get; set; }
        public string? Email { get; set; }
    }
}
