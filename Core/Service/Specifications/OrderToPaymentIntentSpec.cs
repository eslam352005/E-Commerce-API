using Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    internal class OrderToPaymentIntentSpec : BaseSpecification<Order,Guid>
    {
        public OrderToPaymentIntentSpec(string PaymentIntentId)
            :base(p=> p.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
