using Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class OrderSpecification : BaseSpecification<Order,Guid>
    {
        public OrderSpecification(string email):base(o=> o.userEmail == email)
        {
            AddInclude(o=> o.DeliveryMethod);
            AddInclude(o => o.Items);
            AddOrderDesc(o=> o.OrderDate);
        }

        public OrderSpecification(Guid Id) : base(o => o.Id == Id)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
           
        }
    }
}
