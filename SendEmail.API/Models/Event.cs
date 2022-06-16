namespace SendEmail.API.Models
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string? Email { get; set; }
    }
}
