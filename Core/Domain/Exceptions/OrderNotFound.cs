using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class OrderNotFound(Guid id) : NotFoundException($"Order with Id: {id} is not found")
    {
    }
}
