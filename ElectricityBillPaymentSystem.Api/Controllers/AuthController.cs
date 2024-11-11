using ElectricityBillPaymentSystem.Api.Dtos;
using ElectricityBillPaymentSystem.Core.Abstractions;
using ElectricityBillPaymentSystem.Core.Dtos;
using ElectricityBillPaymentSystem.Core.Dtos.ElectricityBillPaymentSystem.DTOs;
using ElectricityBillPaymentSystem.Core.Services;
using Microsoft.AspNetCore.Mvc;
using SalesOrderAPI.Core.Dtos;
using System.Threading.Tasks;

namespace ElectricityBillPaymentSystem.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        var result = await _authService.Register(registerUserDto);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
    {
        var result = await _authService.Login(loginUserDto);

        if (result.IsFailure)
            return BadRequest(result.Errors);

        return Ok(result.Data);
    }
}