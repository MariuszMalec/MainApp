using MainApp.BLL.Entities;
using MainApp.BLL.Repositories;
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

        public EventService(IRepository<Event> events)
        {
            Events = events;
        }
        public async Task<IEnumerable<Event>> GetAll()
        {
            return await Events.GetAll();
        }

        public async Task Insert(Event user)
        {
            await Events.Insert(user);
        }

    }
}
