using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisExam.Services.DTOs.Account
{
    public class GetAccountDetailsResponse : Response
    {
        public AccountDetailsDto AccountDetailsDto { get; set; }
    }
}
