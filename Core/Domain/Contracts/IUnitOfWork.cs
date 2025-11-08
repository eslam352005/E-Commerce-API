using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {


        public IGenericRepo<TEntity, Tkey> GetRepo<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>;


        public Task<int> SaveChangesAsync();

    }
}
