using ElectricityBillPaymentSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ElectricityBillPaymentSystem.Domain.Entities
{
    public class Bill : Entity
    {
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending";
        public string UserId { get; set; }
        public User User { get; set; }
    }

}
