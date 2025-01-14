using Adapter.Model;
using Adapter.Model.Payment;
using Adapter.Model.Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Adapter
{
    public class StripePaymentAdapter(StripeGateway stripeGateway) : IPaymentProcessor
    {
        private readonly StripeGateway _stripeGateway = stripeGateway;

        public async Task<PaymentResult> ProcessPayment(PaymentDetails payment)
        {
            var stripeRequest = new StripeChargeRequest
            {
                CardToken = GenerateCardToken(payment), // man würde stripe.js verwenden
                AmountInCents = (long)(payment.Amount * 100), 
                Currency = payment.Currency.ToLower(),
                Description = $"Order: {payment.OrderReference}"
            };

            try
            {
                var stripeResult = await _stripeGateway.CreateCharge(stripeRequest);

                return new PaymentResult
                {
                    Success = stripeResult.Status == "succeeded",
                    TransactionId = stripeResult.Id,
                    Message = stripeResult.Status == "succeeded" ? "Payment processed successfully" : "Payment failed",
                    ProcessedAt = stripeResult.Created
                };
            }
            catch (Exception ex)
            {
                return new PaymentResult
                {
                    Success = false,
                    Message = ex.Message,
                    ProcessedAt = DateTime.UtcNow
                };
            }
        }

        public async Task<RefundResult> ProcessRefund(RefundDetails refund)
        {
            try
            {
                var stripeResult = await _stripeGateway.RefundCharge(refund.TransactionId, refund.Amount);

                return new RefundResult
                {
                    Success = stripeResult.Status == "succeeded",
                    RefundId = stripeResult.Id,
                    Message = stripeResult.Status == "succeeded" ? "Refund processed successfully" : "Refund failed"
                };
            }
            catch (Exception ex)
            {
                return new RefundResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public Task<bool> ValidatePaymentDetails(PaymentDetails payment)
        {
            return Task.FromResult(
                !string.IsNullOrEmpty(payment.CardNumber) &&
                !string.IsNullOrEmpty(payment.CardHolderName) &&
                !string.IsNullOrEmpty(payment.ExpiryDate) &&
                !string.IsNullOrEmpty(payment.Cvv)
            );
        }

        private string GenerateCardToken(PaymentDetails payment)
        {
            /// In echtem Produktionscode würde man:
            /// 1. Die Kartendaten aus dem Zahlungsobjekt verwenden
            /// 2. Einen API-Aufruf an den Tokenisierungsdienst von Stripe machen
            /// 3. Ein sicheres Token zurückerhalten, das die Kartendaten repräsentiert

            /// payment.CardNumber, payment.ExpiryDate, etc. würden hier verwendet werden
            /// return stripeApi.CreateToken(payment.CardNumber, payment.ExpiryDate, payment.Cvv);
    
            /// Da dies jedoch nur ein Beispiel ist, geben wir stattdessen eine Dummy-GUID zurück
            return Guid.NewGuid().ToString();
        }
    }
}
