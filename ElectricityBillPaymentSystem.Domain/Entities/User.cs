using ElectricityBillPaymentSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityBillPaymentSystem.Domain.Entities
{

    public class User : IdentityUser, IAuditable
    {
        public string Name { get; set; }

        public Wallet Wallet { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public static User Create(string name, string email)
        {
            return new User
            {
                Name = name,
                Email = email,
                UserName = email,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
        }




    }

}
