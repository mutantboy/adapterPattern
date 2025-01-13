using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Model.Paypal
{
    public class PayPalPaymentResponse
    {
        public string PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime ProcessedTime { get; set; }
    }
}
