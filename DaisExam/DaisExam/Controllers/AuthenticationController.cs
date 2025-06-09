using DaisExam.Models.ViewModels.Authentication;
using DaisExam.Services.DTOs.Authentication;
using DaisExam.Services.Interfaces.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DaisExam.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginViewModel() { ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(new LoginRequest
            {
                Username = model.Username,
                Password = model.Password
            });

            if (result.Success)
            {
                HttpContext.Session.SetInt32("UserId", result.EmployeeId.Value);
                HttpContext.Session.SetString("UserName", result.FullName);
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                return RedirectToAction("Index", "Home");
            }

            ViewData["ErrorMessage"] = result.ErrorMessage ?? "Invalid username or password";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
