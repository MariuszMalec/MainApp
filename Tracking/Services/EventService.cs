using System.Collections.Generic;
using Tracking.Models;
using Tracking.Repositories;

namespace Tracking.Services
{
    public class EventService : IRepositoryService<Event>
    {
        private readonly IRepository<Event> Models;

        public EventService(IRepository<Event> models)
        {
            Models = models;
        }
        public IEnumerable<Event> GetAll()
        {
            return Models.GetAll();
        }

        public void Insert(Event model)
        {
            Models.Insert(model);
        }

        public Event Get(int id)
        {
            return Models.Get(id);
        }

        public void Update(Event model)
        {
            Models.Update(model);
        }
        public void Delete(int id)
        {
            var model = Models.Get(id);
            Models.Delete(model);
        }
    }
}
