using MainApp.BLL.Entities;
using MainApp.BLL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainApp.BLL.Services
{
    public class UserService
    {
        private readonly IRepository<User> Users;

        public UserService(IRepository<User> users)
        {
            Users = users;
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            return await Users.GetAll();
        }

        public async Task Insert(User user)
        {
            await Users.Insert(user);
        }

        public async Task<User> GetById(int id)
        {
            return await Users.GetById(id);
        }

        public async Task Delete(User user)
        {
            await Users.Delete(user);
        }
        public async Task Update(User user)
        {
            await Users.Update(user);
        }
    }
}
