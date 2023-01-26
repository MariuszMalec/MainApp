using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.BLL.Services
{
    public class RoleService : IRoleService<ApplicationRoles>
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Delete(ApplicationRoles entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationRoles>> GetAll()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles;
        }

        public IQueryable<ApplicationRoles> GetAllQueryable()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationRoles> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Insert(ApplicationRoles entity)
        {
            entity.NormalizedName = entity.Name.ToUpper();
            entity.ConcurrencyStamp = Guid.NewGuid().ToString();

            if (entity == null)
            {
                return false;
            }

            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task Update(ApplicationRoles entity)
        {
            throw new NotImplementedException();
        }
    }
}
