using System;

namespace MainApp.BLL.Entities
{
    public class Entity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
