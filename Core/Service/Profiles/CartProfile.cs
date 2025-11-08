using AutoMapper;
using Domain.Models.Carts;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart,CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
        }
    }
}
