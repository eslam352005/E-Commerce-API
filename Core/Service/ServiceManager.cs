using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager(IUnitOfWork _unitOfWork, IMapper mapper , ICartRepo repo, UserManager<ApplicationUser> userManager , IConfiguration configuration) : IServiceManager
    {
        private readonly Lazy<IProductService> _productService = new Lazy<IProductService>(()=> new ProductService(_unitOfWork,mapper));
        private readonly Lazy<ICartService> _cartService = new Lazy<ICartService>(() => new CartService(repo , mapper));
        private readonly Lazy<IAuthenticationService> _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, configuration));
        private readonly Lazy<IOrderService> _orderService = new Lazy<IOrderService>(() => new OrderService(mapper,repo,_unitOfWork));
        private readonly Lazy<IPaymentService> _paymentService = new Lazy<IPaymentService>(() => new PaymentService(configuration,repo,_unitOfWork,mapper));
        public IProductService productService => _productService.Value;

        public ICartService cartService => _cartService.Value;

        public IAuthenticationService authenticationService => _authenticationService.Value;
        public IOrderService orderService => _orderService.Value;

        public IPaymentService paymentService => _paymentService.Value;
    }
}
