using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracking.Context;
using Tracking.Models;

namespace Tracking.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly MainApplicationContext _context;
        private DbSet<T> entities;
        private readonly IConfiguration _configuration;
        public Repository(MainApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            entities = context.Set<T>();
            _configuration = configuration;
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await entities.ToListAsync();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return entities.AsQueryable();
        }

        public async Task<T> Get(int id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id);
        }
        public async Task Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            //TODO problem z postgresem w linux automat nie dodaje kolejnego id!
            var defaultprovider = _configuration["Provider"];//Problem z testem wrzuca do bazy relacyjnej!
            if (defaultprovider.Contains("Postgres"))
            {
                if (entities.Count() == 0)
                    entity.Id = 1;
                if (entities.Count() > 0)
                    entity.Id = (entities?.Max(m => m.Id) ?? 0) + 1;
            }
            entities.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
