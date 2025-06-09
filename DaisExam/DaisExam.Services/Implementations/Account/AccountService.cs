using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using DaisExam.Models;
using DaisExam.Services.DTOs.Account;
using DaisExam.Services.Helpers;
using DaisExam.Services.Interfaces.Account;
using DeisExam.Data.Interfaces.Account;
using DeisExam.Data.Interfaces.UserAccount;
using DeisExam.Data.UnitOfWork;
using Microsoft.Identity.Client;

namespace DaisExam.Services.Implementations.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetAccountInfosResponse> GetAccountsForUser(int userId)
        {
            GetAccountInfosResponse response = new();
            try
            {

                var user = await _unitOfWork.Users.RetrieveAsync(userId);
                if (user == null)
                {
                    response.ErrorMessage = "User Id not found!";
                    return response;
                }

                // Get all UserAccount relationships for this user
                var userAccounts = await _unitOfWork.UserAccounts
                    .RetrieveCollectionAsync(new UserAccountFilter { UserId = userId })
                    .ToListAsync();


                if (userAccounts.Count == 0)
                {
                    response.AccountInfos = new List<AccountInfo>();
                    return response;
                }

                var accountInfos = new List<AccountInfo>();

                foreach (var userAccount in userAccounts)
                {
                    var account = await _unitOfWork.Accounts.RetrieveAsync(userAccount.AccountId);
                    if (account == null)
                        continue;

                    var allUserAccounts = await _unitOfWork.UserAccounts
                        .RetrieveCollectionAsync(new UserAccountFilter { AccountId = userAccount.AccountId })
                        .ToListAsync();

                    var otherOwners = new List<string>();

                    foreach (var ua in allUserAccounts)
                    {
                        if (ua.UserId != userId)
                        {
                            var otherUser = await _unitOfWork.Users.RetrieveAsync(ua.UserId);
                            if (otherUser != null)
                                otherOwners.Add(otherUser.FullName);
                        }
                    }

                    accountInfos.Add(MappingHelper.MapToAccountInfo(account,otherOwners));
                }
                response.Success = true;
                response.AccountInfos = accountInfos;
                return response;


            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
        public async Task<GetAccountDetailsResponse> GetAccountDetails(GetAccountDetailsRequest request)
        {
            GetAccountDetailsResponse response = new();
            try
            {
                var account = await _unitOfWork.Accounts.RetrieveAsync(request.AccountId);

                if (account == null)
                {
                    response.ErrorMessage = "Account not found.";
                    return response;
                }
                var isOwner = await _unitOfWork.UserAccounts
                    .RetrieveCollectionAsync(new UserAccountFilter { UserId = request.UserId, AccountId = request.AccountId })
                    .AnyAsync();

                if (!isOwner)
                {
                    response.ErrorMessage = "You are not associated with this account.";
                    return response;
                }

                var userAccounts = await _unitOfWork.UserAccounts
                    .RetrieveCollectionAsync(new UserAccountFilter { AccountId = request.AccountId })
                    .ToListAsync();

                var otherOwners = new List<string>();

                foreach (var userAccount in userAccounts)
                {
                    var user = await _unitOfWork.Users.RetrieveAsync(userAccount.UserId);
                    if (user != null && user.UserId!=request.UserId)
                    {
                        otherOwners.Add(user.FullName);
                    }
                }

                response.AccountDetailsDto = MappingHelper.MapToAccountDetails(account, otherOwners);
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
        public async Task<GetAccountInfosResponse> GetAllOtherAccountInfos(int accountId)
        {
            var response = new GetAccountInfosResponse();

            try
            {
                var allAccounts = await _unitOfWork.Accounts
                    .RetrieveCollectionAsync(new AccountFilter()) 
                    .ToListAsync();

                var otherAccounts = allAccounts
                    .Where(a => a.AccountId != accountId)
                    .Select(a=>MappingHelper.MapToAccountInfo(a))
                    .ToList();

                response.AccountInfos = otherAccounts;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
        public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request)
        {
            CreateAccountResponse response = new();

            try
            {
                var newAccount = new Models.Account
                {
                    AccountNumber = RandomizeHelper.GenerateAccountNumber(),
                    AvailableAmount = 0
                };

                var accountId = await _unitOfWork.Accounts.CreateAsync(newAccount);

                await _unitOfWork.UserAccounts.CreateAsync(new UserAccount
                {
                    AccountId = accountId,
                    UserId = request.UserId
                });

                response.Success = true;
                response.CreatedAccountId = accountId;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
        public async Task<RemoveAccountResponse> RemoveAccount(RemoveAccountRequest request)
        {
            RemoveAccountResponse response = new();

            try
            {

               
                var account = await _unitOfWork.Accounts.RetrieveAsync(request.AccountId);
                if (account == null)
                {
                    response.ErrorMessage = "Account not found.";
                    return response;
                }

                UserAccountFilter filter = new() { AccountId = request.AccountId };

                
                var mappingDeleted = await _unitOfWork.UserAccounts.DeleteComposite(request.UserId, request.AccountId);
                if (!mappingDeleted)
                {
                    response.ErrorMessage = "Error deleting.";
                    return response;
                }
                

                var remainingOwners = await _unitOfWork.UserAccounts.RetrieveCollectionAsync(filter).ToListAsync();
                if (remainingOwners.Count() ==0)
                {
                    await _unitOfWork.Accounts.DeleteAsync(request.AccountId);
                    response.Deleted = true;
                    
                }
                response.AccountNumberRemoved = account.AccountNumber;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
        public async Task<GetAccountInfosResponse> GetAccountsThatAreNotFromUser(int userId)
        {
            GetAccountInfosResponse response = new();

            try
            {
                var allAccounts = await _unitOfWork.Accounts.RetrieveCollectionAsync(new AccountFilter()).ToListAsync();

                var userAccountLinks = await _unitOfWork.UserAccounts
                    .RetrieveCollectionAsync(new UserAccountFilter { UserId = userId })
                    .ToListAsync();

                var userAccountIds = userAccountLinks.Select(link => link.AccountId).ToHashSet();

                var accountsNotOwned = allAccounts
                    .Where(account => !userAccountIds.Contains(account.AccountId))
                    .ToList();

                response.AccountInfos = new();
                List<AccountInfo> AccountInfos = new();

                foreach (var account in accountsNotOwned)
                {
                    var Owners = new List<string>();
                    var userLinks = await _unitOfWork.UserAccounts
                        .RetrieveCollectionAsync(new UserAccountFilter { AccountId = account.AccountId })
                        .ToListAsync();


                    foreach (var link in userLinks)
                    {
                        var user = await _unitOfWork.Users.RetrieveAsync(link.UserId);
                        if (user != null)
                        {
                            Owners.Add(user.FullName);
                        }
                    }
                    AccountInfos.Add(MappingHelper.MapToAccountInfo(account, Owners));

                    
                }
                response.AccountInfos = AccountInfos;
                response.Success = true;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
        //to-do
        public async Task<AddAccountResponse> AddToMyAccounts(int userId, int accountId)
        {
            AddAccountResponse response = new();

            try
            {
                var existingMappings = await _unitOfWork.UserAccounts
                    .RetrieveCollectionAsync(new UserAccountFilter { UserId = userId, AccountId = accountId })
                    .ToListAsync();

                if (existingMappings.Any())
                {
                    response.ErrorMessage = "You are already associated with this account. (Фронт енда е сдал багажа)";
                    return response;
                }

                await _unitOfWork.UserAccounts.CreateAsync(new UserAccount
                {
                    UserId = userId,
                    AccountId = accountId
                });


                response.Success = true;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
    }
}
