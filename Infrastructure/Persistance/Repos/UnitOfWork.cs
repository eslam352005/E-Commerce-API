using Domain.Contracts;
using Domain.Models;
using Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repos
{
    public class UnitOfWork(StoreDbContext _context) : IUnitOfWork
    {
        Dictionary<string, object> _repos = new Dictionary<string, object>();
        public IGenericRepo<TEntity, Tkey> GetRepo<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            
            var name = typeof(TEntity).Name;
            if (_repos.ContainsKey(name))
            {
                return (IGenericRepo<TEntity, Tkey>)_repos[name];
            }
            var repo = new GenericRepo<TEntity, Tkey>(_context);
            _repos[name] = repo;
            return repo;
        }

        public async Task<int> SaveChangesAsync()
        {
          return await _context.SaveChangesAsync();
        }
    }
}
