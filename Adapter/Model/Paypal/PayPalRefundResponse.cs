using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Model.Paypal
{
    public class PayPalRefundResponse
    {
        public string RefundId { get; set; }
        public string Status { get; set; }
    }
}
