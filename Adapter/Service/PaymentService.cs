using Adapter.Model.Payment;
using Adapter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Service
{
    public class PaymentService
    {
        private readonly IPaymentProcessor _paymentProcessor;

        public PaymentService(IPaymentProcessor paymentProcessor)
        {
            _paymentProcessor = paymentProcessor;
        }

        public async Task<PaymentResult> ProcessOrderPayment(PaymentDetails paymentDetails)
        {
            if (!await _paymentProcessor.ValidatePaymentDetails(paymentDetails))
            {
                return new PaymentResult
                {
                    Success = false,
                    Message = "Invalid payment details",
                    ProcessedAt = DateTime.UtcNow
                };
            }

            return await _paymentProcessor.ProcessPayment(paymentDetails);
        }
    }
}
