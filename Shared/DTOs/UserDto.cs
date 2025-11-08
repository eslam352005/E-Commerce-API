using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class UserDto
    {
        public string Email { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Token { get; set; } = null!;
      

    }
}
