using System.Threading.Tasks;
using DaisExam.Attributes;
using DaisExam.Helpers;
using DaisExam.Models.ViewModels.Account;
using DaisExam.Models.ViewModels.Payment;
using DaisExam.Services.DTOs.Account;
using DaisExam.Services.Interfaces.Account;
using DaisExam.Services.Interfaces.Payment;
using Microsoft.AspNetCore.Mvc;

namespace DaisExam.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService,
            IPaymentService paymentService)
        {
            _accountService = accountService;
            _paymentService = paymentService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;

            var getMyAccountsResponse = await _accountService.GetAccountsForUser(userId);
            if (!getMyAccountsResponse.Success)
            {
                TempData["Error"] = getMyAccountsResponse.ErrorMessage;
            }

            AccountInfosListViewModel viewModel = new();
            List<AccountInfoVM> AccountVMs = new();

            foreach (var a in getMyAccountsResponse.AccountInfos)
            {
                AccountVMs.Add(MappingHelper.MapToAccountInfoVM(a));
            }

            viewModel.AccountVMs = AccountVMs;
            return View(viewModel);



        }
        public async Task<IActionResult> Details(int accountId)
        {

            var userId = HttpContext.Session.GetInt32("UserId").Value;

            GetAccountDetailsRequest request = new()
            {
                AccountId = accountId,
                UserId = userId
            };

            var getAccountDetailsResponse = await _accountService.GetAccountDetails(request);
            if(!getAccountDetailsResponse.Success)
            {
                TempData["Error"] = getAccountDetailsResponse.ErrorMessage;
                return RedirectToAction("Index");
            }

            AccountDetailsViewModel viewModel = new();

            viewModel.AccountDetailsInfo = MappingHelper.MapToAccountDetailsDto(getAccountDetailsResponse.AccountDetailsDto);

            var paymentInfosResponse = await _paymentService.GetPaymentsForAccount(accountId);

            if (!paymentInfosResponse.Success)
            {
                TempData["Error"] = paymentInfosResponse.ErrorMessage;
                return RedirectToAction("Index", "Home");
            }
            List<PaymentVM> PaymentVMs = new();

            foreach (var info in paymentInfosResponse.PaymentInfos)
            {
                PaymentVMs.Add(MappingHelper.MapToPaymentVM(info));
            }
            viewModel.PaymentList = new();
            viewModel.PaymentList.PaymentVMs = PaymentVMs;

            return View(viewModel);






        }
        public async Task<IActionResult> Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId").Value;

            var response = await _accountService.CreateAccount(new CreateAccountRequest
            {
                UserId = userId
            });

            if (!response.Success)
            {
                TempData["Error"] = response.ErrorMessage;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Account successfully created!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int accountId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var request = new RemoveAccountRequest
            {
                UserId = userId.Value,
                AccountId = accountId
            };

            var response = await _accountService.RemoveAccount(request);

            if (!response.Success)
            {
                TempData["Error"] = response.ErrorMessage;
            }
            else if (response.Deleted)
            {
                TempData["Success"] = $"Account #{response.AccountNumberRemoved} was fully deleted, no one else was in it.";
            }
            else
            {
                TempData["Success"] = "Account successfully removed from your profile.";
            }

            return RedirectToAction("Index");


        }
        public async Task<IActionResult> Add(int accountId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");


            var response = await _accountService.GetAccountsThatAreNotFromUser(userId.Value);

            if (!response.Success)
            {
                TempData["Error"] = response.ErrorMessage;
                return RedirectToAction("Index");
            }

            var viewModel = new AccountInfosListViewModel
            {
                AccountVMs = response.AccountInfos.Select(MappingHelper.MapToAccountInfoVM).ToList()
            };

            return View(viewModel);


        }
        [HttpPost]
        public async Task<IActionResult> AddToAccounts(int accountId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var response = await _accountService.AddToMyAccounts(userId.Value, accountId);

            if (!response.Success)
            {
                TempData["Error"] = response.ErrorMessage;
            }
            else
            {
                TempData["Success"] = "Account successfully added to your profile.";
            }

            return RedirectToAction("Add");


        }
    }
}
