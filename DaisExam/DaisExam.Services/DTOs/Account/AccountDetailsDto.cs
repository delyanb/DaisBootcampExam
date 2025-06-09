using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisExam.Services.DTOs.Account
{
    public class AccountDetailsDto
    {
        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        public List<string> OtherOwners { get; set; }

        public decimal AvailableAmount { get; set; }
    }
}
