using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Models;
using MainApp.BLL.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Web.Services
{
    public class RoleService : IRepositoryService<ApplicationRoles>
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(int id, ApplicationRoles model)
        {
            if (model == null)
            {
                return false;
            }
            var user = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(user);
            await _context.SaveChangesAsync();
            return true;
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

        public async Task<ApplicationRoles> GetById(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            return role;
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

        public async Task<bool> Update(int id, ApplicationRoles entity)
        {
            if (entity == null)
            {
                return false;
            }
            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}