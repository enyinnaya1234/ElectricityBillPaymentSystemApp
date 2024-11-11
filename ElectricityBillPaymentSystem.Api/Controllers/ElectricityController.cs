using ElectricityBillPaymentSystem.Core.Abstractions;
using ElectricityBillPaymentSystem.Core.Dtos;
using ElectricityBillPaymentSystem.Core.Dtos.ElectricityBillPaymentSystem.DTOs;
using ElectricityBillPaymentSystem.Core.Services;
using ElectricityBillPaymentSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElectricityBillPaymentSystem.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ElectricityController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly IWalletService _walletService;
        private readonly UserManager<User> _userManager;
        public ElectricityController(IBillService billService, IWalletService walletService, UserManager<User> userManager)
        {
            _billService = billService;
            _walletService = walletService;
            _userManager = userManager;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyBill([FromBody] CreateBillDTO createBillDto)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _billService.CreateBill(createBillDto, userId);
            if (result.IsFailure) return BadRequest(result.Errors);
            // Publish bill_created event (mocked SNS)
            await MockNotificationService.PublishEvent("bill_created", result.Data);
            return Ok(result);
        }

        [HttpPost("vend/{billId}/pay")]
        public async Task<IActionResult> PayBill(string billId)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _billService.PayBill(billId, userId);
            if (result.IsFailure) return BadRequest(result.Errors);
            // Publish payment_completed event (mocked SNS)
            await MockNotificationService.PublishEvent("payment_completed", result.Data);
            return Ok(result);
        }

        [HttpPost("wallets/{walletId}/add-funds")]
        public async Task<IActionResult> AddFunds(string walletId, [FromBody] AddFundsDTO addFundsDto)
        {
            var result = await _walletService.AddFunds(walletId, addFundsDto);
            if (result.IsFailure) return BadRequest(result.Errors);
            return Ok(result);
        }
    }
}
