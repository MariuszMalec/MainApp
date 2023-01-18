namespace MainApp.BLL.Models
{
    public class ApplicationUserRoleView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
