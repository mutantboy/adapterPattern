using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Model.Paypal
{
    public class PayPalPaymentRequest
    {
        public string CreditCardNumber { get; set; }
        public string CardholderName { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Currency { get; set; }
    }
}
