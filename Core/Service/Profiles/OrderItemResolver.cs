using AutoMapper;
using Domain.Models.Orders;
using Microsoft.Extensions.Configuration;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Profiles
{
    public class OrderItemResolver(IConfiguration _configuration) : IValueResolver<OrderedItem, OrderItemDto, string>
    {
        public string Resolve(OrderedItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (source.Product.PictureUrl == null)
            {
                return string.Empty;
            }
            return $"{_configuration.GetSection("Urls")["BaseUrl"]}{source.Product.PictureUrl}";
        }
    }
}
