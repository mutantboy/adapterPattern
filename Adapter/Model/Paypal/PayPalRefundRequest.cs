using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Model.Paypal
{
    public class PayPalRefundRequest
    {
        public string PaymentId { get; set; }
        public decimal RefundAmount { get; set; }
        public string Notes { get; set; }
    }
}
