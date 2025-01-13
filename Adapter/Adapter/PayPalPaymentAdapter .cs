using Adapter.Model.Payment;
using Adapter.Model.Paypal;
using Adapter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Adapter
{
    public class PayPalPaymentAdapter(PayPalGateway payPalGateway) : IPaymentProcessor
    {
        private readonly PayPalGateway _payPalGateway = payPalGateway;

        public async Task<PaymentResult> ProcessPayment(PaymentDetails payment)
        {
            var payPalRequest = new PayPalPaymentRequest
            {
                CreditCardNumber = payment.CardNumber,
                CardholderName = payment.CardHolderName,
                ExpirationDate = payment.ExpiryDate,
                SecurityCode = payment.Cvv,
                PaymentAmount = payment.Amount,
                Currency = payment.Currency
            };

            try
            {
                var payPalResult = await _payPalGateway.MakePayment(payPalRequest);

                return new PaymentResult
                {
                    Success = payPalResult.PaymentStatus == "COMPLETED",
                    TransactionId = payPalResult.PaymentId,
                    Message = payPalResult.PaymentStatus == "COMPLETED" ? "Payment processed successfully" : "Payment failed",
                    ProcessedAt = payPalResult.ProcessedTime
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
            var payPalRefundRequest = new PayPalRefundRequest
            {
                PaymentId = refund.TransactionId,
                RefundAmount = refund.Amount,
                Notes = refund.Reason
            };

            try
            {
                var payPalResult = await _payPalGateway.CreateRefund(payPalRefundRequest);

                return new RefundResult
                {
                    Success = payPalResult.Status == "COMPLETED",
                    RefundId = payPalResult.RefundId,
                    Message = payPalResult.Status == "COMPLETED" ? "Refund processed successfully" : "Refund failed"
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
    }

}
