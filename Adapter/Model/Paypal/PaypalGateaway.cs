using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Model.Paypal
{
    public class PayPalGateway
    {
        public async Task<PayPalPaymentResponse> MakePayment(PayPalPaymentRequest request)
        {
            await Task.Delay(100);
            return new PayPalPaymentResponse
            {
                PaymentId = Guid.NewGuid().ToString(),
                PaymentStatus = "COMPLETED",
                ProcessedTime = DateTime.UtcNow
            };
        }

        public async Task<PayPalRefundResponse> CreateRefund(PayPalRefundRequest request)
        {
            await Task.Delay(100);
            return new PayPalRefundResponse
            {
                RefundId = Guid.NewGuid().ToString(),
                Status = "COMPLETED"
            };
        }
    }
}
