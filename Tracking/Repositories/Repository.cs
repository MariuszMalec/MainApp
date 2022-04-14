using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracking.Context;
using Tracking.Models;

namespace Tracking.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly MainApplicationContext _context;
        private DbSet<T> entities;
        public Repository(MainApplicationContext context)
        {
            _context = context;
            entities = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await entities.ToListAsync();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return entities.AsQueryable();
        }

        public T Get(int id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }
        public async Task Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            _context.SaveChanges();
        }
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _context.SaveChanges();
        }
    }
}
