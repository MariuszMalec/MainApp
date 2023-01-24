using MainApp.BLL.Entities;
using MainApp.BLL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainApp.BLL.Services
{
    public class UserService : IPersonService
    {
        private readonly IRepository<ApplicationUser> Users;

        public UserService(IRepository<ApplicationUser> users)
        {
            Users = users;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAll()
        {
            return await Users.GetAll();
        }

        public async Task Insert(ApplicationUser user)
        {
            await Users.Insert(user);
        }

        public async Task<ApplicationUser> GetById(int id)
        {
            return await Users.GetById(id);
        }

        public async Task Delete(ApplicationUser user)
        {
            await Users.Delete(user);
        }
        public async Task Update(ApplicationUser user)
        {
            await Users.Update(user);
        }

        public async Task<ApplicationUser> GetByEmail(string userEmail)
        {
            return await Users.GetAllQueryable().FirstOrDefaultAsync(n => n.NormalizedEmail == userEmail.ToUpper());
        }

    }
}
