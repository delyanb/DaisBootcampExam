using DaisExam.Models;
using DaisExam.Models.ViewModels.Account;
using DaisExam.Models.ViewModels.Payment;
using DaisExam.Services.DTOs.Account;
using DaisExam.Services.DTOs.Payment;

namespace DaisExam.Helpers
{
    public static class MappingHelper
    {
        public static PaymentVM MapToPaymentVM(PaymentInfo i)
        {
            PaymentVM dto = new()
            {
                PaymentId = i.PaymentId,
                FromAccountId = i.FromAccountId,
                FromAccountNumber = i.FromAccountNumber,
                ToAccountId = i.ToAccountId,
                ToAccountNumber = i.ToAccountNumber,
                Amount = i.Amount,
                Reason = i.Reason,
                DateTimeMade = i.DateTimeMade,
                UserIsAccountOwner = i.UserIsAccountOwner,
                Status = i.Status,
                DateApproved = i.DateApproved
                
            };
            return dto;
        }
        public static AccountInfoVM MapToAccountInfoVM(AccountInfo i)
        {
            AccountInfoVM dto = new()
            {
                AccountId = i.AccountId,
                AccountNumber = i.AccountNumber,
                OtherOwners = i.OtherOwners,
            };
            return dto; 
        }
        public static AccountDetailsVM MapToAccountDetailsDto(AccountDetailsDto d)
        {
            AccountDetailsVM vm = new()
            {
                AccountId = d.AccountId,
                AccountNumber = d.AccountNumber,
                OtherOwners = d.OtherOwners,
                AvailableAmount = d.AvailableAmount
            };
            return vm;
        }
    }
}
