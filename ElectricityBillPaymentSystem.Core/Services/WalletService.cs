using ElectricityBillPaymentSystem.Core.Dtos;
using ElectricityBillPaymentSystem.Core.Abstractions;
using ElectricityBillPaymentSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ElectricityBillPaymentSystem.Core.Dtos.ElectricityBillPaymentSystem.DTOs;

namespace ElectricityBillPaymentSystem.Services
{
    public class WalletService : IWalletService
    {
        private readonly IRepository<Wallet> _walletRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        public WalletService(IRepository<Wallet> walletRepository, IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        public async Task<Result<WalletDTO>> AddFunds(string walletId, AddFundsDTO addFundsDto)
        {

            var wallet = await _walletRepository.FindById(walletId);
            if(wallet == null)
            return Result.Failure<WalletDTO>(new[] { new Error("WalletError", "The WalletId Provided is not Correct.") });


            wallet.Balance += addFundsDto.Amount;
            _walletRepository.Update(wallet);
            await _unitOfWork.SaveChangesAsync();


            var walletDto = new WalletDTO
            {
                Id = wallet.Id,
                Balance = wallet.Balance
            };

            return Result.Success(walletDto);
        }


        public async Task<Result<WalletDTO>> CreateUsersWallet(string userId)
        {
           
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure<WalletDTO>(new[] { new Error("UserError", "User not found.") });
            }

            if (user.Wallet != null)
            {
                return Result.Failure<WalletDTO>(new[] { new Error("WalletError", "User already has a wallet.") });
            }

          
            var wallet = new Wallet
            {
                UserId = userId,
                Balance = 0 
            };

          
            await _walletRepository.Add(wallet);
            await _unitOfWork.SaveChangesAsync();

         
            var walletDto = new WalletDTO
            {
                Id = wallet.Id,
                Balance = wallet.Balance
            };

            return Result.Success(walletDto);
        }

    }

}
