using System.Threading.Tasks;
using DaisExam.Attributes;
using DaisExam.Helpers;
using DaisExam.Models.ViewModels.Payment;
using DaisExam.Services.DTOs.Account;
using DaisExam.Services.DTOs.Payment;
using DaisExam.Services.Interfaces.Account;
using DaisExam.Services.Interfaces.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DaisExam.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IAccountService _accountService;

        public PaymentsController(IPaymentService paymentService
            , IAccountService accountService)
        {
         _paymentService = paymentService;
            _accountService = accountService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;

            var paymentInfosResponse = await _paymentService.GetPaymentsForUser(userId);
            if (!paymentInfosResponse.Success)
            {
                TempData["Error"] = paymentInfosResponse.ErrorMessage;
                return RedirectToAction("Index", "Home");
            }

            PaymentListViewModelSeperated viewModel = new();
            List<PaymentVM> allPayments = new();

            foreach (var info in paymentInfosResponse.PaymentInfos)
            {
                var paymentVm = MappingHelper.MapToPaymentVM(info);
                paymentVm.UserIsAccountOwner = info.UserIsAccountOwner; // already present
                allPayments.Add(paymentVm);
            }

            viewModel.PendingPayments = allPayments
                .Where(p => p.Status == "PENDING" && p.UserIsAccountOwner)
                .OrderByDescending(p => p.DateTimeMade)
                .ToList();

            viewModel.ApprovedPayments = allPayments
                .Where(p => p.Status == "APPROVED")
                .OrderByDescending(p => p.DateApproved)
                .ToList();

            return View(viewModel);
        }
        public async Task<IActionResult> Create(int fromAccountId)
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;

            GetAccountDetailsRequest request = new() { UserId = userId,AccountId = fromAccountId };

            var accountResponse = await _accountService.GetAccountDetails(request);
            if (!accountResponse.Success)
            {
                TempData["Error"] = accountResponse.ErrorMessage;
                return RedirectToAction("Index");
            }

            var allAccountsResponse = await _accountService.GetAllOtherAccountInfos(fromAccountId);
            if (!allAccountsResponse.Success)
            {
                TempData["Error"] = allAccountsResponse.ErrorMessage;
                return RedirectToAction("Index");
            }

            var viewModel = new CreatePaymentViewModel
            {
                FromAccountId = fromAccountId,
                FromAccountNumber = accountResponse.AccountDetailsDto.AccountNumber,
                ToAccountOptions = allAccountsResponse.AccountInfos
                    .Select(a => new SelectListItem
                    {
                        Value = a.AccountId.ToString(),
                        Text = a.AccountNumber
                    }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;

            if (!ModelState.IsValid)
            {
                // Re-populate ToAccountOptions in case of validation error
                var allAccountsResponse = await _accountService.GetAllOtherAccountInfos(model.FromAccountId);
                if (allAccountsResponse.Success)
                {
                    model.ToAccountOptions = allAccountsResponse.AccountInfos
                        .Where(a => a.AccountId != model.FromAccountId)
                        .Select(a => new SelectListItem
                        {
                            Value = a.AccountId.ToString(),
                            Text = a.AccountNumber
                        }).ToList();
                }

                TempData["Error"] = "Please correct the form and try again.";
                return View(model);
            }

            var request = new CreatePaymentRequest
            {
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId,
                Amount = model.Amount,
                Reason = model.Reason,
                UserId = userId
            };

            var response = await _paymentService.CreatePayment(request);

            if (!response.Success)
            {
                TempData["Error"] = response.ErrorMessage;
                var allAccountsResponse = await _accountService.GetAllOtherAccountInfos(model.FromAccountId);
                if (allAccountsResponse.Success)
                {
                    model.ToAccountOptions = allAccountsResponse.AccountInfos
                        .Where(a => a.AccountId != model.FromAccountId)
                        .Select(a => new SelectListItem
                        {
                            Value = a.AccountId.ToString(),
                            Text = a.AccountNumber
                        }).ToList();
                }

                return View(model);
            }

            TempData["Success"] = "Payment created successfully! Now Pending!";
            return RedirectToAction("Details", "Accounts", new { accountId = model.FromAccountId });
        }
        [HttpPost]
        public async Task<IActionResult> ApprovePayment(int paymentId, int accountId)
        {
            var response = await _paymentService.ApprovePayment(paymentId);

            if (!response.Success)
            {
                TempData["Error"] = response.ErrorMessage;
            }
            else
            {
                TempData["Success"] = "Payment approved successfully.";
            }

            return RedirectToAction("Details","Accounts", new { accountId });
        }
        [HttpPost]
        public async Task<IActionResult> RejectPayment(int paymentId, int accountId)
        {
            var response = await _paymentService.DeclinePayment(paymentId);

            if (!response.Success)
            {
                TempData["Error"] = response.ErrorMessage;
            }
            else
            {
                TempData["Success"] = "Payment rejected successfully.";
            }

            return RedirectToAction("Details", "Accounts", new { accountId });
        }
    }
}
