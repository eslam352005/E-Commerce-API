using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BadRequestException(List<string> errors ) : Exception("Invalid Registration")
    {
        public List<string> Errors { get; } = errors;
    }
}
