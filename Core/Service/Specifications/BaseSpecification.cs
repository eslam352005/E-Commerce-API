using Domain.Contracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
   public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        protected BaseSpecification(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }


        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new List<Expression<Func<TEntity, object>>>();

        protected void AddInclude(Expression<Func<TEntity, object>> IncludeExpression)
        {
            IncludeExpressions.Add(IncludeExpression);
        }


        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDesc { get; private set; }
      
        public void AddOrder(Expression<Func<TEntity,object>>expression)
        {
            OrderBy = expression;
        }
        public void AddOrderDesc(Expression<Func<TEntity, object>> expression)
        {
            OrderByDesc = expression;
        }


        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool isPaginated { get;  set; }

        public void Paginate(int pageIndex, int pageSize)
        {
           Take = pageSize;
           Skip = (pageIndex - 1) * pageSize;
            isPaginated = true;
        }


    }
}
