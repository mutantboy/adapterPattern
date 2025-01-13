using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Model.Stripe
{
    public class StripeGateway
    {
        public async Task<StripeChargeResult> CreateCharge(StripeChargeRequest charge)
        {
            // Simulate API call to Stripe
            await Task.Delay(100);
            return new StripeChargeResult
            {
                Id = Guid.NewGuid().ToString(),
                Status = "succeeded",
                Created = DateTime.UtcNow
            };
        }

        public async Task<StripeRefundResult> RefundCharge(string chargeId, decimal amount)
        {
            await Task.Delay(100);
            return new StripeRefundResult
            {
                Id = Guid.NewGuid().ToString(),
                Status = "succeeded"
            };
        }
    }
}
