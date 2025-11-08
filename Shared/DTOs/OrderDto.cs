using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class OrderDto
    {
        public string CartId { get; set; } = default!;
        public int DeliveryMethodId { get; set; } 
        public AddressDto Address { get; set; } = default!;

    }
}
