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

        public async Task<Event> Get(int id)
        {
            return await Models.Get(id);
        }

        public async Task Update(Event model)
        {
            await Models.Update(model);
        }
        public async Task Delete(int id)
        {
            var model = await Models.Get(id);
            await Models.Delete(model);
        }

        public Task<AuthenticateModel> Authenticate(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}
