using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);

        Task<bool> CheckEmailAsync(string email);
        Task<UserDto> GetCurrentUserAsync(string email);
        Task<AddressDto> GetCurrentUserAddressAsync(string email);
        Task<AddressDto> UpdateCurrentUserAddressAsync(string email,AddressDto addressDto);
    }
}
