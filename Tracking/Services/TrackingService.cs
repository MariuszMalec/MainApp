using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Models;
using Tracking.Repositories;

namespace Tracking.Services
{
    public class TrackingService : IRepositoryService<Event>
    {
        private readonly IRepository<Event> Models;

        public TrackingService(IRepository<Event> models)
        {
            Models = models;
        }
        public async Task<IEnumerable<Event>> GetAll()
        {
            return await Models.GetAll();
        }

        public async Task Insert(Event model)
        {
            await Models.Insert(model);
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
