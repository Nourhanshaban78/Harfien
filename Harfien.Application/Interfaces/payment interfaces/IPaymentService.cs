using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harfien.Application.DTO.Payment;

namespace Harfien.Application.Interfaces.payment_interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResultDto> PayOrderWithCardAsync(CreatePaymentDto dto, string clientId);
    }
}
