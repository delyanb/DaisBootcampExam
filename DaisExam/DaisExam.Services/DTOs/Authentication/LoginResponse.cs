using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisExam.Services.DTOs.Authentication
{
    public class LoginResponse : Response
    {
        public int? EmployeeId { get; set; }
        public string? FullName { get; set; }
    }
}
