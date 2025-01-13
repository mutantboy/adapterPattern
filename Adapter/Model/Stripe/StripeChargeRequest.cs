using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Model.Stripe
{
    public class StripeChargeRequest
    {
        public string CardToken { get; set; }
        public long AmountInCents { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
    }
}
