using ElectricityBillPaymentSystem.Core.Abstractions;
using ElectricityBillPaymentSystem.Core.Dtos;
using ElectricityBillPaymentSystem.Core.Dtos.ElectricityBillPaymentSystem.DTOs;
using ElectricityBillPaymentSystem.Core.Services;
using ElectricityBillPaymentSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ElectricityBillPaymentSystem.Services
{
    public class BillService : IBillService
    {
        private readonly IRepository<Bill> _billRepository;
        private readonly IRepository<Wallet> _walletRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public BillService(IRepository<Bill> billRepository, IRepository<Wallet> walletRepository, IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _billRepository = billRepository;
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Result<BillDTO>> CreateBill(CreateBillDTO createBillDto,  string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure<BillDTO>(new[] { new Error("UserError", "User not found.") });
        
            var isValid = await MockElectricityProviderService.ValidateBill(createBillDto.Amount);
            if (!isValid) return Result.Failure<BillDTO>(new[] { new Error("ValidationFailed", "Invalid bill amount.") });

            var bill = new Bill
            {
                Amount = createBillDto.Amount,
                Status = "Pending",
                UserId = user.Id,
            };

            await _billRepository.Add(bill);
            await _unitOfWork.SaveChangesAsync();

            var billDto = new BillDTO { Id = bill.Id, Amount = bill.Amount, Status = bill.Status };
            await MockNotificationService.PublishEvent("bill_created", billDto);
            await MockSMSService.SendSmsAsync("+1234567890", $"A new bill of {bill.Amount} has been created.");

            return Result.Success(billDto);
        }

        public async Task<Result<BillDTO>> PayBill(string billId, string userId)
        {
            var user = await _userManager.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return Result.Failure<BillDTO>(new[] { new Error("UserError", "User not found.") });

            if (user.Wallet == null)
                return Result.Failure<BillDTO>(new[] { new Error("WalletFunds", "Wallet could not be found.") });

            var userWalletId = user.Wallet.Id;

            var bill = await _billRepository.FindById(billId);
            if (bill == null)
                return Result.Failure<BillDTO>(new[] { new Error("BillError", "Bill not found.") });

            var wallet = user.Wallet;
            if (wallet == null || wallet.Balance < bill.Amount)
                return Result.Failure<BillDTO>(new[] { new Error("InsufficientFunds", "Insufficient wallet funds.") });

            wallet.Balance -= bill.Amount;
            bill.Status = "Paid";

            _walletRepository.Update(wallet);
            _billRepository.Update(bill);
            await _unitOfWork.SaveChangesAsync();

            var billDto = new BillDTO { Id = bill.Id, Amount = bill.Amount, Status = bill.Status };
            await MockNotificationService.PublishEvent("payment_completed", billDto);
            await MockSMSService.NotifyPaymentSuccess(bill.Id);
            await MockSMSService.NotifyLowBalance(wallet.Balance);

            return Result.Success(billDto);
        }
    }

}
