using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _serviceManager.authenticationService.LoginAsync(loginDto);
            return Ok(user);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _serviceManager.authenticationService.RegisterAsync(registerDto);
            return Ok(user);
        }
        
        [HttpGet("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {

            var result = await _serviceManager.authenticationService.CheckEmailAsync(email);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _serviceManager.authenticationService.GetCurrentUserAsync(email!);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _serviceManager.authenticationService.GetCurrentUserAddressAsync(email!);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress( AddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _serviceManager.authenticationService.UpdateCurrentUserAddressAsync(email!,addressDto);
            return Ok(result);
        }
    }
}
