using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepo<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task<TEntity?> GetByIdAsync(Tkey id);
        public Task AddAsync(TEntity entity);
        public void Update(TEntity entity);
        public void Remove(TEntity entity);

        public Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity,Tkey> specification);
        public Task<TEntity?> GetByIdAsync(ISpecification<TEntity, Tkey> specification);

        public Task<int> CountAsync(ISpecification<TEntity, Tkey> specification);

    }
}
