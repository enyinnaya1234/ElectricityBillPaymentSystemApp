using ElectricityBillPaymentSystem.Core.Dtos;
using SalesOrderAPI.Core.Dtos;

namespace ElectricityBillPaymentSystem.Core.Abstractions;

public interface IAuthService
{
    public Task<Result<RegisterResponseDto>> Register(RegisterUserDto registerUserDto);
    public Task<Result<LoginResponseDto>> Login(LoginUserDto loginUserDto);
}