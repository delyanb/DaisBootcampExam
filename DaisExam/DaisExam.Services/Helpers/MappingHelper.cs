using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisExam.Models;
using DaisExam.Services.DTOs.Account;
using DaisExam.Services.DTOs.Payment;

namespace DaisExam.Services.Helpers
{
    public static class MappingHelper
    {
        public static PaymentInfo MapToPaymentInfo(Payment p, string fromAccountNumber, string toAccountNumber, bool? IsMine)
        {
            PaymentInfo info = new()
            {
               Amount =  p.Amount,
               DateTimeMade = p.DateTimeMade,
               PaymentId = p.PaymentId,
               FromAccountId = p.FromAccountId,
               ToAccountId = p.ToAccountId,
               Reason = p.Reason,
               FromAccountNumber = fromAccountNumber, 
               ToAccountNumber = toAccountNumber,
               Status = p.Status,
               DateApproved = p.DateApproved
               
               

            };
            if (IsMine != null) info.UserIsAccountOwner = (bool)IsMine;
            else info.UserIsAccountOwner = false;

                return info;
        }
        public static AccountDetailsDto MapToAccountDetails(Account a, List<string> otherOwners)
        {
            AccountDetailsDto dto = new()
            {
              AccountId = a.AccountId,
              AccountNumber = a.AccountNumber,
              AvailableAmount = a.AvailableAmount,
              OtherOwners = otherOwners
             

            };
            return dto;
        }
        public static AccountInfo MapToAccountInfo(Account a, List<string> otherOwners)
        {
            AccountInfo info = new()
            {
                AccountId = a.AccountId,
                AccountNumber = a.AccountNumber,
                OtherOwners = otherOwners

            };
            return info;
        }
        public static AccountInfo MapToAccountInfo(Account a)
        {
            AccountInfo info = new()
            {
                AccountId = a.AccountId,
                AccountNumber = a.AccountNumber,
                OtherOwners = null

            };
            return info;
        }
    }
}
