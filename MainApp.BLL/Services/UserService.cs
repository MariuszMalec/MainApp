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

        public async Task<bool> Insert(ApplicationUser user)
        {
            if (user == null)
            {
                return false;
            }
            await Users.Insert(user);
            return true;
        }

        public async Task<ApplicationUser> GetById(int id)
        {
            return await Users.GetById(id);
        }

        public async Task<bool> Delete(ApplicationUser user)
        {
            if (user == null)
            {
                return false;
            }
            return await Users.Delete(user);
        }

        public async Task<bool> Update(ApplicationUser user)
        {
            if (user == null)
            {
                return false;
            }
            return await Users.Update(user);
        }

        public async Task<ApplicationUser> GetByEmail(string userEmail)
        {
            return await Users.GetAllQueryable().FirstOrDefaultAsync(n => n.NormalizedEmail == userEmail.ToUpper());
        }

    }
}
