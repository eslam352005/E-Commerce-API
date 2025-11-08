using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotFoundUser(string email) : NotFoundException($"This Email {email} is Not Found , Try Register")
    {
    }
}
