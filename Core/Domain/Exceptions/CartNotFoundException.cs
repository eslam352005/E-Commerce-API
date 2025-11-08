using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class CartNotFoundException(string id) : NotFoundException($"Cart with id {id} is Not found")
    {
    }
}
