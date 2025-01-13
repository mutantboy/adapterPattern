using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Model.Stripe
{
    public class StripeRefundResult
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }
}
