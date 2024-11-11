using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityBillPaymentSystem.Core.Dtos
{
    namespace ElectricityBillPaymentSystem.DTOs
    {
        public class UserDTO
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public decimal WalletBalance { get; set; }
        }

        public class BillDTO
        {
            public string Id { get; set; }
            public decimal Amount { get; set; }
            public string Status { get; set; }
        }

        public class WalletDTO
        {
            public string Id { get; set; }
            public decimal Balance { get; set; }
        }

        public class CreateBillDTO
        {
            public decimal Amount { get; set; }
        }

        public class AddFundsDTO
        {
            public decimal Amount { get; set; }
        }
    }



}
