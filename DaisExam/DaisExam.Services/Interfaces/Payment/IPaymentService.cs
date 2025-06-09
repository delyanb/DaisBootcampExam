using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisExam.Services.DTOs.Account;
using DaisExam.Services.DTOs.Payment;

namespace DaisExam.Services.Interfaces.Payment
{
    public interface IPaymentService
    {
        Task<GetPaymentInfosResponse> GetPaymentsForUser(int userId);
        Task<GetPaymentInfosResponse> GetPaymentsForAccount(int accountId);
        Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request);
        Task<ProcessPaymentResponse> DeclinePayment(int paymentId);
        Task<ProcessPaymentResponse> ApprovePayment(int paymentId);

    }
}
