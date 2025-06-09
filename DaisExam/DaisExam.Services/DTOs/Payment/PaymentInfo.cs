using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaisExam.Services.DTOs.Payment
{
    public class PaymentInfo
    {
        public int PaymentId { get; set; }
        public int FromAccountId { get; set; }
        public string FromAccountNumber { get; set; }
        public int ToAccountId { get; set; }
        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Reason { get; set; }
        public DateTime DateTimeMade { get; set; }
        public bool UserIsAccountOwner { get; set; }
        public string Status { get; set; }
        public DateTime? DateApproved { get; set; }
    }
}
