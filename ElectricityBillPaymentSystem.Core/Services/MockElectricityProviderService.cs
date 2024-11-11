using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityBillPaymentSystem.Core.Services
{
    public static class MockElectricityProviderService
    {
        public static Task<bool> ValidateBill(decimal amount)
        {
            Console.WriteLine($"Bill validation for amount {amount} passed.");
            return Task.FromResult(true);
        }

        public static Task<bool> ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Payment of {amount} processed by provider.");
            return Task.FromResult(true);
        }
    }

}
