

using ElectricityBillPaymentSystem.Core.Abstractions;
using ElectricityBillPaymentSystem.Core.Dtos;
using ElectricityBillPaymentSystem.Core.Dtos.ElectricityBillPaymentSystem.DTOs;
using ElectricityBillPaymentSystem.Domain.Constants;
using ElectricityBillPaymentSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using SalesOrderAPI.Core.Dtos;

namespace SalesOrderAPI.Core.Services;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<User> _userManager;
    private readonly IWalletService _walletRepository;
    public AuthService(UserManager<User> userManager, IJwtService jwtService, IWalletService walletRepository)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _walletRepository = walletRepository;
    }

    public async Task<Result<RegisterResponseDto>> Register(RegisterUserDto registerUserDto)
    {
        var user = User.Create(registerUserDto.Name, registerUserDto.Email);

        var result = await _userManager.CreateAsync(user, registerUserDto.Password);
        if (!result.Succeeded)
            return Result.Failure<RegisterResponseDto>(result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray());

        var newUser = await _userManager.FindByEmailAsync(user.Email);
        if (newUser == null)
        {
            return Result.Failure<RegisterResponseDto>(new[] { new Error("UserError", "Failed to find user after creation.") });
        }
        var createWalletResult = await _walletRepository.CreateUsersWallet(newUser.Id);
        if (!createWalletResult.IsSuccess)
        {
            return Result.Failure<RegisterResponseDto>(createWalletResult.Errors);
        }

        var walletresult = createWalletResult.Data;

        var resultDto = new RegisterResponseDto
        {
            Id = newUser.Id,
            Name = newUser.Name,
            Email = user.Email,
            WalletId = walletresult.Id,
            Balance = walletresult.Balance
        };

        return Result.Success(resultDto);
    }

    public async Task<Result<LoginResponseDto>> Login(LoginUserDto loginUserDto)
    {
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email);

        if (user is null) return new Error[] { new("Auth.Error", "email or password not correct") };

        var isValidUser = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

        if (!isValidUser) return new Error[] { new("Auth.Error", "email or password not correct") };

        var token = _jwtService.GenerateToken(user);

        return new LoginResponseDto(token);
    }
}