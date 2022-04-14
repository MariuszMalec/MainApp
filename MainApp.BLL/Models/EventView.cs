using MainApp.BLL.Entities;

namespace MainApp.BLL.Models
{
    public class EventView : Entity
    {
        public string Action { get; set; }
        public string? Email { get; set; }
    }
}
