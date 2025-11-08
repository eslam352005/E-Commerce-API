using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, OrderAddress address, DeliveryMethod deliveryMethod, ICollection<OrderedItem> items, decimal subTotal,string paymentIntentId)
        {
            this.userEmail = userEmail;
            Address = address;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string userEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderAddress Address { get; set; } = default!;
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public string PaymentIntentId { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderedItem> Items { get; set; } = [];
        public decimal SubTotal { get; set; }
        public decimal GetTotal()
        {
            return SubTotal + DeliveryMethod.Price;
        }
    }
}
