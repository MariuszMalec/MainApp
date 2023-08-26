using MainApp.BLL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainApp.BLL
{
    public interface IPersonService
    {
        Task<IEnumerable<ApplicationUser>> GetAll();

        Task<bool> Insert(ApplicationUser user);

        Task<ApplicationUser> GetById(int id);

        Task Delete(ApplicationUser user);

        Task<bool> Update(ApplicationUser user);

        Task<ApplicationUser> GetByEmail(string userEmail);
    }
}
