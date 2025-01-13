using Adapter.Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adapter.Model
{
    public interface IPaymentProcessor
    {
        Task<PaymentResult> ProcessPayment(PaymentDetails payment);
        Task<RefundResult> ProcessRefund(RefundDetails refund);
        Task<bool> ValidatePaymentDetails(PaymentDetails payment);
    }
}
