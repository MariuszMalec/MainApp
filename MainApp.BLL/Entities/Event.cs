namespace MainApp.BLL.Entities
{
    public class Event : Entity
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Email { get; set; }
    }
}
