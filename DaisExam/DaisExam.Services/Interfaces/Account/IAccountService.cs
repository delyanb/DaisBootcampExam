using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisExam.Services.DTOs.Account;

namespace DaisExam.Services.Interfaces.Account
{
    public interface IAccountService
    {
        Task<GetAccountInfosResponse> GetAccountsForUser(int userId);
        Task<GetAccountDetailsResponse> GetAccountDetails(GetAccountDetailsRequest request);
        Task<GetAccountInfosResponse> GetAllOtherAccountInfos(int accountId);
        Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request);
        Task<RemoveAccountResponse> RemoveAccount(RemoveAccountRequest request);
        Task<GetAccountInfosResponse> GetAccountsThatAreNotFromUser(int userId);
        //to add request
        Task<AddAccountResponse> AddToMyAccounts(int userId, int accountId);
    }
}
