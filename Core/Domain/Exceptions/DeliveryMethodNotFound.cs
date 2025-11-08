using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DeliveryMethodNotFound(int id) : NotFoundException($"Delivery Method With Id {id} is not found")
    {
    }
}
