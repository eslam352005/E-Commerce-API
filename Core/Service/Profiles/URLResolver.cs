using AutoMapper;
using Domain.Models.Products;
using Microsoft.Extensions.Configuration;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Service.Profiles
{
    public class URLResolver(IConfiguration _configuration) : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
           
            if(source.PictureUrl == null)
            {
                return string.Empty;
            }
            return $"{_configuration.GetSection("Urls")["BaseUrl"]}{source.PictureUrl}";
        }
    }
}
