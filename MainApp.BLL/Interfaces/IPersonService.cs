using MainApp.BLL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainApp.BLL
{
    public interface IPersonService
    {
        Task<IEnumerable<ApplicationUser>> GetAll();

        Task Insert(ApplicationUser user);

        Task<ApplicationUser> GetById(int id);

        Task Delete(ApplicationUser user);

        Task Update(ApplicationUser user);

        Task<ApplicationUser> GetByEmail(string userEmail);
    }
}
