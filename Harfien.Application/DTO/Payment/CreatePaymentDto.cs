using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Payment
{
    public class CreatePaymentDto
    {
        public int OrderId { get; set; }
        public string CardNumber { get; set; }
        public string stripeToken { get; set; }

        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }
        public string Cvc { get; set; }

    }
}
