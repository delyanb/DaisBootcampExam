using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisExam.Services.DTOs.Authentication;
using DaisExam.Services.Helpers;
using DaisExam.Services.Interfaces.Authentication;
using DeisExam.Data.Interfaces.User;

namespace DaisExam.Services.Implementations.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Username and password are required"
                };
            }

            var hashedPassword = SecurityHelper.HashPassword(request.Password);
            var filter = new UserFilter { Username = new SqlString(request.Username) };

            var employees = await _userRepository.RetrieveCollectionAsync(filter).ToListAsync();
            var employee = employees.SingleOrDefault();

            if (employee == null || employee.PasswordHash != hashedPassword)
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Invalid username or password"
                };
            }

            return new LoginResponse
            {
                Success = true,
                EmployeeId = employee.UserId,
                FullName = employee.FullName
            };
        }
    }
}
