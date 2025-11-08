using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotFoundAddress(string username) : NotFoundException($"User {username} doesn't have Address")
    {
    }
}
