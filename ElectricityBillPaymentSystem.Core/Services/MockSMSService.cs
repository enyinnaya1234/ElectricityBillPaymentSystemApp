using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityBillPaymentSystem.Core.Services
{
    public static class MockSMSService
    {
        public static Task SendSmsAsync(string phoneNumber, string message)
        {
            Console.WriteLine($"SMS sent to {phoneNumber}: {message}");
            return Task.CompletedTask;
        }

        public static async Task NotifyLowBalance(decimal balance)
        {
            if (balance < 50) // Assume 50 as a low balance threshold
            {
                await SendSmsAsync("+1234567890", "Warning: Your wallet balance is low.");
            }
        }

        public static async Task NotifyPaymentSuccess(string billId)
        {
            await SendSmsAsync("+1234567890", $"Your payment for bill {billId} was successful.");
        }
    }

}
