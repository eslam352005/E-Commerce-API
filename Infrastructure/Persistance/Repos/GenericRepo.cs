using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repos
{
    public class GenericRepo<TEntity, TKey>(StoreDbContext _context) : IGenericRepo<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public async Task AddAsync(TEntity entity)
        {
         await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<int> CountAsync(ISpecification<TEntity, TKey> specification)
        {
            return await SpecificationEvaluator.createQuery(_context.Set<TEntity>(), specification).CountAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
           return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification)
        {
            return await SpecificationEvaluator.createQuery(_context.Set<TEntity>() , specification).ToListAsync();
        }

       
        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification)
        {
            return await SpecificationEvaluator.createQuery(_context.Set<TEntity>(),specification).FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }


        public void Remove(TEntity entity)
        {
           _context.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}
