using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IServiceManager
    {
        public IProductService productService { get; }
        public ICartService cartService { get; }
        public IAuthenticationService authenticationService { get; }
        public IOrderService orderService { get; }
        public IPaymentService paymentService { get; }
    }
}
