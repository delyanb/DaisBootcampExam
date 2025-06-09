using DaisExam.Data.Implementations.Account;
using DaisExam.Services.Implementations.Account;
using DaisExam.Services.Implementations.Authentication;
using DaisExam.Services.Implementations.Payment;
using DaisExam.Services.Interfaces.Account;
using DaisExam.Services.Interfaces.Authentication;
using DaisExam.Services.Interfaces.Payment;
using DeisExam.Data;
using DeisExam.Data.Implementations.Payment;
using DeisExam.Data.Implementations.User;
using DeisExam.Data.Implementations.UserAccount;
using DeisExam.Data.Interfaces.Account;
using DeisExam.Data.Interfaces.Payment;
using DeisExam.Data.Interfaces.User;
using DeisExam.Data.Interfaces.UserAccount;
using DeisExam.Data.UnitOfWork;
using Microsoft.AspNetCore.Connections;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Register services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Initialize connection factory
ConnectionFactory.Initialize(
    builder.Configuration.GetConnectionString("DefaultConnection"));

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();