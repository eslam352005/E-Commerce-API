using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> createQuery<TEntity,TKey>(IQueryable<TEntity> inputQuery , ISpecification<TEntity,TKey> specification)where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;

            if(specification.Criteria is not null)
            {
                query= query.Where(specification.Criteria);
            }
            if(specification.OrderBy is not null)
            {
                query= query.OrderBy(specification.OrderBy);
            }
            if (specification.OrderByDesc is not null)
            {
                query = query.OrderByDescending(specification.OrderByDesc);
            }
            if (specification.isPaginated)
            {
                query = query.Skip(specification.Skip);
                query = query.Take(specification.Take);
            }

            if (specification.IncludeExpressions is not null && specification.IncludeExpressions.Any())
            {
                foreach(var expression in specification.IncludeExpressions)
                {
                    query = query.Include(expression);
                }
            }


            return query;
        }
    }
}
