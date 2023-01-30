﻿using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.BLL.Services
{
    public class UserRoleService : IRepositoryService<ApplicationUserRoleView>
    {
        private readonly ApplicationDbContext _context;

        public UserRoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<bool> Delete(int Id, ApplicationUserRoleView entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ApplicationUserRoleView>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            var models = users.Select(MapApplicationUserApplicationUserRoleView);//reczne mapowanie na model
            return models;
        }

        private ApplicationUserRoleView MapApplicationUserApplicationUserRoleView(ApplicationUser appUser)
        {
            var roleId = _context.UserRoles.Where(r => r.UserId == appUser.Id).Select(r => r.RoleId).FirstOrDefault();
            return new ApplicationUserRoleView
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                UserId = appUser.Id,
                RoleId = roleId,
                UserRole = _context.Roles.Where(u => u.Id == roleId).Select(r => r.Name).FirstOrDefault()
            };
        }

        public IQueryable<ApplicationUserRoleView> GetAllQueryable()
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUserRoleView> GetById(int id)
        {
            var userRoles = await _context.Users.ToListAsync();
            var userRolesView = userRoles.Select(MapApplicationUserApplicationUserRoleView);
            var model = userRolesView.SingleOrDefault(u=>u.Id == id);
            return model;
        }

        public Task<bool> Insert(ApplicationUserRoleView entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(int id, ApplicationUserRoleView entity)
        {
            throw new NotImplementedException();
        }
    }
}
