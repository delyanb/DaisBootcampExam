using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisExam.Models;
using DaisExam.Services.DTOs.Account;
using DaisExam.Services.DTOs.Payment;
using DaisExam.Services.Helpers;
using DaisExam.Services.Interfaces.Payment;
using DeisExam.Data.Interfaces.Account;
using DeisExam.Data.Interfaces.Payment;
using DeisExam.Data.Interfaces.UserAccount;
using DeisExam.Data.UnitOfWork;

namespace DaisExam.Services.Implementations.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetPaymentInfosResponse> GetPaymentsForUser(int userId)
        {
            GetPaymentInfosResponse response = new();
            try
            {

                var user = _unitOfWork.Users.RetrieveAsync(userId);
                if (user == null) 
                {
                    response.ErrorMessage = "User Id not found!";
                    return response;
                }

                PaymentFilter filter = new() { UserId = userId};
                var paymentsForUser = await _unitOfWork.Payments.RetrieveCollectionAsync(filter).ToListAsync();

                List<PaymentInfo> paymentInfos = new List<PaymentInfo>();

                var userAccounts = await _unitOfWork.UserAccounts
                    .RetrieveCollectionAsync(new UserAccountFilter { UserId = userId })
                    .ToListAsync();

                var userOwnedAccountIds = userAccounts.Select(ua => ua.AccountId).ToHashSet();


                foreach (var p in paymentsForUser)
                {
                    var toAccount = await _unitOfWork.Accounts.RetrieveAsync(p.ToAccountId);
                    var fromAccount = await _unitOfWork.Accounts.RetrieveAsync(p.FromAccountId);
                    var isCurrentlyMine = userOwnedAccountIds.Contains(p.FromAccountId);
                    
                    paymentInfos.Add(MappingHelper.MapToPaymentInfo(p, fromAccount.AccountNumber, toAccount.AccountNumber,isCurrentlyMine));
                }

                response.Success = true;
                response.PaymentInfos = paymentInfos;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }

        }
        public async Task<GetPaymentInfosResponse> GetPaymentsForAccount(int accountId)
        {
            GetPaymentInfosResponse response = new();
            try
            {
                var filter = new PaymentFilter();

                var payments = await _unitOfWork.Payments
                    .RetrieveCollectionAsync(filter)
                    .ToListAsync();

                var relevantPayments = payments
                    .Where(p => p.FromAccountId == accountId || p.ToAccountId == accountId)
                    .OrderBy(p => p.DateTimeMade)
                    .ToList();

                var paymentInfos = new List<PaymentInfo>();

                foreach (var p in relevantPayments)
                {
                    var fromAccount = await _unitOfWork.Accounts.RetrieveAsync(p.FromAccountId);
                    var toAccount = await _unitOfWork.Accounts.RetrieveAsync(p.ToAccountId);

                    paymentInfos.Add(MappingHelper.MapToPaymentInfo(p,fromAccount.AccountNumber,toAccount.AccountNumber,false));
                }

                response.PaymentInfos = paymentInfos;
                response.Success = true;
                return response;

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
        public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request)
        {
            CreatePaymentResponse response = new();
            try
            {
                if (request.Amount <= 0)
                {
                    response.ErrorMessage = "Amount must be greater than 0.";
                    return response;
                }

                if (request.FromAccountId == request.ToAccountId)
                {
                    response.ErrorMessage = "Cannot transfer to the same account.";
                    return response;
                }

                var fromAccount = await _unitOfWork.Accounts.RetrieveAsync(request.FromAccountId);
                var toAccount = await _unitOfWork.Accounts.RetrieveAsync(request.ToAccountId);

                if (fromAccount == null || toAccount == null)
                {
                    response.ErrorMessage = "Invalid account(s).";
                    return response;
                }

                var ownerships = await _unitOfWork.UserAccounts
                    .RetrieveCollectionAsync(new UserAccountFilter { UserId = request.UserId })
                    .ToListAsync();

                if (!ownerships.Any(ua => ua.AccountId == request.FromAccountId))
                {
                    response.ErrorMessage = "User is not authorized to use the source account.";
                    return response;
                }

                if (fromAccount.AvailableAmount < request.Amount)
                {
                    response.ErrorMessage = "Insufficient funds.";
                    return response;
                }

                var payment = new Models.Payment
                {
                    FromAccountId = request.FromAccountId,
                    ToAccountId = request.ToAccountId,
                    Amount = request.Amount,
                    Reason = request.Reason ?? "",
                    DateTimeMade = DateTime.Now,
                    UserId = request.UserId,
                    Status = "PENDING"
                };

                var paymentId = await _unitOfWork.Payments.CreateAsync(payment);


                response.CreatedPaymentId = paymentId;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Payment creation failed: {ex.Message}";
                return response;
            }
        }
        public async Task<ProcessPaymentResponse> ApprovePayment(int paymentId)
        {
            ProcessPaymentResponse response = new();
            try
            {
                var payment = await _unitOfWork.Payments.RetrieveAsync(paymentId);

                if (payment == null)
                {
                    response.ErrorMessage = "Payment not found.";
                    return response;
                }

                if (payment.Status != "PENDING")
                {
                    response.ErrorMessage = "Only pending payments can be approved.";
                    return response;
                }

                var fromAccount = await _unitOfWork.Accounts.RetrieveAsync(payment.FromAccountId);
                if (fromAccount == null)
                {
                    response.ErrorMessage = "Source account not found.";
                    return response;
                }

                if (fromAccount.AvailableAmount < payment.Amount)
                {
                    response.ErrorMessage = "Insufficient funds in source account.";
                    return response;
                }

                fromAccount.AvailableAmount -= payment.Amount;

                await _unitOfWork.Accounts.UpdateAsync(fromAccount.AccountId, new AccountUpdate
                {
                    AvailableAmount = new System.Data.SqlTypes.SqlDecimal(fromAccount.AvailableAmount)
                });

                await _unitOfWork.Payments.UpdateAsync(payment.PaymentId, new PaymentUpdate
                {
                    Status = new System.Data.SqlTypes.SqlString("APPROVED"),
                    DateApproved = DateTime.Now
                });

                response.Success = true;
                return response;



            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Payment approval failed: {ex.Message}";
                return response;
            }
        }
        public async Task<ProcessPaymentResponse> DeclinePayment(int paymentId)
        {
            ProcessPaymentResponse response = new();
            try
            {
                var payment = await _unitOfWork.Payments.RetrieveAsync(paymentId);

                if (payment == null)
                {
                    response.ErrorMessage = "Payment not found.";
                    return response;
                }

                if (payment.Status != "PENDING")
                {
                    response.ErrorMessage = "Only pending payments can be rejected.";
                    return response;
                }

                await _unitOfWork.Payments.UpdateAsync(payment.PaymentId, new PaymentUpdate
                {
                    Status = new System.Data.SqlTypes.SqlString("DECLINED"),
                    DateApproved = DateTime.Now
                });

                response.Success = true;
                return response;


            }
            catch (Exception ex)
            {
                response.ErrorMessage = $"Payment rejecting failed: {ex.Message}";
                return response;
            }
        }
    }
}
