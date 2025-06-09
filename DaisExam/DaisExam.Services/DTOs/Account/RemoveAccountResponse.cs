using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisExam.Services.DTOs.Account
{
    public class RemoveAccountResponse : Response
    {
        public string AccountNumberRemoved { get; set; }
        public bool Deleted { get; set; } = false;

    }
}
