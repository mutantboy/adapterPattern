using Adapter.Adapter;
using Adapter.Model.Payment;
using Adapter.Model.Paypal;
using Adapter.Model.Stripe;
using Adapter.Service;

namespace Adapter
{
    public class Program
    {
        public static async Task Main()
        {
            var stripeService = new PaymentService(
                new StripePaymentAdapter(new StripeGateway())
            );

// tag::snippet-AdapterUsage[]
            var paypalService = new PaymentService(
                new PayPalPaymentAdapter(new PayPalGateway())
            );
// end::snippet-AdapterUsage[]

            await TestSuccessfulPayments(stripeService, paypalService);
            await TestInvalidPaymentDetails(stripeService, paypalService);
            await TestRefundProcess(stripeService, paypalService);
            await TestDifferentCurrencies(stripeService, paypalService);
        }

        private static async Task TestSuccessfulPayments(PaymentService stripeService, PaymentService paypalService)
        {
            Console.WriteLine("\n=== Testing Successful Payments ===");

            var validPayment = new PaymentDetails
            {
                CardNumber = "4111111111111111",
                CardHolderName = "John Doe",
                ExpiryDate = "12/25",
                Cvv = "123",
                Amount = 99.99m,
                Currency = "USD",
                OrderReference = "ORD-12345"
            };

            var stripeResult = await stripeService.ProcessOrderPayment(validPayment);
            Console.WriteLine($"Stripe Payment Result: {stripeResult.Success}, Message: {stripeResult.Message}");
            Console.WriteLine($"Transaction ID: {stripeResult.TransactionId}");
            Console.WriteLine($"Processed At: {stripeResult.ProcessedAt}");

            var paypalResult = await paypalService.ProcessOrderPayment(validPayment);
            Console.WriteLine($"PayPal Payment Result: {paypalResult.Success}, Message: {paypalResult.Message}");
            Console.WriteLine($"Transaction ID: {paypalResult.TransactionId}");
            Console.WriteLine($"Processed At: {paypalResult.ProcessedAt}");
        }
        private static async Task TestInvalidPaymentDetails(PaymentService stripeService, PaymentService paypalService)
        {
            Console.WriteLine("\n=== Testing Invalid Payment Details ===");

            var invalidPayment = new PaymentDetails
            {
                CardNumber = "", /// Ungültig: Leere Kartennummer
                CardHolderName = "Jane Doe",
                ExpiryDate = "12/25",
                Cvv = "123",
                Amount = 50.00m,
                Currency = "USD",
                OrderReference = "ORD-12346"
            };

            var stripeResult = await stripeService.ProcessOrderPayment(invalidPayment);
            Console.WriteLine($"Stripe Invalid Payment Result: {stripeResult.Success}, Message: {stripeResult.Message}");

            var paypalResult = await paypalService.ProcessOrderPayment(invalidPayment);
            Console.WriteLine($"PayPal Invalid Payment Result: {paypalResult.Success}, Message: {paypalResult.Message}");
        }

        private static async Task TestRefundProcess(PaymentService stripeService, PaymentService paypalService)
        {
            Console.WriteLine("\n=== Testing Refund Process ===");

            /// Zuerst Zahlung machen unm Transaktionsid zu bekommen
            var paymentDetails = new PaymentDetails
            {
                CardNumber = "4111111111111111",
                CardHolderName = "John Smith",
                ExpiryDate = "12/25",
                Cvv = "123",
                Amount = 150.00m,
                Currency = "USD",
                OrderReference = "ORD-12347"
            };


            var stripePayment = await stripeService.ProcessOrderPayment(paymentDetails);
            var paypalPayment = await paypalService.ProcessOrderPayment(paymentDetails);


            var refundDetails = new RefundDetails
            {
                TransactionId = stripePayment.TransactionId,
                Amount = 150.00m,
                Reason = "Customer request"
            };

            var stripeRefund = await ((StripePaymentAdapter)stripeService.GetType()
                .GetField("_paymentProcessor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(stripeService)).ProcessRefund(refundDetails);

            Console.WriteLine($"Stripe Refund Result: {stripeRefund.Success}, Message: {stripeRefund.Message}");

            refundDetails.TransactionId = paypalPayment.TransactionId;
            var paypalRefund = await (paypalService.GetType()
                .GetField("_paymentProcessor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(paypalService) as PayPalPaymentAdapter).ProcessRefund(refundDetails);

            Console.WriteLine($"PayPal Refund Result: {paypalRefund.Success}, Message: {paypalRefund.Message}");
        }

        private static async Task TestDifferentCurrencies(PaymentService stripeService, PaymentService paypalService)
        {
            Console.WriteLine("\n=== Testing Different Currencies ===");

            var currencies = new[] { "USD", "EUR", "GBP", "JPY" };

            foreach (var currency in currencies)
            {
                var paymentDetails = new PaymentDetails
                {
                    CardNumber = "4111111111111111",
                    CardHolderName = "Alice Johnson",
                    ExpiryDate = "12/25",
                    Cvv = "123",
                    Amount = 200.00m,
                    Currency = currency,
                    OrderReference = $"ORD-{currency}-12348"
                };

                var stripeResult = await stripeService.ProcessOrderPayment(paymentDetails);
                Console.WriteLine($"Stripe {currency} Payment: {stripeResult.Success}, Message: {stripeResult.Message}");

                var paypalResult = await paypalService.ProcessOrderPayment(paymentDetails);
                Console.WriteLine($"PayPal {currency} Payment: {paypalResult.Success}, Message: {paypalResult.Message}");
            }
        }
    }
}