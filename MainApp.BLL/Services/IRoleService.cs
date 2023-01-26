using MainApp.BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.BLL.Services
{
    public interface IRoleService
    {
        Task Delete(ApplicationUserRoleView entity);
        Task<IEnumerable<ApplicationUserRoleView>> GetAll();
        IQueryable<ApplicationUserRoleView> GetAllQueryable();
        Task<ApplicationUserRoleView> GetById(int id);
        Task Insert(ApplicationUserRoleView entity);
        Task Update(ApplicationUserRoleView entity);
    }
}