using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaisExam.Services.DTOs.Authentication;

namespace DaisExam.Services.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
