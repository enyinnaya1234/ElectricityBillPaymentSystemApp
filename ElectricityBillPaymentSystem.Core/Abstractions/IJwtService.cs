using ElectricityBillPaymentSystem.Domain.Entities;

namespace ElectricityBillPaymentSystem.Core.Abstractions;

public interface IJwtService
{
    public string GenerateToken(User user);
}