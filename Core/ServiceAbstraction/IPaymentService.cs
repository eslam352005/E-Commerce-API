using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IPaymentService
    {

        Task<CartDto> CreateOrUpdatePaymentIntentAsync(string cartId);
        Task UpdatePaymentStatusAsync(string request ,  string stripeHeader);
    }
}
