using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.BLL.Services
{
    public class EventService
    {
        private readonly IRepository<Event> Events;
        private UserService _userService;

        public EventService(IRepository<Event> events, UserService userService)
        {
            Events = events;
            _userService = userService;
        }
        public async Task<IEnumerable<Event>> GetAll()
        {
            return await Events.GetAll();
        }
        public async Task Insert(Event myEvent)
        {
            await Events.Insert(myEvent);
        }
        public async Task<Event> GetById(int id)
        {
            return await Events.GetById(id);
        }
        public async Task Delete(Event myEvent)
        {
            await Events.Delete(myEvent);
        }
        public async Task<string> InsertEvent(ActivityActions activityActions, HttpContext httpContext, string email)
        {
            var userEmail = httpContext.User.Identity.Name;
            if (userEmail == null)
                userEmail = email;
            var user = await _userService.GetByEmail(userEmail);
            await Insert(new Event { CreatedDate = DateTime.UtcNow, User = user, Email = userEmail, Action = activityActions.ToString() });
            return userEmail;
        }

    }
}
