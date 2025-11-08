using AutoMapper;
using Domain.Models.Orders;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile() { 
        
         CreateMap<AddressDto, OrderAddress>().ReverseMap();

         CreateMap<Order,OrderToReturnDto>()
                .ForMember(dist=> dist.DeliveryMethod , options=> options.MapFrom(src=> src.DeliveryMethod.ShortName));

         CreateMap<OrderedItem, OrderItemDto>()
               .ForMember(dist => dist.ProductName, options => options.MapFrom(src => src.Product.ProductName))
               .ForMember(dist => dist.PictureUrl, options => options.MapFrom<OrderItemResolver>());

            CreateMap<DeliveryMethod, DeliveryMethodDto>().ReverseMap();
        }
    }
}
