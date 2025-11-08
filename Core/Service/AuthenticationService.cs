using Domain.Exceptions;
using Domain.Models.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager , IConfiguration _configuration) : IAuthenticationService
    {
        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
          
            var user = await _userManager.Users.Include(u=> u.Address)
                                               .FirstOrDefaultAsync(u=> u.Email == email);

            if (user == null)
            {
                throw new NotFoundUser(email);
            }
            if (user.Address == null)
            {
                throw new NotFoundAddress(user.UserName!);
            }
            return new AddressDto()
            {
                
                City = user.Address.City,
                Country = user.Address.Country,
                FirstName = user.Address.FirstName,
                LastName = user.Address.LastName,
                Street = user.Address.Street
            };
        }

        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundUser(email);
            }
            else
            {
                return new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email!,
                    Token = await CreateTokenAsync(user)
                };
            }
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user != null;
        }
        public async Task<AddressDto> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto)
        {
            var user = await _userManager.Users.Include(u => u.Address)
                                               .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new NotFoundUser(email);
            }
            if (user.Address != null)
            {
               
               user.Address.Street = addressDto.Street;
               user.Address.City = addressDto.City;
               user.Address.FirstName = addressDto.FirstName;
               user.Address.LastName = addressDto.LastName;
               user.Address.Country = addressDto.Country;
              
            }
            else
            {
                user.Address = new Address()
                {
                    
                    Street = addressDto.Street,
                    City = addressDto.City,
                    FirstName = addressDto.FirstName,
                    LastName = addressDto.LastName,
                    Country = addressDto.Country
                };
            }

            await _userManager.UpdateAsync(user);
            return new AddressDto
            {
                
                Street = addressDto.Street,
                City = addressDto.City,
                FirstName = addressDto.FirstName,
                LastName = addressDto.LastName,
                Country = addressDto.Country
            };
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
           var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new NotFoundUser(loginDto.Email);
            }
            var IsValidPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (IsValidPassword)
            {
                return new UserDto
                {
                    Email = user.Email!,
                    DisplayName = user.DisplayName,
                    Token = await CreateTokenAsync(user)
                };
            }
            else
            {
                throw new UnAuthorizedException();
            }
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {

            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Username,
                PhoneNumber = registerDto.PhoneNumber,
            };


            var result = await _userManager.CreateAsync(user,registerDto.Password);

            if (result.Succeeded)
            {
                return new UserDto()
                {
                    Email = user.Email!,
                    DisplayName = user.DisplayName,
                    Token = await CreateTokenAsync(user)
                };
            }
            else
            {
                throw new BadRequestException(result.Errors.Select(e=> e.Description).ToList());
            }
        }

      

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.NameIdentifier,user.Id!)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {

                claims.Add(new Claim(ClaimTypes.Role,role));
            }

            var secretKey = _configuration["JwtOptions:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:issuer"],
                audience: _configuration["JwtOptions:audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
                );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
