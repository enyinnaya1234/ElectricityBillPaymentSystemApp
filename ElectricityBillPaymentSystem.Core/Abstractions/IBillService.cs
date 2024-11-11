using ElectricityBillPaymentSystem.Core.Dtos;
using ElectricityBillPaymentSystem.Core.Dtos.ElectricityBillPaymentSystem.DTOs;

namespace ElectricityBillPaymentSystem.Core.Abstractions
{


    public interface IBillService
    {
        Task<Result<BillDTO>> CreateBill(CreateBillDTO createBillDto, string userId);
        Task<Result<BillDTO>> PayBill(string billId, string userId);
    }


}
