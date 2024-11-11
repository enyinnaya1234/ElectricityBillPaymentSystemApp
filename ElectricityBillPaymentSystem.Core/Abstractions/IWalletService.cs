using ElectricityBillPaymentSystem.Core.Dtos;
using ElectricityBillPaymentSystem.Core.Dtos.ElectricityBillPaymentSystem.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityBillPaymentSystem.Core.Abstractions
{
    public interface IWalletService
    {
        Task<Result<WalletDTO>> AddFunds(string walletId, AddFundsDTO addFundsDto);
        Task<Result<WalletDTO>> CreateUsersWallet(string userId);
    }
}
