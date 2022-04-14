using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.BLL.Entities
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime Created { get; set; }
    }
}
